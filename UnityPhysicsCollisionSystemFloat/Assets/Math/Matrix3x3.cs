using System;

namespace FixedMath
{
    public struct Matrix3x3
    {
        public float m11, m12, m13;
        public float m21, m22, m23;
        public float m31, m32, m33;


        private static readonly Matrix3x3 _identity = new Matrix3x3
        (
            1f, 0f, 0f,
            0f, 1f, 0f,
            0f, 0f, 1f
        );

        public static Matrix3x3 Identity
        {
            get { return _identity; }
        }

        public bool IsIdentity
        {
            get
            {
                return m11 == 1f && m22 == 1f && m33 == 1f &&  // Check diagonal element first for early out.
                       m12 == 0f && m13 == 0f &&
                       m21 == 0f && m23 == 0f &&
                       m31 == 0f && m32 == 0f;
            }
        }


        public Matrix3x3(float m11, float m12, float m13,
                            float m21, float m22, float m23,
                            float m31, float m32, float m33)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;

            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;

            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;

        }

        public float GetDeterminant()
        {
            //  |a b c|
            //  |d e f| = a|e f|  - b |d f|  + c |d e| 
            //  |g h i|    |h i|      |g i|      |g h|

            float a = m11, b = m12, c = m13;
            float d = m21, e = m22, f = m23;
            float g = m31, h = m32, i = m33;

            return a * (e * i - f * h) - b * (d * i - f * g) + c * (d * h - e * g);
        }



        public static Matrix3x3 Transpose(Matrix3x3 matrix)
        {
            Matrix3x3 result;

            result.m11 = matrix.m11;
            result.m12 = matrix.m21;
            result.m13 = matrix.m31;
            result.m21 = matrix.m12;
            result.m22 = matrix.m22;
            result.m23 = matrix.m32;
            result.m31 = matrix.m13;
            result.m32 = matrix.m23;
            result.m33 = matrix.m33;

            return result;
        }

        //        [a b c]
        //   M=   [d e f]
        //        [g h i]

        // matrix of minors
        //  [ei-fh di-fg dh-eg]
        //  [bi-ch ai-cg ah-bg] 
        //  [bf-ce af-cd ae-bd]
        //

        //cofactor
        //  [ei-fh   -di+fg   dh-eg]
        //  [-bi+ch   ai-cg  -ah+bg] 
        //  [bf-ce   -af+cd   ae-bd]

        //adjoint
        //  [ei-fh    -bi+ch    bf-ce]
        //  [-di+fg    ai-cg   -af+cd]
        //  [dh-eg    -ah+bg    ae-bd]

        // det(M)= a(ei-fh)-b(di-fg)+c(dh-eg)

        //   -1          1 
        //  M    =    -------- Adjoint
        //              det(M)
        public static bool Invert(Matrix3x3 matrix, out Matrix3x3 result)
        {
            float a = matrix.m11, b = matrix.m12, c = matrix.m13;
            float d = matrix.m21, e = matrix.m22, f = matrix.m23;
            float g = matrix.m31, h = matrix.m32, i = matrix.m33;

            float det = a * (e * i - f * h) - b * (d * i - f * g) + c * (d * h - e * g);
            if (det < float.Epsilon)
            {
                result = new Matrix3x3(
                    float.NaN, float.NaN, float.NaN,
                    float.NaN, float.NaN, float.NaN,
                    float.NaN, float.NaN, float.NaN);
                return false;
            }

            float invDet = 1 / det;
            float a11 = e * i - f * h, a12 = -b * i + c * h, a13 = b * f - c * e;
            float a21 = -d * i + f * g, a22 = a * i - c * g, a23 = -a * f + c * d;
            float a31 = d * h - e * g, a32 = -a * h + b * g, a33 = a * e - b * d;

            result.m11 = a11 * invDet;
            result.m12 = a12 * invDet;
            result.m13 = a13 * invDet;
            result.m21 = a21 * invDet;
            result.m22 = a22 * invDet;
            result.m23 = a23 * invDet;
            result.m31 = a31 * invDet;
            result.m32 = a32 * invDet;
            result.m33 = a33 * invDet;

            return true;
        }


        public static Matrix3x3 operator *(Matrix3x3 matrix, float scalar)
        {
            Matrix3x3 result;

            result.m11 = matrix.m11 * scalar;
            result.m12 = matrix.m12 * scalar;
            result.m13 = matrix.m13 * scalar;
            result.m21 = matrix.m21 * scalar;
            result.m22 = matrix.m22 * scalar;
            result.m23 = matrix.m23 * scalar;
            result.m31 = matrix.m31 * scalar;
            result.m32 = matrix.m32 * scalar;
            result.m33 = matrix.m33 * scalar;

            return result;
        }


        public static Matrix3x3 operator *(Matrix3x3 value1, Matrix3x3 value2)
        {
            Matrix3x3 result;

            // First row
            result.m11 = value1.m11 * value2.m11 + value1.m12 * value2.m21 + value1.m13 * value2.m31;
            result.m12 = value1.m11 * value2.m12 + value1.m12 * value2.m22 + value1.m13 * value2.m32;
            result.m13 = value1.m11 * value2.m13 + value1.m12 * value2.m23 + value1.m13 * value2.m33;

            // Second row
            result.m21 = value1.m21 * value2.m11 + value1.m22 * value2.m21 + value1.m23 * value2.m31;
            result.m22 = value1.m21 * value2.m12 + value1.m22 * value2.m22 + value1.m23 * value2.m32;
            result.m23 = value1.m21 * value2.m13 + value1.m22 * value2.m23 + value1.m23 * value2.m33;

            // Third row
            result.m31 = value1.m31 * value2.m11 + value1.m32 * value2.m21 + value1.m33 * value2.m31;
            result.m32 = value1.m31 * value2.m12 + value1.m32 * value2.m22 + value1.m33 * value2.m32;
            result.m33 = value1.m31 * value2.m13 + value1.m32 * value2.m23 + value1.m33 * value2.m33;

            return result;
        }

    }
}