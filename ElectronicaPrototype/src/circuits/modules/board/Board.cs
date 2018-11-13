using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Electronica.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Electronica.Circuits.Modules
{
    public class Board
    {
        private static readonly float StandardBoardSize = 5;
        private static readonly float StandardThickness = 0.1f;

        /// <summary>
        /// !Ordered! list of vertices
        /// </summary>
        private List<Vector2> vertices;

        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private int vertexCount, indexCount;

        private BasicEffect basicEffect;

        private Matrix translation;

        public Board()
        {
            vertices = new List<Vector2>();
            vertices.Add(new Vector2(-StandardBoardSize, -StandardBoardSize));
            vertices.Add(new Vector2(-StandardBoardSize,  StandardBoardSize));
            vertices.Add(new Vector2( StandardBoardSize,  StandardBoardSize));
            vertices.Add(new Vector2( StandardBoardSize,  -StandardBoardSize));

            UpdateBuffers();

            basicEffect = new BasicEffect(Main.Instance.GraphicsDevice);
            translation = Matrix.Identity;
        }

        /// <summary>
        /// Draws the board on the screen.
        /// </summary>
        /// <param name="graphics">The current GraphicsDeviceManager.</param>
        /// <param name="projection">The projection matrix of the camera.</param>
        /// <param name="view">The view matrix of the camera.</param>
        public void Draw(GraphicsDeviceManager graphics, Matrix projection, Matrix view)
        {
            basicEffect.EnableDefaultLighting();
            basicEffect.World = translation;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
            basicEffect.VertexColorEnabled = true;

            graphics.GraphicsDevice.Indices = indexBuffer;
            graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indexCount / 3);
            }
        }

        /// <summary>
        /// Generates a new mesh from the vertices and updates the vertex and index buffer.
        /// </summary>
        private void UpdateBuffers()
        {
            Mesh mesh = BoardCreator.CreateBoard(vertices, StandardThickness);
            vertexCount = mesh.vertices.Length;
            indexCount = mesh.indices.Length;

            vertexBuffer = new VertexBuffer(Main.Instance.GraphicsDevice, VertexPositionColor.VertexDeclaration, vertexCount, BufferUsage.WriteOnly);
            vertexBuffer.SetData(mesh.vertices);

            indexBuffer = new IndexBuffer(Main.Instance.GraphicsDevice, typeof(short), indexCount, BufferUsage.WriteOnly);
            indexBuffer.SetData(mesh.indices);
        }
    }
}
