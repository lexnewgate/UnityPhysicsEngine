using System;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace FixedMath
{
    public struct Vector2 : IEquatable<Vector2>, IFormattable
    {
        #region Static Properties
        public static Vector2 One = new Vector2(1, 1);
        public static Vector2 Zero = new Vector2(0, 0);
        #endregion

        #region Properties
        public float x, y;

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
                return (index == 0) ? x : y;
            }
            set
            {
                if (index == 0)
                {
                    x = value;
                }
                else
                {
                    y = value;
                }
            }
        }

        public void Normalize()
        {
            this=Normalize(this);
        }

        public Vector2 Normalized
        {
            get
            {
                return Normalize(this);
            }
        }

        public static Vector2 Normalize(Vector2 vector)
        {
            if (vector.MagnitudeSqr == 0)
            {
                return Vector2.Zero;
            }
            return vector * (1 / vector.Magnitude);
        }

        #endregion

        #region Constructors
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        #endregion

        #region Static Methods
        public static float Dot(Vector2 a, Vector2 b)
        {
            return a.x * b.x + a.y * b.y;
        }


        //radians
        public static float Angle(Vector2 l, Vector2 r)
        {
            float m = Mathf.Sqrt(l.MagnitudeSqr * r.MagnitudeSqr);
            if (m == 0)
            {
                return 0;
            }
            return Mathf.Acos(Dot(l, r) / m);
        }

        public static Vector2 Project(Vector2 vector, Vector2 direction)
        {
            float dot = Dot(vector, direction);
            float magSq = direction.MagnitudeSqr;
            return direction * (dot / magSq);
        }

        public static Vector2 Perpendicular(Vector2 vector, Vector2 direction)
        {
            return vector - Project(vector, direction);
        }

        public static Vector2 Reflection(Vector2 vector, Vector2 normal)
        {
            float d = Dot(vector, normal);
            return vector - normal * (d * 2);
        }

        #endregion

        #region Equality
        public bool Equals(Vector2 other)
        {
            return x == other.x &&
               y == other.y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2))
                return false;
            return Equals((Vector2)obj);
        }

        public override int GetHashCode()
        {
            int hash = this.x.GetHashCode();
            hash = HashCodeHelper.CombineHashCodes(hash, this.y.GetHashCode());
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
            sb.Append('>');
            return sb.ToString();
        }
        #endregion

        #region Operators
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator *(Vector2 a, float scalar)
        {
            return new Vector2(a.x * scalar, a.y * scalar);
        }

        public static Vector2 operator *(float scalar, Vector2 a)
        {
            return new Vector2(a.x * scalar, a.y * scalar);
        }

        public static float operator *(Vector2 a, Vector2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !(a == b);
        }
        #endregion
    }
}