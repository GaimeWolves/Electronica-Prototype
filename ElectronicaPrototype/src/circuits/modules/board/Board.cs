using System.Collections.Generic;
using Electronica.Base;
using Electronica.Graphics.Output;
using Electronica.States;
using Electronica.Utils.VertexDeclarations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Poly2Tri.Triangulation.Polygon;

namespace Electronica.Circuits.Modules
{
    public class Board
    {
        private static readonly float StandardBoardSize = 5;
        private static readonly float StandardThickness = 0.1f;

        private static Effect mEffect;
        private static Texture2D mBase, mWirering;

        public float Thickness { get; private set; }

        /// <summary>
        /// !Ordered! list of vertices
        /// </summary>
        private List<Vector2> mVertices;

        private VertexBuffer mVertexBuffer;
        private IndexBuffer mIndexBuffer;
        private int mVertexCount, mIndexCount;
        private Polygon mPolygon;

        public static void LoadContent(ContentManager content)
        {
            mBase = content.Load<Texture2D>("board/base");
            mEffect = content.Load<Effect>("board/shader");
            mWirering = content.Load<Texture2D>("board/wirering");
        }

        public Board()
        {
            Thickness = StandardThickness;

            mVertices = new List<Vector2>(4);
            mVertices.Add(new Vector2(-StandardBoardSize, -StandardBoardSize));
            mVertices.Add(new Vector2(-StandardBoardSize, StandardBoardSize));
            mVertices.Add(new Vector2(StandardBoardSize, StandardBoardSize));
            mVertices.Add(new Vector2(StandardBoardSize, -StandardBoardSize));

            UpdateBuffers();
        }

        /// <summary>
        /// Draws the board on the screen.
        /// </summary>
        /// <param name="graphics">The current GraphicsDeviceManager.</param>
        /// <param name="projection">The projection matrix of the camera.</param>
        /// <param name="view">The view matrix of the camera.</param>
        public void Draw(GraphicsDeviceManager graphics, Camera camera, Matrix parentTransform)
        {
            mEffect.Parameters["World"].SetValue(parentTransform);
            mEffect.Parameters["View"].SetValue(camera.ViewMatrix);
            mEffect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
            mEffect.Parameters["ModelTexture"].SetValue(mBase);
            mEffect.Parameters["BrightnessMap"].SetValue(mWirering);

            graphics.GraphicsDevice.Indices = mIndexBuffer;
            graphics.GraphicsDevice.SetVertexBuffer(mVertexBuffer);

            foreach (EffectPass pass in mEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, mIndexCount / 3);
            }
        }

        /// <summary>
        /// Generates a new mesh from the vertices and updates the vertex and index buffer.
        /// </summary>
        private void UpdateBuffers()
        {
            Mesh mesh = BoardCreator.CreateBoard(mVertices, Thickness);
            mVertexCount = mesh.vertexArray.Length;
            mIndexCount = mesh.indexArray.Length;

            mPolygon = mesh.polygon;

            mVertexBuffer = new VertexBuffer(StateManager.CurrentState.Graphics.GraphicsDevice, VertexPositionDualTexture.VertexDeclaration, mVertexCount, BufferUsage.WriteOnly);
            mVertexBuffer.SetData(mesh.vertexArray);

            mIndexBuffer = new IndexBuffer(StateManager.CurrentState.Graphics.GraphicsDevice, typeof(short), mIndexCount, BufferUsage.WriteOnly);
            mIndexBuffer.SetData(mesh.indexArray);
        }

        /// <summary>
        /// Checks if a point is inside the board.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <returns>Is the point inside?</returns>
        public bool IsPointInside(Vector3 point)
        {
            if (point.Y > 0 || point.Y < -Thickness)
                return false;

            return mPolygon.IsPointInside(new Poly2Tri.Triangulation.TriangulationPoint(point.X, point.Z));
        }
    }
}