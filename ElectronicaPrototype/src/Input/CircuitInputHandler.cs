using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Electronica.Base;
using Electronica.Circuits;
using Electronica.Circuits.Modules;
using Electronica.Graphics.Output;
using Electronica.Input.CameraInput;
using Electronica.Raycast;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Electronica.Input
{
    public class CircuitInputHandler
    {
        private Camera mCurrentCam;
        private CircuitHandler mCircuithandler;
        private Vector2 dragPosition;

        public Module ModulePick { get; private set; }

        public CircuitInputHandler(Camera camera, CircuitHandler circuitHandler)
        {
            mCurrentCam = camera;
            mCircuithandler = circuitHandler;
            ModulePick = new AND(Vector2.Zero, 0);
            dragPosition = Vector2.Zero;
            InputHandler.FixedMousePosDrag = true;
        }

        public void HandleInput(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            if (InputHandler.IsKeyJustPressed(Keys.Escape))
                Main.Close();

            if (InputHandler.IsKeyJustPressed(Keys.Space))
                if (InputHandler.IsMouseAnchored())
                    InputHandler.ReleaseAnchor();
                else
                    InputHandler.SetAnchor(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);

            if (InputHandler.IsKeyJustPressed(Keys.F1))
                mCurrentCam.InputMode = new TargetedMovement();

            if (InputHandler.IsKeyJustPressed(Keys.F2))
                mCurrentCam.InputMode = new FreeMovement();

            if (InputHandler.IsKeyJustPressed(Keys.F3))
                mCurrentCam.InputMode = new KeyboardMovement();

            if (mCurrentCam.InputMode is FreeMovement)
            {
                if (InputHandler.IsMouseButtonPressed(MouseButton.Right))
                {
                    //Vector2 rotation = MathUtils.RotateRelativeToXZAxis(camera.Position, InputHandler.DeltaMousePosition.Y * deltaTime * 0.1f);
                    //mActiveCircuit.Rotate(-InputHandler.DeltaMousePosition.X * deltaTime * 0.1f, rotation.X, rotation.Y);
                }
            }
            else if (mCurrentCam.InputMode is KeyboardMovement)
            {
                if (InputHandler.IsMouseButtonPressed(MouseButton.Right))
                    ModulePick.Rotate(InputHandler.DeltaMousePosition.X * 0.01f);

                RayCast ray = new RayCast(InputHandler.WorldSpacePosition, InputHandler.WorldSpaceDirection);
                ray.CastBoard(500, 10000, mCircuithandler.CurrentCircuit.Board);

                if (ray.Hit.HasValue && !InputHandler.IsMouseButtonPressed(MouseButton.Right))
                    ModulePick.SetPosition(new Vector2(ray.Hit.Value.X, ray.Hit.Value.Z));

                if (ray.Hit.HasValue && InputHandler.IsMouseButtonJustPressed(MouseButton.Left))
                {
                    mCircuithandler.CurrentCircuit.AddModule(ModulePick);
                    ModulePick = new AND();
                    ModulePick.SetPosition(new Vector2(ray.Hit.Value.X, ray.Hit.Value.Z));
                }
            }
        }
    }
}
