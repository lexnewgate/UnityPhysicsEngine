using System;
using System.Numerics;
namespace FixedMath
{
    public struct Matrix4x4
    {
        public float m11, m12, m13, m14;
        public float m21, m22, m23, m24;
        public float m31, m32, m33, m34;
        public float m41, m42, m43, m44;

        private static readonly Matrix4x4 _identity = new Matrix4x4
        (
            1f, 0f, 0f, 0f,
            0f, 1f, 0f, 0f,
            0f, 0f, 1f, 0f,
            0f, 0f, 0f, 1f
        );

        public static Matrix4x4 Identity
        {
            get { return _identity; }
        }

        public bool IsIdentity
        {
            get
            {
                return m11 == 1f && m22 == 1f && m33 == 1f && m44 == 1f && // Check diagonal element first for early out.
                                    m12 == 0f && m13 == 0f && m14 == 0f &&
                       m21 == 0f && m23 == 0f && m24 == 0f &&
                       m31 == 0f && m32 == 0f && m34 == 0f &&
                       m41 == 0f && m42 == 0f && m43 == 0f;
            }
        }


        public Matrix4x4(float m11, float m12, float m13, float m14,
                            float m21, float m22, float m23, float m24,
                            float m31, float m32, float m33, float m34,
                            float m41, float m42, float m43, float m44)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m14 = m14;

            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m24 = m24;

            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
            this.m34 = m34;

            this.m41 = m41;
            this.m42 = m42;
            this.m43 = m43;
            this.m44 = m44;
        }


        public float GetDeterminant()
        {
            // | a b c d |     | f g h |     | e g h |     | e f h |     | e f g |
            // | e f g h | = a | j k l | - b | i k l | + c | i j l | - d | i j k |
            // | i j k l |     | n o p |     | m o p |     | m n p |     | m n o |
            // | m n o p |
            //
            //   | f g h |
            // a | j k l | = a ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
            //   | n o p |
            //
            //   | e g h |     
            // b | i k l | = b ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
            //   | m o p |     
            //
            //   | e f h |
            // c | i j l | = c ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
            //   | m n p |
            //
            //   | e f g |
            // d | i j k | = d ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
            //   | m n o |
            //
            // Cost of operation
            // 17 adds and 28 muls.
            //
            // add: 6 + 8 + 3 = 17
            // mul: 12 + 16 = 28

            float a = m11, b = m12, c = m13, d = m14;
            float e = m21, f = m22, g = m23, h = m24;
            float i = m31, j = m32, k = m33, l = m34;
            float m = m41, n = m42, o = m43, p = m44;

            float kp_lo = k * p - l * o;
            float jp_ln = j * p - l * n;
            float jo_kn = j * o - k * n;
            float ip_lm = i * p - l * m;
            float io_km = i * o - k * m;
            float in_jm = i * n - j * m;

            return a * (f * kp_lo - g * jp_ln + h * jo_kn) -
                   b * (e * kp_lo - g * ip_lm + h * io_km) +
                   c * (e * jp_ln - f * ip_lm + h * in_jm) -
                   d * (e * jo_kn - f * io_km + g * in_jm);
        }



        public static Matrix4x4 Transpose(Matrix4x4 matrix)
        {
            Matrix4x4 result;

            result.m11 = matrix.m11;
            result.m12 = matrix.m21;
            result.m13 = matrix.m31;
            result.m14 = matrix.m41;
            result.m21 = matrix.m12;
            result.m22 = matrix.m22;
            result.m23 = matrix.m32;
            result.m24 = matrix.m42;
            result.m31 = matrix.m13;
            result.m32 = matrix.m23;
            result.m33 = matrix.m33;
            result.m34 = matrix.m43;
            result.m41 = matrix.m14;
            result.m42 = matrix.m24;
            result.m43 = matrix.m34;
            result.m44 = matrix.m44;

            return result;
        }

