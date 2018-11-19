using Electronica.Circuits.Modules;
using Electronica.Graphics.Output;

using Microsoft.Xna.Framework;

namespace Electronica.Circuits
{
    public class Circuit
    {
        private Board mBoard;
        private Matrix transform;

        public Circuit()
        {
            mBoard = new Board();
            transform = Matrix.Identity;
        }

        public void Draw(GraphicsDeviceManager graphics, Camera camera)
        {
            mBoard.Draw(graphics, camera, transform);
        }

        public void Rotate(float yaw, float pitch, float roll)
            => transform *= Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);
    }
}