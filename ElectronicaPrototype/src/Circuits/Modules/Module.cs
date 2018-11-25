using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Electronica.Graphics.Output;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XMLSchemes;

namespace Electronica.Circuits.Modules
{
    public abstract class Module : ICloneable
    {
        protected ModuleSettings mModuleSettings;
        private protected Model mModel;
        private protected Matrix mOffsetTranslation;

        private protected Matrix mPositionTranslation;
        protected Vector2 mPosition;
        protected float mRotation;

        public Module()
        {
            mPosition = Vector2.Zero;
            mRotation = 0;
            UpdateMatrix();
        }

        public virtual void Draw(GraphicsDeviceManager graphics, Camera camera, Matrix parentTransform)
        {
            foreach (ModelMesh mesh in mModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = mPositionTranslation * parentTransform * mOffsetTranslation;
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                    effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                }

                mesh.Draw();
            }
        }

        /// <summary>
        /// Ignores the parent transform.
        /// </summary>
        /// <param name="graphics">The graphics device.</param>
        /// <param name="camera">The camera.</param>
        public virtual void Draw(GraphicsDeviceManager graphics, Camera camera)
            => Draw(graphics, camera, Matrix.Identity);

        /// <summary>
        /// Updates the position matrix.
        /// </summary>
        private protected void UpdateMatrix()
            => mPositionTranslation = Matrix.CreateRotationY(mRotation) * Matrix.CreateTranslation(new Vector3(mPosition.X, 0, mPosition.Y));

        public void SetPosition(Vector2 position)
        {
            mPosition = position;
            UpdateMatrix();
        }

        public void Translate(Vector2 translation)
        {
            mPosition += translation;
            UpdateMatrix();
        }

        public void Rotate(float rotation)
        {
            mRotation += rotation;
            UpdateMatrix();
        }

        /// <summary>
        /// Clones the module memberwise.
        /// </summary>
        /// <returns>The cloned module as a object.</returns>
        public object Clone()
            => MemberwiseClone();
    }
}