        /// <summary>
        /// Attempts to calculate the inverse of the given matrix. If successful, result will contain the inverted matrix.
        /// </summary>
        /// <param name="matrix">The source matrix to invert.</param>
        /// <param name="result">If successful, contains the inverted matrix.</param>
        /// <returns>True if the source matrix could be inverted; False otherwise.</returns>
        public static bool Invert(Matrix4x4 matrix, out Matrix4x4 result)
        {
            //                                       -1
            // If you have matrix M, inverse Matrix M   can compute
            //
            //     -1       1      
            //    M   = --------- A
            //            det(M)
            //
            // A is adjugate (adjoint) of M, where,
            //
            //      T
            // A = C
            //
            // C is Cofactor matrix of M, where,
            //           i + j
            // C   = (-1)      * det(M  )
            //  ij                    ij
            //
            //     [ a b c d ]
            // M = [ e f g h ]
            //     [ i j k l ]
            //     [ m n o p ]
            //
            // First Row
            //           2 | f g h |
            // C   = (-1)  | j k l | = + ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
            //  11         | n o p |
            //
            //           3 | e g h |
            // C   = (-1)  | i k l | = - ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
            //  12         | m o p |
            //
            //           4 | e f h |
            // C   = (-1)  | i j l | = + ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
            //  13         | m n p |
            //
            //           5 | e f g |
            // C   = (-1)  | i j k | = - ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
            //  14         | m n o |
            //
            // Second Row
            //           3 | b c d |
            // C   = (-1)  | j k l | = - ( b ( kp - lo ) - c ( jp - ln ) + d ( jo - kn ) )
            //  21         | n o p |
            //
            //           4 | a c d |
            // C   = (-1)  | i k l | = + ( a ( kp - lo ) - c ( ip - lm ) + d ( io - km ) )
            //  22         | m o p |
            //
            //           5 | a b d |
            // C   = (-1)  | i j l | = - ( a ( jp - ln ) - b ( ip - lm ) + d ( in - jm ) )
            //  23         | m n p |
            //
            //           6 | a b c |
            // C   = (-1)  | i j k | = + ( a ( jo - kn ) - b ( io - km ) + c ( in - jm ) )
            //  24         | m n o |
            //
            // Third Row
            //           4 | b c d |
            // C   = (-1)  | f g h | = + ( b ( gp - ho ) - c ( fp - hn ) + d ( fo - gn ) )
            //  31         | n o p |
            //
            //           5 | a c d |
            // C   = (-1)  | e g h | = - ( a ( gp - ho ) - c ( ep - hm ) + d ( eo - gm ) )
            //  32         | m o p |
            //
            //           6 | a b d |
            // C   = (-1)  | e f h | = + ( a ( fp - hn ) - b ( ep - hm ) + d ( en - fm ) )
            //  33         | m n p |
            //
            //           7 | a b c |
            // C   = (-1)  | e f g | = - ( a ( fo - gn ) - b ( eo - gm ) + c ( en - fm ) )
            //  34         | m n o |
            //
            // Fourth Row
            //           5 | b c d |
            // C   = (-1)  | f g h | = - ( b ( gl - hk ) - c ( fl - hj ) + d ( fk - gj ) )
            //  41         | j k l |
            //
            //           6 | a c d |
            // C   = (-1)  | e g h | = + ( a ( gl - hk ) - c ( el - hi ) + d ( ek - gi ) )
            //  42         | i k l |
            //
            //           7 | a b d |
            // C   = (-1)  | e f h | = - ( a ( fl - hj ) - b ( el - hi ) + d ( ej - fi ) )
            //  43         | i j l |
            //
            //           8 | a b c |
            // C   = (-1)  | e f g | = + ( a ( fk - gj ) - b ( ek - gi ) + c ( ej - fi ) )
            //  44         | i j k |
            //
            // Cost of operation
            // 53 adds, 104 muls, and 1 div.
            float a = matrix.m11, b = matrix.m12, c = matrix.m13, d = matrix.m14;
            float e = matrix.m21, f = matrix.m22, g = matrix.m23, h = matrix.m24;
            float i = matrix.m31, j = matrix.m32, k = matrix.m33, l = matrix.m34;
            float m = matrix.m41, n = matrix.m42, o = matrix.m43, p = matrix.m44;

            float kp_lo = k * p - l * o;
            float jp_ln = j * p - l * n;
            float jo_kn = j * o - k * n;
            float ip_lm = i * p - l * m;
            float io_km = i * o - k * m;
            float in_jm = i * n - j * m;

            float a11 = +(f * kp_lo - g * jp_ln + h * jo_kn);
            float a12 = -(e * kp_lo - g * ip_lm + h * io_km);
            float a13 = +(e * jp_ln - f * ip_lm + h * in_jm);
            float a14 = -(e * jo_kn - f * io_km + g * in_jm);

            float det = a * a11 + b * a12 + c * a13 + d * a14;

            if (System.Math.Abs(det) < float.Epsilon)
            {
                result = new Matrix4x4(float.NaN, float.NaN, float.NaN, float.NaN,
                                       float.NaN, float.NaN, float.NaN, float.NaN,
                                       float.NaN, float.NaN, float.NaN, float.NaN,
                                       float.NaN, float.NaN, float.NaN, float.NaN);
                return false;
            }

            float invDet = 1.0f / det;

            result.m11 = a11 * invDet;
            result.m21 = a12 * invDet;
            result.m31 = a13 * invDet;
            result.m41 = a14 * invDet;

            result.m12 = -(b * kp_lo - c * jp_ln + d * jo_kn) * invDet;
            result.m22 = +(a * kp_lo - c * ip_lm + d * io_km) * invDet;
            result.m32 = -(a * jp_ln - b * ip_lm + d * in_jm) * invDet;
            result.m42 = +(a * jo_kn - b * io_km + c * in_jm) * invDet;

            float gp_ho = g * p - h * o;
            float fp_hn = f * p - h * n;
            float fo_gn = f * o - g * n;
            float ep_hm = e * p - h * m;
            float eo_gm = e * o - g * m;
            float en_fm = e * n - f * m;

            result.m13 = +(b * gp_ho - c * fp_hn + d * fo_gn) * invDet;
            result.m23 = -(a * gp_ho - c * ep_hm + d * eo_gm) * invDet;
            result.m33 = +(a * fp_hn - b * ep_hm + d * en_fm) * invDet;
            result.m43 = -(a * fo_gn - b * eo_gm + c * en_fm) * invDet;

            float gl_hk = g * l - h * k;
            float fl_hj = f * l - h * j;
            float fk_gj = f * k - g * j;
            float el_hi = e * l - h * i;
            float ek_gi = e * k - g * i;
            float ej_fi = e * j - f * i;

            result.m14 = -(b * gl_hk - c * fl_hj + d * fk_gj) * invDet;
            result.m24 = +(a * gl_hk - c * el_hi + d * ek_gi) * invDet;
            result.m34 = -(a * fl_hj - b * el_hi + d * ej_fi) * invDet;
            result.m44 = +(a * fk_gj - b * ek_gi + c * ej_fi) * invDet;

            return true;
        }



