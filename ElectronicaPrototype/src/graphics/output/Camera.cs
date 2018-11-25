using Electronica.Input.CameraInput;
using Electronica.States;
using Electronica.Utils;

using Microsoft.Xna.Framework;

namespace Electronica.Graphics.Output
{
    /// <summary>
    /// A basic camera class.
    /// </summary>
    public class Camera
    {
        public Matrix ProjectionMatrix { get; private set; }
        public Matrix ViewMatrix { get; private set; }
        public Vector3 Position { get; set; }
        public CameraInputMode InputMode { get; set; }

        public Vector3 Direction
        {
            get => mDirection;
            set => SetRotation(MathUtils.ConvertToEulerAngles(value));
        }

        private Vector3 mDirection;
        private Vector3 mCameraUp;

        public Camera()
        {
            InputMode = new FreeMovement();
            Direction = Vector3.UnitZ;

            SetProjection(MathHelper.PiOver4, 0.01f, 1000f);
        }

        public void Update(float deltaTime)
        {
            InputMode.Update(this, deltaTime);
            UpdateViewMatrix();
        }

        /// <summary>
        /// Updates the view matrix.
        /// </summary>
        private void UpdateViewMatrix()
            => ViewMatrix = Matrix.CreateLookAt(Position, Position + mDirection, mCameraUp);

        /// <summary>
        /// Sets the direction and up vector to the rotation.
        /// </summary>
        /// <param name="pitch">The pitch of the rotation.</param>
        /// <param name="yaw">The yaw of the rotation.</param>
        private void SetRotation(float pitch, float yaw)
            => MathUtils.CreateCamera(pitch, yaw, out mDirection, out mCameraUp);

        /// <summary>
        /// Sets the direction and up vector to the rotation.
        /// </summary>
        /// <param name="rotation">The rotation vector.</param>
        private void SetRotation(Vector2 rotation)
            => MathUtils.CreateCamera(rotation.X, rotation.Y, out mDirection, out mCameraUp);

        /// <summary>
        /// Rotates the camera with Euler angles.
        /// </summary>
        /// <param name="dYaw">The change in yaw.</param>
        /// <param name="dPitch">The change in pitch</param>
        public void Rotate(float dYaw, float dPitch)
            => SetRotation(MathUtils.ConvertToEulerAngles(mDirection) + new Vector2(dPitch, dYaw));

        /// <summary>
        /// Translates relative to the yaw angle (on the x-z plane).
        /// </summary>
        /// <param name="translation">The translation vector.</param>
        public void TranslateRelativeToYaw(Vector3 translation)
            => Position += Vector3.Transform(translation, Matrix.CreateFromAxisAngle(Vector3.UnitY, MathUtils.ConvertToEulerAngles(mDirection).Y));

        /// <summary>
        /// Translates the position by the direction vector.
        /// </summary>
        /// <param name="translation">The amount of translation.</param>
        public void TranslateOnDirectionAxis(float translation)
            => Position += mDirection * translation;

        /// <summary>
        /// Updates the projection matrix with the new values.
        /// </summary>
        /// <param name="fov">The new field ov view.</param>
        /// <param name="nearPlane">The new near clip plane.</param>
        /// <param name="farPlane">The new far clip plane.</param>
        public void SetProjection(float fov, float nearPlane, float farPlane)
            => ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(fov, StateManager.CurrentState.Graphics.GraphicsDevice.Viewport.AspectRatio, nearPlane, farPlane);

        /// <summary>
        /// Rotates the camera towards the target.
        /// </summary>
        /// <param name="target">The target.</param>
        public void LookAt(Vector3 target)
            => (Direction = target - Position).Normalize();
    }
}