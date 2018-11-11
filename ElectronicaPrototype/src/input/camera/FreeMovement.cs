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
        private float mTranslationSpeed;
        private Vector3 mTranslationVelocity;
        private Vector2 mRotationVelocity;
        private float mZoomSpeed;

        public FreeMovement(float rotationSpeed = 0.01f, float translationSpeed = 0.1f)
        {
            currentTranslation = new CameraTranslation();
            Main.Instance.mouseInputHandler.MouseMoved += OnMouseMovedEvent;
            Main.Instance.mouseInputHandler.ScrollWheelMoved += OnScrollWheelMoved;

            mTranslationVelocity = new Vector3();
            mRotationVelocity = new Vector2();
            mZoomSpeed = 0;

            mRotationSpeed = rotationSpeed;
            mTranslationSpeed = translationSpeed;
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            currentTranslation.translation = Matrix.CreateTranslation(mTranslationVelocity.X, mTranslationVelocity.Y, mTranslationVelocity.Z);
            currentTranslation.rotation = Matrix.CreateFromYawPitchRoll(mRotationVelocity.X, -mRotationVelocity.Y, 0f);

            camera.Direction = Vector3.Transform(camera.Direction, currentTranslation.rotation);

            float angle = (float)Math.Atan2(camera.Direction.X, camera.Direction.Z);

            camera.Position += Vector3.Transform(Vector3.Transform(camera.Position, currentTranslation.translation) - camera.Position, Matrix.CreateFromAxisAngle(Vector3.UnitY, angle)) + camera.Direction * mZoomSpeed;

            currentTranslation.rotation = Matrix.CreateRotationY(0f);
            currentTranslation.translation = Matrix.CreateTranslation(0f, 0f, 0f);

            mTranslationVelocity *= 1f - (float)gameTime.ElapsedGameTime.TotalSeconds * 5;
            mRotationVelocity *= 1f - (float)gameTime.ElapsedGameTime.TotalSeconds * 5;
            mZoomSpeed *= 1f - (float)gameTime.ElapsedGameTime.TotalSeconds * 5;            
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
