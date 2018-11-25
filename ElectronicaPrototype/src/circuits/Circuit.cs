using System.Collections.Generic;
using Electronica.Circuits.Modules;
using Electronica.Graphics.Output;

using Microsoft.Xna.Framework;

namespace Electronica.Circuits
{
    public class Circuit
    {
        public Board Board { get; }

        private List<Module> mModules;

        private Matrix transform;

        public Circuit()
        {
            Board = new Board();
            transform = Matrix.Identity;
            mModules = new List<Module>();
        }

        public void Draw(GraphicsDeviceManager graphics, Camera camera)
        {
            Board.Draw(graphics, camera, transform);

            mModules.ForEach(m => m.Draw(graphics, camera, transform));
        }

        public void Rotate(float yaw, float pitch, float roll)
            => transform *= Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);

        /// <summary>
        /// Adds a copy of the model to the model list.
        /// </summary>
        /// <param name="module">The module to add.</param>
        public void AddModule(Module module)
            => mModules.Add((Module)module.Clone());
    }
}