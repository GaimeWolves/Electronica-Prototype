using Electronica.Graphics.Output;

using Microsoft.Xna.Framework;

namespace Electronica.Input.CameraInput
{
    /// <summary>
    /// Camera movement logic for free movement.
    ///
    /// Left click translates on X-Z plane.
    /// Right click rotates on X and Y axes.
    /// Middle click translates on X-Y plane.
    /// Scrollwheel translates on direction axis.
    ///
    /// Translations happen relative to the directions X-Z plane.
    /// </summary>
    public sealed class FreeMovement : CameraInputMode
    {
        private float mRotationSpeed;
        private Vector2 mRotationVelocity;
        private float mRotationDecelleration;

        private float mTranslationSpeed;
        private Vector3 mTranslationVelocity;
        private float mTranslationDecelleration;

        private float mZoomSpeed;

        public FreeMovement(float rotationSpeed = 0.01f, float translationSpeed = 0.1f, float rotationDecelleration = 10, float translationDecelleration = 5)
        {
            IsTargetedMovementMode = false;

            mTranslationVelocity = new Vector3();
            mRotationVelocity = new Vector2();
            mZoomSpeed = 0;

            mRotationSpeed = rotationSpeed;
            mTranslationSpeed = translationSpeed;

            mTranslationDecelleration = translationDecelleration;
            mRotationDecelleration = rotationDecelleration;
        }

        private void HandleInput(float deltaTime)
        {
            Vector2 delta = InputHandler.DeltaMousePosition;

            if (InputHandler.IsMouseButtonPressed(MouseButton.Left))
                mTranslationVelocity += new Vector3(delta.X, 0f, delta.Y) * mTranslationSpeed * deltaTime;
            else if (InputHandler.IsMouseButtonPressed(MouseButton.Right))
                mRotationVelocity += new Vector2(delta.X, delta.Y) * mRotationSpeed * deltaTime;
            else if (InputHandler.IsMouseButtonPressed(MouseButton.Middle))
                mTranslationVelocity += new Vector3(delta.X, delta.Y, 0) * mTranslationSpeed * deltaTime;

            mZoomSpeed += InputHandler.DeltaScrollWheel * mTranslationSpeed * deltaTime;
        }

        public override void Update(Camera camera, float deltaTime)
        {
            HandleInput(deltaTime);

            camera.Rotate(mRotationVelocity.X, -mRotationVelocity.Y);
            camera.TranslateRelativeToYaw(mTranslationVelocity);
            camera.TranslateOnDirectionAxis(mZoomSpeed);

            mTranslationVelocity *= 1f - deltaTime * mTranslationDecelleration;
            mRotationVelocity *= 1f - deltaTime * mRotationDecelleration;
            mZoomSpeed *= 1f - deltaTime * mTranslationDecelleration;
        }
    }
}