using System;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace FixedMath
{
    public struct Vector3 : IEquatable<Vector3>, IFormattable
    {
        #region Static Properties
        public static Vector3 One = new Vector3(1, 1, 1);
        public static Vector3 Zero = new Vector3(0, 0, 0);
        #endregion

        #region Properties
        public float x, y, z;

        public float MagnitudeSqr
        {
            get
            {
                return Dot(this, this);
            }
        }

        public float Magnitude
        {
            get
            {
                return Mathf.Sqrt(MagnitudeSqr);
            }
        }
        public float this[int index]
        {
            get
            {
                return (index == 0) ? x : (index == 1) ? y : z;
            }
            set
            {
                if (index == 0)
                {
                    x = value;
                }
                else if (index == 1)
                {
                    y = value;
                }
                else
                {
                    z = value;
                }
            }
        }

        public void Normalize()
        {
            this=Normalize(this);
        }

        public Vector3 Normalized
        {
            get
            {
                return Normalize(this);
            }
        }

        public static Vector3 Normalize(Vector3 vector)
        {
            if (vector.MagnitudeSqr == 0)
            {
                return Vector3.Zero;
            }
            return vector * (1 / vector.Magnitude);
        }
        #endregion

        #region Constructors
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        #endregion

        #region Static Methods
        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        //  | x    y     z  |
        //  | l.x  l.y   l.z|
        //  | r.x  r.y   r.z|
        public static Vector3 Cross(Vector3 l, Vector3 r)
        {
            Vector3 result;
            result.x = l.y * r.z - l.z * r.y;
            result.y = l.x * r.z - l.z * r.x;
            result.z = l.x * r.y - l.y * r.x;
            return result;
        }


        //                 l dot r       l dot r
        //  cos angle=  ----------- = -----------------
        //                 |l|*|r|      sqrt(l^2+ r^2)
        //radians
        public static float Angle(Vector3 l, Vector3 r)
        {
            float m = Mathf.Sqrt(l.MagnitudeSqr * r.MagnitudeSqr);
            if (m == 0)
            {
                return 0;
            }
            return Mathf.Acos(Dot(l, r) / m);
        }

        public static Vector3 Project(Vector3 vector, Vector3 direction)
        {
            float dot = Dot(vector, direction);
            float magSq = direction.MagnitudeSqr;
            return direction * (dot / magSq);
        }

        public static Vector3 Perpendicular(Vector3 vector, Vector3 direction)
        {
            return vector - Project(vector, direction);
        }

        public static Vector3 Reflection(Vector3 vector, Vector3 normal)
        {
            float d = Dot(vector, normal);
            return vector - normal * (d * 2);
        }

        #endregion

        #region Equality
        public bool Equals(Vector3 other)
        {
            return x == other.x &&
               y == other.y &&
               z == other.z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector3))
                return false;
            return Equals((Vector3)obj);
        }

        public override int GetHashCode()
        {
            int hash = this.x.GetHashCode();
            hash = HashCodeHelper.CombineHashCodes(hash, this.y.GetHashCode());
            hash = HashCodeHelper.CombineHashCodes(hash, this.z.GetHashCode());
            return hash;
        }
        #endregion


        #region ToString
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(((IFormattable)this.x).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.y).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.z).ToString(format, formatProvider));
            sb.Append('>');
            return sb.ToString();
        }
        #endregion

        #region Operators
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 operator *(Vector3 a, float scalar)
        {
            return new Vector3(a.x * scalar, a.y * scalar, a.z * scalar);
        }

        public static Vector3 operator *(float scalar, Vector3 a)
        {
            return new Vector3(a.x * scalar, a.y * scalar, a.z * scalar);
        }

        public static float operator *(Vector3 a, Vector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !(a == b);
        }
        #endregion
    }
}