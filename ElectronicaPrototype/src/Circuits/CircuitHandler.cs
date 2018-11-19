using System;
using Electronica.Graphics.Output;
using Electronica.Input;
using Electronica.Input.CameraInput;
using Electronica.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Electronica.Circuits
{
    public class CircuitHandler
    {
        private Circuit mActiveCircuit;

        public CircuitHandler()
        {
            mActiveCircuit = new Circuit();
        }

        public void HandleInput(Camera camera, float deltaTime)
        {
            if (InputHandler.IsKeyJustPressed(Keys.F1))
                camera.InputMode = CameraInputMode.TargetedMovement;

            if (InputHandler.IsKeyJustPressed(Keys.F2))
                camera.InputMode = CameraInputMode.FreeMovement;

            if (camera.InputMode == CameraInputMode.TargetedMovement)
            {
                if (InputHandler.IsMouseButtonPressed(MouseButton.Right))
                {
                    Vector2 rotation = MathUtils.RotateRelativeToXZAxis(camera.Position, InputHandler.DeltaMousePosition.Y * deltaTime * 0.1f);
                    mActiveCircuit.Rotate(-InputHandler.DeltaMousePosition.X * deltaTime * 0.1f, rotation.X, rotation.Y);
                }
            }
        }

        public void Draw(GraphicsDeviceManager graphics, Camera camera)
        {
            mActiveCircuit.Draw(graphics, camera);
        }
    }
}