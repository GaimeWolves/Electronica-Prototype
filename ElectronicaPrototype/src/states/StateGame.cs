using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Electronica.Base;
using Electronica.Circuits.Modules;
using Electronica.Graphics.Output;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Electronica.States
{
    public sealed class StateGame : State
    {
        Camera camera;

        Model model;
        Texture2D diffuse;
        Texture2D normal;

        Matrix world;

        Board board;

        protected internal override void Initialize()
        {
            camera = new Camera();
            camera.Position = new Vector3(0, 0, -10);

            LoadContent();
        }

        private protected override void LoadContent()
        {
            model = Main.Instance.Content.Load<Model>("moduleBattery/battery");

            diffuse = Main.Instance.Content.Load<Texture2D>("moduleBattery/diffuse");
            normal = Main.Instance.Content.Load<Texture2D>("moduleBattery/normals");

            board = new Board();
        }

        public override void Update(GameTime gameTime)
        {
            camera.Update(gameTime);

            if (Main.Instance.KeyboardInputHandler.IsKeyJustPressed(Keys.Space))
                Main.Instance.MouseInputHandler.SetAnchor(new Vector2(Main.Instance.GraphicsDevice.Viewport.Width / 2, Main.Instance.GraphicsDevice.Viewport.Height / 2));
            else if (Main.Instance.KeyboardInputHandler.IsKeyJustReleased(Keys.Space))
                Main.Instance.MouseInputHandler.ReleaseAnchor();

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            world = Matrix.CreateTranslation(-1, 5, 0);
            foreach(ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                    effect.Projection = camera.ProjectionMatrix;
                    effect.View = camera.ViewMatrix;
                    effect.World = world;

                    effect.TextureEnabled = true;
                    effect.Texture = diffuse;
                }
                mesh.Draw();
            }

            world = Matrix.CreateTranslation(1, 5, 0);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                    effect.Projection = camera.ProjectionMatrix;
                    effect.View = camera.ViewMatrix;
                    effect.World = world;

                    effect.TextureEnabled = true;
                    effect.Texture = diffuse;
                }
                mesh.Draw();
            }

            board.Draw(Main.Instance.graphics, camera.ProjectionMatrix, camera.ViewMatrix);

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
