using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Electronica.Base;

namespace Electronica.Graphics.Output
{
    class Camera
    {
        public Matrix ProjectionMatrix { get; private set; }
        public Matrix ViewMatrix { get; private set; }
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }

        public void Update(GameTime gameTime)
        {
            UpdateMatricies();
        }

        private void UpdateMatricies()
        {
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), Game1.Instance.graphics.GraphicsDevice.Viewport.AspectRatio, 1f, 1000f);
            ViewMatrix = Matrix.CreateLookAt(Position, Position + Direction, Vector3.UnitY);
        }

        public void Rotate(float yaw, float pitch, float roll = 0) => Direction = Vector3.Transform(Direction, Matrix.CreateFromYawPitchRoll(yaw, pitch, roll));

        public void SetRotation(float yaw, float pitch, float roll = 0) => Direction = Vector3.Transform(Vector3.UnitY, Matrix.CreateFromYawPitchRoll(yaw, pitch, roll));

        public void LookAt(Vector3 target) => Direction = Vector3.Normalize(target - Position);
    }
}
