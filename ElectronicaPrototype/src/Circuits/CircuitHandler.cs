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
        public Circuit CurrentCircuit { get; }

        public CircuitHandler()
        {
            CurrentCircuit = new Circuit();
        }

        public void Draw(GraphicsDeviceManager graphics, Camera camera)
        {
            CurrentCircuit.Draw(graphics, camera);
        }
    }
}