        public static Matrix4x4 operator *(Matrix4x4 matrix, float scalar)
        {
            Matrix4x4 result;

            result.m11 = matrix.m11 * scalar;
            result.m12 = matrix.m12 * scalar;
            result.m13 = matrix.m13 * scalar;
            result.m14 = matrix.m14 * scalar;
            result.m21 = matrix.m21 * scalar;
            result.m22 = matrix.m22 * scalar;
            result.m23 = matrix.m23 * scalar;
            result.m24 = matrix.m24 * scalar;
            result.m31 = matrix.m31 * scalar;
            result.m32 = matrix.m32 * scalar;
            result.m33 = matrix.m33 * scalar;
            result.m34 = matrix.m34 * scalar;
            result.m41 = matrix.m41 * scalar;
            result.m42 = matrix.m42 * scalar;
            result.m43 = matrix.m43 * scalar;
            result.m44 = matrix.m44 * scalar;

            return result;
        }

        public static Matrix4x4 operator *(Matrix4x4 value1, Matrix4x4 value2)
        {
            Matrix4x4 result;

            // First row
            result.m11 = value1.m11 * value2.m11 + value1.m12 * value2.m21 + value1.m13 * value2.m31 + value1.m14 * value2.m41;
            result.m12 = value1.m11 * value2.m12 + value1.m12 * value2.m22 + value1.m13 * value2.m32 + value1.m14 * value2.m42;
            result.m13 = value1.m11 * value2.m13 + value1.m12 * value2.m23 + value1.m13 * value2.m33 + value1.m14 * value2.m43;
            result.m14 = value1.m11 * value2.m14 + value1.m12 * value2.m24 + value1.m13 * value2.m34 + value1.m14 * value2.m44;

            // Second row
            result.m21 = value1.m21 * value2.m11 + value1.m22 * value2.m21 + value1.m23 * value2.m31 + value1.m24 * value2.m41;
            result.m22 = value1.m21 * value2.m12 + value1.m22 * value2.m22 + value1.m23 * value2.m32 + value1.m24 * value2.m42;
            result.m23 = value1.m21 * value2.m13 + value1.m22 * value2.m23 + value1.m23 * value2.m33 + value1.m24 * value2.m43;
            result.m24 = value1.m21 * value2.m14 + value1.m22 * value2.m24 + value1.m23 * value2.m34 + value1.m24 * value2.m44;

            // Third row
            result.m31 = value1.m31 * value2.m11 + value1.m32 * value2.m21 + value1.m33 * value2.m31 + value1.m34 * value2.m41;
            result.m32 = value1.m31 * value2.m12 + value1.m32 * value2.m22 + value1.m33 * value2.m32 + value1.m34 * value2.m42;
            result.m33 = value1.m31 * value2.m13 + value1.m32 * value2.m23 + value1.m33 * value2.m33 + value1.m34 * value2.m43;
            result.m34 = value1.m31 * value2.m14 + value1.m32 * value2.m24 + value1.m33 * value2.m34 + value1.m34 * value2.m44;

            // Fourth row
            result.m41 = value1.m41 * value2.m11 + value1.m42 * value2.m21 + value1.m43 * value2.m31 + value1.m44 * value2.m41;
            result.m42 = value1.m41 * value2.m12 + value1.m42 * value2.m22 + value1.m43 * value2.m32 + value1.m44 * value2.m42;
            result.m43 = value1.m41 * value2.m13 + value1.m42 * value2.m23 + value1.m43 * value2.m33 + value1.m44 * value2.m43;
            result.m44 = value1.m41 * value2.m14 + value1.m42 * value2.m24 + value1.m43 * value2.m34 + value1.m44 * value2.m44;

            return result;
        }

    }
}