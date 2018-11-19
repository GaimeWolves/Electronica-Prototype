using System;

using Microsoft.Xna.Framework;

namespace Electronica.Utils
{
    public static class MathUtils
    {
        /// <summary>
        /// Converts a vector to euler angles.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns>A vector containing the euler angles.</returns>
        public static Vector2 ConvertToEulerAngles(Vector3 vector)
        {
            vector.Normalize();
            float yaw = (float)Math.Atan2(vector.X, vector.Z);
            float pitch = (float)Math.Asin(-vector.Y);

            return new Vector2(pitch, yaw);
        }

        /// <summary>
        /// Wraps the angle between -π/2 and +π/2 (± 0.001 for rounding errors so that cameras don't flip or anything).
        /// </summary>
        /// <param name="angle">The angle to wrap.</param>
        /// <returns>The wraped angle.</returns>
        public static float WrapAngle(float angle)
            => Clamp(angle, -MathHelper.PiOver2 + 0.001f, MathHelper.PiOver2 - 0.001f);

        /// <summary>
        /// Clamps a value between the specified values.
        /// </summary>
        /// <param name="x">The value to clamp.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>The clamped value.</returns>
        public static float Clamp(float x, float min, float max)
            => Math.Min(Math.Max(x, min), max);

        /// <summary>
        /// Creates the direction und up vector for a camera.
        /// </summary>
        /// <param name="pitch">The pitch of the camera.</param>
        /// <param name="yaw">The yaw of the camera.</param>
        /// <param name="direction">The calculated direction.</param>
        /// <param name="up">The calculated up vector.</param>
        public static void CreateCamera(float pitch, float yaw, out Vector3 direction, out Vector3 up)
        {
            Matrix rotationMatrix = Matrix.CreateRotationX(pitch) * Matrix.CreateRotationY(yaw);
            direction = Vector3.Transform(Vector3.UnitZ, rotationMatrix);
            up = Vector3.Transform(Vector3.UnitY, rotationMatrix);
        }

        public static bool InRange(float x, float min, float max) => x >= min && x <= max;

        /// <summary>
        /// Normalizes a vector with fixed values.
        /// </summary>
        /// <param name="v">The vector to normalize.</param>
        /// <param name="xFixed">Is x fixed.</param>
        /// <param name="yFixed">Is y fixed.</param>
        /// <param name="zFixed">Is z fixed.</param>
        /// <returns>The normalized vector.</returns>
        public static Vector3 FixedNormalize(Vector3 v, bool xFixed, bool yFixed, bool zFixed)
        {
            if (!xFixed && !yFixed && !zFixed || v.X == 1 || v.X == -1 || v.Y == 1 || v.Y == -1 || v.Z == 1 || v.Z == -1)
                return Vector3.Normalize(v);

            Vector3 vector = v;
            float xSquared = vector.X * vector.X;
            float ySquared = vector.Y * vector.Y;
            float zSquared = vector.Z * vector.Z;

            if (!InRange(vector.X, -1, 1))
                throw new Exception("Absolute fixed value is larger than 1.");

            if (!InRange(vector.Y, -1, 1))
                throw new Exception("Absolute fixed value is larger than 1.");

            if (!InRange(vector.Z, -1, 1))
                throw new Exception("Absolute fixed value is larger than 1.");

            if (xFixed && yFixed && zFixed)
                throw new Exception("Cannot normalize. All values fixed.");

            if (xFixed && yFixed || xFixed && zFixed || yFixed && zFixed)
            {
                if (!xFixed && new Vector2(vector.Y, vector.Z).Length() <= 1)
                    vector.X *= (1 - ySquared - zSquared) / xSquared;
                else if (!yFixed && new Vector2(vector.X, vector.Z).Length() <= 1)
                    vector.Y *= (1 - xSquared - zSquared) / ySquared;
                else if (!zFixed && new Vector2(vector.Y, vector.X).Length() <= 1)
                    vector.Z *= (1 - ySquared - xSquared) / zSquared;
                else
                    throw new Exception("Cannot normalize. Lenght of vector always larger than 1.");
            }
            else
            {
                if (xFixed)
                {
                    vector.Y *= (1 - xSquared) / (ySquared + zSquared);
                    vector.Z *= (1 - xSquared) / (ySquared + zSquared);
                }
                else if (yFixed)
                {
                    vector.X *= (1 - ySquared) / (xSquared + zSquared);
                    vector.Z *= (1 - ySquared) / (xSquared + zSquared);
                }
                else
                {
                    vector.Y *= (1 - zSquared) / (ySquared + xSquared);
                    vector.X *= (1 - zSquared) / (ySquared + xSquared);
                }
            }

            return Vector3.Normalize(vector);
        }

        /// <summary>
        /// Rotates a vector up and down by setting the y value rather than using Euler angles avoiding gimbal lock. 
        /// </summary>
        /// <param name="v">The vector to rotate.</param>
        /// <param name="delta">The rotation amount.</param>
        /// <returns>The rotated vector.</returns>
        public static Vector3 RotatePitch(Vector3 v, float delta)
        {
            Vector3 vector = v;
            vector.Normalize();
            vector.Y = Clamp(vector.Y + delta, -0.999f, 0.999f);
            vector = FixedNormalize(vector, false, true, false);
            return vector * v.Length();
        }

        /// <summary>
        /// Calculates the roll and pitch relative to a X-Z Plane Axis (like camera yaw angle) from a given rotation.
        /// </summary>
        /// <param name="axisPosition">The Axis to rotate from.</param>
        /// <param name="rotation">The rotation.</param>
        /// <returns>The pitch and roll values.</returns>
        public static Vector2 RotateRelativeToXZAxis(Vector3 axisPosition, float rotation)
        {
            axisPosition.Normalize();
            float pitch = (float)Math.Cos(axisPosition.Z) * rotation;
            float roll = (float)Math.Sin(axisPosition.X) * rotation;
            return new Vector2(pitch, roll);
        }
    }
}