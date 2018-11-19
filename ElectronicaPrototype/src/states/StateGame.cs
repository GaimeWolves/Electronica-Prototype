using Electronica.Base;
using Electronica.Circuits;
using Electronica.Graphics.Output;
using Electronica.Input;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Electronica.States
{
    public sealed class StateGame : State
    {
        private Camera mCamera;
        private CircuitHandler mCircuitHandler;

        protected internal override void Initialize(GraphicsDeviceManager graphics)
        {
            Graphics = graphics;

            mCamera = new Camera();
            mCamera.Position = new Vector3(0, 5, -10);
            mCamera.Direction = new Vector3(0, -3.5f, 5);

            LoadContent();
        }

        private protected override void LoadContent()
        {
            mCircuitHandler = new CircuitHandler();
        }

        private void HandleInput(GameTime gameTime, float deltaTime)
        {
            if (InputHandler.IsKeyJustPressed(Keys.Escape))
                Main.Close();

            if (InputHandler.IsKeyJustPressed(Keys.Space))
                if (InputHandler.IsMouseAnchored())
                    InputHandler.ReleaseAnchor();
                else
                    InputHandler.SetAnchor(Graphics.GraphicsDevice.Viewport.Width / 2, Graphics.GraphicsDevice.Viewport.Height / 2);

            mCircuitHandler.HandleInput(mCamera, deltaTime);
        }

        public override void Update(GameTime gameTime, float deltaTime)
        {
            HandleInput(gameTime, deltaTime);

            mCamera.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
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