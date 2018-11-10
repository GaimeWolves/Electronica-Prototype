using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Electronica.Base;
using Electronica.Graphics.Output;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Electronica.States
{
    public sealed class StateGame : State
    {
        Camera camera;

        Model model;
        Texture2D diffuse;
        Texture2D normal;

        Matrix world;
        float rotation;

        protected internal override void Initialize()
        {
            camera = new Camera();
            camera.Position = new Vector3(0, 0, -10);
            camera.Direction = new Vector3(0, 0, 1);

            world = Matrix.CreateRotationY(rotation);

            LoadContent();
        }

        private protected override void LoadContent()
        {
            model = Game1.Instance.Content.Load<Model>("moduleBattery/battery");

            diffuse = Game1.Instance.Content.Load<Texture2D>("moduleBattery/diffuse");
            normal = Game1.Instance.Content.Load<Texture2D>("moduleBattery/normals");
        }

        public override void Update(GameTime gameTime)
        {
            rotation += (float) gameTime.ElapsedGameTime.TotalSeconds;

            camera.Position = Vector3.Transform(camera.Position, Matrix.CreateRotationY(MathHelper.ToRadians(1f)));
            camera.LookAt(Vector3.Zero);

            camera.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            world = Matrix.CreateTranslation(-1, 0, 0);
            foreach(ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.Projection = camera.ProjectionMatrix;
                    effect.View = camera.ViewMatrix;
                    effect.World = world;

                    effect.TextureEnabled = true;
                    effect.Texture = diffuse;
                }
                mesh.Draw();
            }

            world = Matrix.CreateTranslation(1, 0, 0);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.Projection = camera.ProjectionMatrix;
                    effect.View = camera.ViewMatrix;
                    effect.World = world;

                    effect.TextureEnabled = true;
                    effect.Texture = diffuse;
                }
                mesh.Draw();
            }
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
