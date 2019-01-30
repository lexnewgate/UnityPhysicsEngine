using UnityEngine;

namespace FixedMath
{
    public class MatrixConverter
    {
        public static Matrix4x4 Translation(float x, float y, float z)
        {
            return new Matrix4x4(
                1, 0, 0, x,
                0, 1, 0, y,
                0, 0, 1, z,
                0, 0, 0, 1
                );
        }

        public static Matrix4x4 Translation(Vector3 pos)
        {
            return new Matrix4x4(
                1, 0, 0, pos.x,
                0, 1, 0, pos.y,
                0, 0, 1, pos.z,
                0, 0, 0, 1
            );
        }

        public static Vector3 GetTranslation(Matrix4x4 mat)
        {
            return new Vector3(mat.m14, mat.m24, mat.m34);
        }

        public static Matrix4x4 Scale(float x, float y, float z)
        {
            return new Matrix4x4(
                x, 0, 0, 0,
                0, y, 0, 0,
                0, 0, z, 0,
                0, 0, 0, 1
            );
        }

        public static Matrix4x4 Scale(Vector3 scale)
        {
            return new Matrix4x4(
                scale.x, 0, 0, 0,
                0, scale.y, 0, 0,
                0, 0, scale.z, 0,
                0, 0, 0, 1
            );
        }

        public static Vector3 GetScale(Matrix4x4 mat)
        {
            return new Vector3(mat.m11, mat.m22, mat.m33);
        }

        public static Matrix4x4 Rotation(float pitch, float yaw, float roll)
        {
            return ZRotation(roll) * XRotation(pitch) * YRotation(yaw);
        }

        public static Matrix3x3 Rotation3x3(float pitch, float yaw, float roll)
        {
            return ZRotation3x3(roll) * XRotation3x3(pitch) * YRotation3x3(yaw);
        }

        public static Matrix4x4 ZRotation(float angle)
        {
            angle = angle * Math.Deg2Rad;
            return new Matrix4x4(
                Mathf.Cos(angle), -Mathf.Sin(angle), 0, 0,
                Mathf.Sin(angle), Mathf.Cos(angle), 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
            );
        }

        public static Matrix3x3 ZRotation3x3(float angle)
        {
            angle = angle * Math.Deg2Rad;
            return new Matrix3x3(
                Mathf.Cos(angle), -Mathf.Sin(angle), 0,
                Mathf.Sin(angle), Mathf.Cos(angle), 0,
                0, 0, 1
            );

        }
        public static Matrix4x4 XRotation(float angle)
        {
            angle = angle * Math.Deg2Rad;
            return new Matrix4x4(
                1, 0, 0, 0,
                0, Mathf.Cos(angle), -Mathf.Sin(angle), 0,
                0, Mathf.Sin(angle), Mathf.Cos(angle), 0,
                0, 0, 0, 1
            );
        }

        public static Matrix3x3 XRotation3x3(float angle)
        {
            angle = angle * Math.Deg2Rad;
            return new Matrix3x3(
                1, 0, 0,
                0, Mathf.Cos(angle), -Mathf.Sin(angle),
                0, Mathf.Sin(angle), Mathf.Cos(angle)
            );

        }

        public static Matrix4x4 YRotation(float angle)
        {
            angle = angle * Math.Deg2Rad;
            return new Matrix4x4(
                 Mathf.Cos(angle), 0, -Mathf.Sin(angle), 0,
                 0, 1, 0, 0,
                 Mathf.Sin(angle), 0, Mathf.Cos(angle), 0,
                 0, 0, 0, 1
            );

        }

        public static Matrix3x3 YRotation3x3(float angle)
        {
            angle = angle * Math.Deg2Rad;
            return new Matrix3x3(
                 Mathf.Cos(angle), 0, -Mathf.Sin(angle),
                0, 1, 0,
                 Mathf.Sin(angle), 0, Mathf.Cos(angle)
            );
        }

        public static Matrix4x4 AxisAngle(Vector3 axis, float angle)
        {
            angle = angle * Math.Deg2Rad;
            float c = Mathf.Cos(angle);
            float s = Mathf.Sin(angle);
            float t = 1 - Mathf.Cos(angle);

            float x = axis.x;
            float y = axis.y;
            float z = axis.z;

            if (axis.MagnitudeSqr != 1)
            {
                float inv_len = 1 / axis.Magnitude;
                x *= inv_len;
                y *= inv_len;
                z *= inv_len;
            }

            return new Matrix4x4(
                t * (x * x) + c, t * x * y - s * z, t * x * z + s * y, 0,
                t * x * y + s * z, t * (y * y) + c, t * y * z - s * x, 0,
                t * x * z - s * y, t * y * z + s * x, t * (z * z) + c, 0,
                0, 0, 0, 1);

        }

        public static Matrix3x3 AxisAngle3x3(Vector3 axis, float angle)
        {
            angle = angle * Math.Deg2Rad;
            float c = Mathf.Cos(angle);
            float s = Mathf.Sin(angle);
            float t = 1 - Mathf.Cos(angle);

            float x = axis.x;
            float y = axis.y;
            float z = axis.z;

            if (axis.MagnitudeSqr != 1)
            {
                float inv_len = 1 / axis.Magnitude;
                x *= inv_len;
                y *= inv_len;
                z *= inv_len;
            }

            return new Matrix3x3(
                t * (x * x) + c, t * x * y - s * z, t * x * z + s * y,
                t * x * y + s * z, t * (y * y) + c, t * y * z - s * x,
                t * x * z - s * y, t * y * z + s * x, t * (z * z) + c
            );
        }

        public static Vector3 MultiplyPoint(Vector3 point, Matrix4x4 mat4)
        {
            Vector3 result;
            result.x = mat4.m11 * point.x + mat4.m12 * point.y + mat4.m13 * point.z + mat4.m14;
            result.y = mat4.m21 * point.x + mat4.m22 * point.y + mat4.m23 * point.z + mat4.m24;
            result.z = mat4.m31 * point.x + mat4.m32 * point.y + mat4.m33 * point.z + mat4.m34;
            return result;
        }

        public static Vector3 MultiplyVector(Vector3 vector, Matrix4x4 mat4)
        {
            Vector3 result;
            result.x = mat4.m11 * vector.x + mat4.m12 * vector.y + mat4.m13 * vector.z;
            result.y = mat4.m21 * vector.x + mat4.m22 * vector.y + mat4.m23 * vector.z;
            result.z = mat4.m31 * vector.x + mat4.m32 * vector.y + mat4.m33 * vector.z;
            return result;

        }

        public static Vector3 MultiplyVector(Vector3 vector, Matrix3x3 mat3)
        {
            Vector3 result;
            result.x = mat3.m11 * vector.x + mat3.m12 * vector.y + mat3.m13 * vector.z;
            result.y = mat3.m21 * vector.x + mat3.m22 * vector.y + mat3.m23 * vector.z;
            result.z = mat3.m31 * vector.x + mat3.m32 * vector.y + mat3.m33 * vector.z;
            return result;
        }


        public static Matrix4x4 Transform(Vector3 scale, Vector3 eulerRotation, Vector3 translate)
        {
            return Translation(translate) * Rotation(eulerRotation.x, eulerRotation.y, eulerRotation.z) * Scale(scale);
        }

        public static Matrix4x4 Transform(Vector3 scale, Vector3 rotationAxis, float rotateAngle, Vector3 translate)
        {
            return Translation(translate) * AxisAngle(rotationAxis, rotateAngle) * Scale(scale);
        }

    }
}