using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Poly2Tri;
using Poly2Tri.Triangulation.Delaunay;
using Poly2Tri.Triangulation.Polygon;
using Poly2Tri.Utility;

namespace Electronica.Circuits.Modules
{
    /// <summary>
    /// A data structure holding the mesh.
    /// </summary>
    public struct Mesh
    {
        public VertexPositionColor[] vertices;
        public short[] indices;
    }

    /// <summary>
    /// A helper class for creating the circuit boards mesh.
    /// </summary>
    public static class BoardCreator
    {
        /// <summary>
        /// This algorithm creates the mesh for convex circuit boards.
        /// </summary>
        /// <param name="upperVertices">The list of points.</param>
        /// <param name="thickness">The thickness of the board.</param>
        /// <returns></returns>
        public static Mesh CreateBoard(List<Vector2> upperVertices, float thickness)
        {
            Polygon polygon = Triangulate(upperVertices);
            List<short> upperFaceIndices = IndexPolygon(polygon, upperVertices);

            List<Vector3> lowerVertices = TranslateUpperPlane(upperVertices, thickness);
            List<short> lowerFaceIndices = IndexLowerPlane(upperFaceIndices, upperVertices.Count);

            List<short> sideFacesIndices = IndexSides(upperVertices.Count);

            return CreateMesh(upperFaceIndices, sideFacesIndices, lowerFaceIndices, upperVertices, lowerVertices);
        }

        /// <summary>
        /// Creates the mesh instance based on the values calculated beforehand.
        /// </summary>
        /// <param name="upperIndices">The indices of the upper face.</param>
        /// <param name="sideIndices">The indices of the side faces.</param>
        /// <param name="lowerIndices">The indices of the lower face.</param>
        /// <param name="upperVertices">The vertices of the upper face.</param>
        /// <param name="lowerVertices">The vertices of the lower face.</param>
        /// <returns>The created mesh.</returns>
        private static Mesh CreateMesh(List<short> upperIndices, List<short> sideIndices, List<short> lowerIndices, List<Vector2> upperVertices, List<Vector3> lowerVertices)
        {
            Mesh mesh = new Mesh();

            List<short> indices = new List<short>();
            indices.AddRange(upperIndices);
            indices.AddRange(sideIndices);
            indices.AddRange(lowerIndices);

            mesh.indices = indices.ToArray();

            List<Vector3> vertices = new List<Vector3>();

            foreach (Vector2 vector in upperVertices)
                vertices.Add(new Vector3(vector.X, 0, vector.Y));

            vertices.AddRange(lowerVertices);

            VertexPositionColor[] vertexArray = new VertexPositionColor[vertices.Count];

            for (int i = 0; i < vertices.Count; i++)
                vertexArray[i] = new VertexPositionColor(vertices[i], Color.DarkGreen);

            mesh.vertices = vertexArray;

            return mesh;
        }

        /// <summary>
        /// Creates the list of indexes for the side faces.
        /// </summary>
        /// <param name="vertexCount">The amount of vertices of one face (upper or lower face).</param>
        /// <returns>A list of indices.</returns>
        private static List<short> IndexSides(int vertexCount)
        {
            List<short> sideFacesIndices = new List<short>();

            for (short i = 0; i < vertexCount; i++)
            {
                short n = (short)((i + 1) % vertexCount);
                sideFacesIndices.Add((short)(i + vertexCount));
                sideFacesIndices.Add(i);
                sideFacesIndices.Add(n);

                sideFacesIndices.Add((short)(i + vertexCount));
                sideFacesIndices.Add(n);
                sideFacesIndices.Add((short)(n + vertexCount));
            }

            return sideFacesIndices;
        }

        /// <summary>
        /// Creates the list of indices for the lower plane.
        /// </summary>
        /// <param name="upperPlaneIndices">The upper plane's indices.</param>
        /// <param name="vertexCount">The amount of vertices of the upper plane.</param>
        /// <returns>A list of indices.</returns>
        private static List<short> IndexLowerPlane(List<short> upperPlaneIndices, int vertexCount)
        {
            List<short> lowerFaceIndices = new List<short>();

            for (short i = 0; i < vertexCount; i++)
                lowerFaceIndices.Add((short)(upperPlaneIndices[i] + vertexCount));

            return lowerFaceIndices;
        }

        /// <summary>
        /// Translates the upper planes vertices the specified thickness downwards (-Y).
        /// </summary>
        /// <param name="upperVertices">The upper plane's vertices</param>
        /// <param name="thickness">The thickness of the board.</param>
        /// <returns>The translated vertices.</returns>
        private static List<Vector3> TranslateUpperPlane(List<Vector2> upperVertices, float thickness)
        {
            List<Vector3> lowerVertices = new List<Vector3>(upperVertices.Count);

            foreach (Vector2 point in upperVertices)
                lowerVertices.Add(new Vector3(point.X, -thickness, point.Y));

            return lowerVertices;
        }

        /// <summary>
        /// Creates the list of indices for the upper plane.
        /// </summary>
        /// <param name="polygon">The polygon triangulated beforehand.</param>
        /// <param name="originalVertices">The original list of vertices.</param>
        /// <returns>A list of indices.</returns>
        private static List<short> IndexPolygon(Polygon polygon, List<Vector2> originalVertices)
        {
            List<short> indices = new List<short>();

            foreach (DelaunayTriangle triangle in polygon.Triangles)
                foreach (Point2D point in triangle.Points)
                    for (short i = 0; i < originalVertices.Count; i++)
                        if (originalVertices[i].X == point.X && originalVertices[i].Y == point.Y)
                            indices.Add(i);

            return indices;
        }

        /// <summary>
        /// Triangulates the point cloud.
        /// </summary>
        /// <param name="points">The point cloud.</param>
        /// <returns>A triangulated polygon.</returns>
        private static Polygon Triangulate(List<Vector2> points)
        {
            PolygonPoint[] tempPoints = new PolygonPoint[points.Count];
            for (int i = 0; i < points.Count; i++)
                tempPoints[i] = new PolygonPoint(points[i].X, points[i].Y);

            Polygon polygon = new Polygon(tempPoints);
            P2T.Triangulate(polygon);
            return polygon;
        }
    }
}
