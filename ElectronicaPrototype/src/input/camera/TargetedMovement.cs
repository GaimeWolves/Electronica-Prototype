using System;

using Electronica.Graphics.Output;
using Electronica.Utils;

using Microsoft.Xna.Framework;

namespace Electronica.Input.CameraInput
{
    /// <summary>
    /// Camera movement logic for targeted movement.
    ///
    /// Left click rotates around the target.
    /// Scrollwheel zooms in- and outwards.
    /// </summary>
    public sealed class TargetedMovement : CameraInputMode
    {
        private Vector3? mTarget;

        private float mMinDistance;
        private float mZoomSpeed;
        private float mRotateSpeed;

        private float mZoomDecelleration;
        private float mRotationDecelleration;

        private Vector2 mRotationVelocity;
        private float mZoomVelocity;

        public TargetedMovement(float minDistance = 0.02f, float zoomSpeed = 0.1f, float rotateSpeed = 0.05f, float zoomDecelleration = 5, float rotationDecelleration = 10)
        {
            IsTargetedMovementMode = true;
            mTarget = Vector3.Zero;

            mMinDistance = minDistance;
            mZoomSpeed = zoomSpeed;
            mRotateSpeed = rotateSpeed;
            mRotationDecelleration = rotationDecelleration;
            mZoomDecelleration = zoomDecelleration;

            mRotationVelocity = Vector2.Zero;
            mZoomVelocity = 0;
        }

        public override void Update(Camera camera, float deltaTime)
        {
            if (InputHandler.IsMouseButtonPressed(MouseButton.Left))
                mRotationVelocity += new Vector2(InputHandler.DeltaMousePosition.X, InputHandler.DeltaMousePosition.Y) * mRotateSpeed * deltaTime;

            mZoomVelocity += InputHandler.DeltaScrollWheel * mZoomSpeed * deltaTime;

            camera.Position = Vector3.Transform(camera.Position, Matrix.CreateFromAxisAngle(Vector3.UnitY, mRotationVelocity.X));
            camera.Position = MathUtils.RotatePitch(camera.Position, mRotationVelocity.Y);

            camera.LookAt(mTarget.Value);
            camera.TranslateOnDirectionAxis(mZoomVelocity);

            if (Vector3.Distance(camera.Position, mTarget.Value) < mMinDistance)
            {
                Vector3 delta = camera.Position - mTarget.Value;
                delta.Normalize();
                camera.Position = mTarget.Value + delta * mMinDistance;
                mZoomVelocity = 0;
            }
            

            mRotationVelocity *= 1f - deltaTime * mRotationDecelleration;
            mZoomVelocity *= 1f - deltaTime * mZoomDecelleration;
        }

        /// <summary>
        /// Sets the target of the camera.
        /// </summary>
        /// <param name="target">The camera.</param>
        public override void SetTarget(Vector3? target)
            => mTarget = target;
    }
}