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
    public enum InputState
    {
        View,
        Selected,
        Adding
    }

    public class CircuitInputHandler
    {
        private Camera mCurrentCam;
        private CircuitHandler mCircuithandler;
        private InputState mInputState;

        public Module ModulePick { get; private set; }

        public CircuitInputHandler(Camera camera, CircuitHandler circuitHandler)
        {
            mCurrentCam = camera;
            mCircuithandler = circuitHandler;
            ModulePick = new AND(Vector2.Zero, 0);
            InputHandler.FixedMousePosDrag = true;
            mInputState = InputState.View;
        }

        public void HandleInput(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            if (InputHandler.IsKeyJustPressed(Keys.Escape))
                Main.Close();

            if (InputHandler.IsKeyJustPressed(Keys.F1))
            {
                mInputState = InputState.Selected;
                mCurrentCam.InputMode = new TargetedMovement();
            }

            if (InputHandler.IsKeyJustPressed(Keys.F2))
            {
                mInputState = InputState.View;
                mCurrentCam.InputMode = new FreeMovement();
            }

            if (InputHandler.IsKeyJustPressed(Keys.F3))
            {
                mInputState = InputState.Adding;
                mCurrentCam.InputMode = new KeyboardMovement();
            }

            if (mInputState is InputState.View)
            {
                if (InputHandler.IsMouseButtonPressed(MouseButton.Right))
                {
                    //Vector2 rotation = MathUtils.RotateRelativeToXZAxis(camera.Position, InputHandler.DeltaMousePosition.Y * deltaTime * 0.1f);
                    //mActiveCircuit.Rotate(-InputHandler.DeltaMousePosition.X * deltaTime * 0.1f, rotation.X, rotation.Y);
                }
            }
            else if (mInputState is InputState.Adding)
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
