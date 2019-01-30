namespace FixedMath
{
    public struct Matrix2x2
    {
        float m11, m12;
        float m21, m22;

        private static readonly Matrix2x2 _identity = new Matrix2x2
        (
            1f, 0f,
            0f, 1f
        );

        public static Matrix2x2 Identity
        {
            get { return _identity; }
        }

        public bool IsIdentity
        {
            get
            {
                return m11 == 1f && m22 == 1f &&
                       m12 == 0f &&
                       m21 == 0f
                       ;
            }
        }


        public Matrix2x2(float m11, float m12,
                            float m21, float m22
                            )
        {
            this.m11 = m11;
            this.m12 = m12;

            this.m21 = m21;
            this.m22 = m22;
        }

        public float GetDeterminant()
        {
            return m11 * m22 - m12 * m21;
        }

        //  [a  b]
        //  [c  d]
        public static bool Invert(Matrix2x2 matrix, out Matrix2x2 result)
        {
            float a = matrix.m11;
            float b = matrix.m12;
            float c = matrix.m21;
            float d = matrix.m22;
            float det = a * d - b * c;
            if (det < float.Epsilon)
            {
                result = new Matrix2x2();
                return false;
            }
            float invDet = 1 / det;

            result.m11 = d * invDet; result.m12 = -b * invDet;
            result.m21 = -c * invDet; result.m22 = a * invDet;
            return true;
        }

        public static Matrix2x2 Transpose(Matrix2x2 matrix)
        {
            Matrix2x2 result;

            result.m11 = matrix.m11;
            result.m12 = matrix.m21;
            result.m21 = matrix.m12;
            result.m22 = matrix.m22;
            return result;
        }

        public static Matrix2x2 operator *(Matrix2x2 matrix, float scalar)
        {
            Matrix2x2 result;
            result.m11 = matrix.m11 * scalar;
            result.m12 = matrix.m12 * scalar;
            result.m21 = matrix.m21 * scalar;
            result.m22 = matrix.m22 * scalar;
            return result;
        }


        public static Matrix2x2 operator *(Matrix2x2 value1, Matrix2x2 value2)
        {
            Matrix2x2 result;

            // First row
            result.m11 = value1.m11 * value2.m11 + value1.m12 * value2.m21;
            result.m12 = value1.m11 * value2.m12 + value1.m12 * value2.m22;

            // Second row
            result.m21 = value1.m21 * value2.m11 + value1.m22 * value2.m21;
            result.m22 = value1.m21 * value2.m12 + value1.m22 * value2.m22;


            return result;
        }


    }
}