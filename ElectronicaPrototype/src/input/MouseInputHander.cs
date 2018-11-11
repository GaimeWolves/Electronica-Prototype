using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Electronica.Base;
using Electronica.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Electronica.Input
{
    /// <summary>
    /// Provides event handlers for mouse movement.
    /// </summary>
    public class MouseInputHander
    {
        private MouseState mCurrentState, mLastState;
        private Vector2 mAnchor;
        private bool mAnchored;

        public MouseInputHander()
        {
            mLastState = Mouse.GetState();
            mCurrentState = Mouse.GetState();
            mAnchor = new Vector2();
            mAnchored = false;
        }

        /// <summary>
        /// Calculates useful values like the cursors velocity and calls the respective events.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            mCurrentState = Mouse.GetState();

            if (mCurrentState.Position != mLastState.Position || (mAnchored && mCurrentState.Position.ToVector2() != mAnchor))
            {
                MouseMovedEventArgs args = new MouseMovedEventArgs();

                args.RightButtonPressed = mCurrentState.RightButton == ButtonState.Pressed;
                args.LeftButtonPressed = mCurrentState.LeftButton == ButtonState.Pressed;
                args.MiddleButtonPressed = mCurrentState.MiddleButton == ButtonState.Pressed;
                args.Position = mCurrentState.Position.ToVector2();
                
                if (mAnchored)
                    args.DeltaPosition = mAnchor - mCurrentState.Position.ToVector2();
                else
                    args.DeltaPosition = (mLastState.Position - mCurrentState.Position).ToVector2();

                args.Speed = Vector2.Multiply(args.DeltaPosition, (float)gameTime.ElapsedGameTime.TotalSeconds);

                MouseMoved(this, args);
            }

            if (mCurrentState.ScrollWheelValue != mLastState.ScrollWheelValue)
            {
                ScrollWheelMovedEventArgs args = new ScrollWheelMovedEventArgs();

                args.Position = mCurrentState.ScrollWheelValue;
                args.Delta = args.Position - mLastState.ScrollWheelValue;
                args.Speed = args.Delta * (float)gameTime.ElapsedGameTime.TotalSeconds;

                ScrollWheelMoved(this, args);
            }

            if (mAnchored)
                Mouse.SetPosition((int)mAnchor.X, (int)mAnchor.Y);

            mLastState = mCurrentState;
        }

        /// <summary>
        /// Anchores cursor at the specified vector.
        /// </summary>
        /// <param name="anchor">The position of the anchor.</param>
        public void SetAnchor(Vector2 anchor)
        {
            mAnchored = true;
            mAnchor = anchor;
        }

        /// <summary>
        /// Releases the cursor.
        /// </summary>
        public void ReleaseAnchor() => mAnchored = false;

        public event EventHandler<MouseMovedEventArgs> MouseMoved;
        public event EventHandler<ScrollWheelMovedEventArgs> ScrollWheelMoved;
    }
}
