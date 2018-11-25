using System;

using Electronica.Graphics.Output;
using Electronica.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Electronica.Input.CameraInput
{
    /// <summary>
    /// Camera logic for keyboard movement.
    /// 
    /// WASD to move on X-Z plane.
    /// </summary>
    public sealed class KeyboardMovement : CameraInputMode
    {
        private float mTranslateSpeed;
        private float mTranslationDecelleration;

        private Vector2 mTranslateVelocity;

        public KeyboardMovement(float translateSpeed = 0.05f, float translationDecelleration = 10)
        {
            IsTargetedMovementMode = false;

            mTranslateSpeed = translateSpeed;
            mTranslationDecelleration = translationDecelleration;

            mTranslateVelocity = Vector2.Zero;
        }

        public override void Update(Camera camera, float deltaTime)
        {
            if (InputHandler.IsKeyPressed(Keys.W))
                mTranslateVelocity.Y += mTranslateSpeed * deltaTime;

            if (InputHandler.IsKeyPressed(Keys.A))
                mTranslateVelocity.X += mTranslateSpeed * deltaTime;

            if (InputHandler.IsKeyPressed(Keys.S))
                mTranslateVelocity.Y -= mTranslateSpeed * deltaTime;

            if (InputHandler.IsKeyPressed(Keys.D))
                mTranslateVelocity.X -= mTranslateSpeed * deltaTime;

            camera.TranslateRelativeToYaw(new Vector3(mTranslateVelocity.X, 0, mTranslateVelocity.Y));
            
            mTranslateVelocity *= 1f - deltaTime * mTranslationDecelleration;
        }
    }
}