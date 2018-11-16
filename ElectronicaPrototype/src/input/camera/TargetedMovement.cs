using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Electronica.Base;
using Electronica.Events;
using Electronica.Graphics.Output;
using Electronica.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
        private Vector3 mTarget;
        private float mDistance;
        private float mYaw;
        private float mPitch;

        private float mMinDistance;
        private float mZoomSpeed;
        private float mRotateSpeed;

        private float mZoomDecelleration;
        private float mRotationDecelleration;

        private Vector2 mRotationVelocity;
        private float mZoomVelocity;

        public TargetedMovement(float minDistance = 0.05f, float zoomSpeed = 0.1f, float rotateSpeed = 0.05f, float zoomDecelleration = 5, float rotationDecelleration = 10)
        {
            Main.Instance.MouseInputHandler.MouseMoved += OnMouseMovedEvent;
            Main.Instance.MouseInputHandler.ScrollWheelMoved += OnScrollWheelMoved;

            mMinDistance = minDistance;
            mZoomSpeed = zoomSpeed;
            mRotateSpeed = rotateSpeed;
            mRotationDecelleration = rotationDecelleration;
            mZoomDecelleration = zoomDecelleration;

            mRotationVelocity = Vector2.Zero;
            mZoomVelocity = 0;

            mDistance = 5;
            mYaw = 0;
            mPitch = MathHelper.PiOver4;
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            mYaw -= mRotationVelocity.X;
            mPitch += mRotationVelocity.Y;
            mDistance -= mZoomVelocity;

            mPitch = MathUtils.WrapAngle(mPitch);
            mDistance = Math.Max(mDistance, mMinDistance);

            Console.WriteLine(camera.Position);

            camera.Position = (new Vector3((float)(Math.Cos(mPitch) * Math.Sin(mYaw)), (float)Math.Sin(mPitch), (float)(Math.Cos(mPitch) * Math.Cos(mYaw)))) * mDistance + mTarget;
            camera.LookAt(mTarget);

            mRotationVelocity *= 1f - (float)gameTime.ElapsedGameTime.TotalSeconds * mRotationDecelleration;
            mZoomVelocity *= 1f - (float)gameTime.ElapsedGameTime.TotalSeconds * mZoomDecelleration;
        }

        private void OnMouseMovedEvent(object e, MouseMovedEventArgs args)
        {
            if (args.LeftButtonPressed)
                mRotationVelocity += new Vector2(args.Speed.X, args.Speed.Y) * mRotateSpeed;
        }

        private void OnScrollWheelMoved(object e, ScrollWheelMovedEventArgs args)
        {
            mZoomVelocity += args.Speed * mZoomSpeed;
        }

        /// <summary>
        /// Sets the target of the camera.
        /// </summary>
        /// <param name="target">The camera.</param>
        public void SetTarget(Vector3 target) 
            => mTarget = target;
    }
}
