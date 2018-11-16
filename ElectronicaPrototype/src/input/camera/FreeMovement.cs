using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Electronica.Base;
using Electronica.Events;
using Electronica.Graphics.Output;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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

        public FreeMovement(
            float rotationSpeed = 0.01f, 
            float translationSpeed = 0.1f,
            float rotationDecelleration = 10, 
            float translationDecelleration = 5
            )
        {
            Main.Instance.MouseInputHandler.MouseMoved += OnMouseMovedEvent;
            Main.Instance.MouseInputHandler.ScrollWheelMoved += OnScrollWheelMoved;

            mTranslationVelocity = new Vector3();
            mRotationVelocity = new Vector2();
            mZoomSpeed = 0;

            mRotationSpeed = rotationSpeed;
            mTranslationSpeed = translationSpeed;

            mTranslationDecelleration = translationDecelleration;
            mRotationDecelleration = rotationDecelleration;
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            camera.Rotate(mRotationVelocity.X, -mRotationVelocity.Y);
            camera.TranslateRelativeToYaw(mTranslationVelocity);
            camera.TranslateOnDirectionAxis(mZoomSpeed);

            mTranslationVelocity *= 1f - (float)gameTime.ElapsedGameTime.TotalSeconds * mTranslationDecelleration;
            mRotationVelocity *= 1f - (float)gameTime.ElapsedGameTime.TotalSeconds * mRotationDecelleration;
            mZoomSpeed *= 1f - (float)gameTime.ElapsedGameTime.TotalSeconds * mTranslationDecelleration;            
        }

        private void OnMouseMovedEvent(object e, MouseMovedEventArgs args)
        {
            if (args.LeftButtonPressed)
                mTranslationVelocity += new Vector3(args.Speed.X, 0f, args.Speed.Y) * mTranslationSpeed;
            else if (args.RightButtonPressed)
                mRotationVelocity += new Vector2(args.Speed.X, args.Speed.Y) * mRotationSpeed;
            else if (args.MiddleButtonPressed)
                mTranslationVelocity += new Vector3(args.Speed.X, args.Speed.Y, 0) * mTranslationSpeed;
        }

        private void OnScrollWheelMoved(object e, ScrollWheelMovedEventArgs args)
        {
            mZoomSpeed += args.Speed * mTranslationSpeed;
        }
    }
}
