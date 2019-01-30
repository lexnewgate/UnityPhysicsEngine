
//Extensions for FixMath.NET

namespace FixMath.NET
{
    public partial struct Fix64
    {
        #region Operator "-"
        public static Fix64 operator -(Fix64 x, int y)
        {
            return x - (Fix64)y;
        }

        public static Fix64 operator -(int x, Fix64 y)
        {
            return (Fix64)x - y;
        }

        public static Fix64 operator -(Fix64 x, float y)
        {
            return x - (Fix64)y;
        }

        public static Fix64 operator -(float x, Fix64 y)
        {
            return (Fix64)x + y;
        }

        public static Fix64 operator -(Fix64 x, double y)
        {
            return x - (Fix64)y;
        }

        public static Fix64 operator -(double x, Fix64 y)
        {
            return (Fix64)x - y;
        }
        #endregion

        #region Operator "/"
        public static Fix64 operator /(int x, Fix64 y)
        {
            return (Fix64)x / y;
        }
        #endregion

        #region Operator explicit
        public static explicit operator int(Fix64 value)
        {
            return (int)((float)value);
        }

        #endregion


    }
}