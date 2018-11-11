using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Electronica.Base;
using Electronica.Input.CameraInput;

namespace Electronica.Graphics.Output
{
    /// <summary>
    /// A basic camera class.
    /// </summary>
    public class Camera
    {
        public Matrix ProjectionMatrix { get; private set; }
        public Matrix ViewMatrix { get; private set; }
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public CameraInputMode InputMode { get; set; }

        public Camera()
        {
            InputMode = CameraInputMode.FreeMovement;
        }

        public void Update(GameTime gameTime)
        {
            InputMode.Update(this, gameTime);
            UpdateMatricies();            
        }

        /// <summary>
        /// Generates the view and projection matrices.
        /// </summary>
        private void UpdateMatricies()
        {
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), Main.Instance.graphics.GraphicsDevice.Viewport.AspectRatio, 1f, 1000f);
            ViewMatrix = Matrix.CreateLookAt(Position, Position + Direction, Vector3.UnitY);
        }


        /// <summary>
        /// Rotates the camera with Euler angles.
        /// </summary>
        /// <param name="yaw">X angle</param>
        /// <param name="pitch">Y angle</param>
        /// <param name="roll">Z angle</param>
        public void Rotate(float yaw, float pitch, float roll = 0) => Direction = Vector3.Transform(Direction, Matrix.CreateFromYawPitchRoll(yaw, pitch, roll));

        /// <summary>
        /// Sets the rotation to an Euler angle.
        /// </summary>
        /// <param name="yaw">X angle</param>
        /// <param name="pitch">Y angle</param>
        /// <param name="roll">Z angle</param>
        public void SetRotation(float yaw, float pitch, float roll = 0) => Direction = Vector3.Transform(Vector3.UnitY, Matrix.CreateFromYawPitchRoll(yaw, pitch, roll));

        /// <summary>
        /// Sets the direction to point to the target vector.
        /// </summary>
        /// <param name="target">The target.</param>
        public void LookAt(Vector3 target) => Direction = Vector3.Normalize(target - Position);
    }
}
