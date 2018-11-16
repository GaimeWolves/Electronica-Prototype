using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
