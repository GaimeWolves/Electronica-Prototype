using System;
using Electronica.Base;
using Electronica.Circuits;
using Electronica.Graphics.Output;
using Electronica.Input;
using Electronica.Input.CameraInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Electronica.States
{
    public sealed class StateGame : State
    {
        private Camera mCamera;
        private CircuitHandler mCircuitHandler;
        private CircuitInputHandler mInputHandler;

        protected internal override void Initialize(GraphicsDeviceManager graphics)
        {
            Graphics = graphics;

            mCamera = new Camera();
            mCamera.Position = new Vector3(0, 5, -10);
            mCamera.Direction = new Vector3(0, -3.5f, 5);

            mCircuitHandler = new CircuitHandler();
            mInputHandler = new CircuitInputHandler(mCamera, mCircuitHandler);
        }

        public override void Update(GameTime gameTime, float deltaTime)
        {
            InputHandler.UpdateWorldSpace(mCamera, Graphics);
            mInputHandler.HandleInput(gameTime, Graphics);

            mCamera.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mCamera.InputMode is KeyboardMovement)
                mInputHandler.ModulePick.Draw(Graphics, mCamera);

            mCircuitHandler.Draw(Graphics, mCamera);
        }

        private protected override void UnloadContent()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                //Delete unmanaged resources

                base.Dispose(disposing);
            }
        }
    }
}