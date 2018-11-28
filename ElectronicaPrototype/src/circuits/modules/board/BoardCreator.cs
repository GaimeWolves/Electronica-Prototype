using System;
using System.Collections.Generic;
using System.Linq;
using Electronica.Utils;
using Electronica.Utils.VertexDeclarations;
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
        public Polygon polygon;
        public VertexPositionDualTexture[] vertexArray;
        public short[] indexArray;
    }

    /// <summary>
    /// A helper class for creating the circuit boards mesh.
    /// </summary>
    public static class BoardCreator
    {
        /// <summary>
        /// This algorithm creates the mesh for circuit boards.
        /// </summary>
        /// <param name="upperVertices">The list of points.</param>
        /// <param name="thickness">The thickness of the board.</param>
        /// <returns>A mesh holding the created board.</returns>
        public static Mesh CreateBoard(List<Vector2> upperVertices, float thickness)
        {
            Polygon polygon = Triangulate(upperVertices);
            List<short> upperFaceIndices = IndexPolygon(polygon, upperVertices);

            List<Vector3> lowerVertices = TranslateUpperPlane(upperVertices, thickness);
            List<short> lowerFaceIndices = IndexLowerPlane(upperFaceIndices, upperVertices.Count);

            List<short> sideFacesIndices = IndexSides(upperVertices.Count);

            return CreateMesh(upperFaceIndices, sideFacesIndices, lowerFaceIndices, upperVertices, lowerVertices, polygon);
        }

        /// <summary>
        /// Creates the mesh instance based on the values calculated beforehand.
        /// </summary>
        /// <param name="upperIndices">The indices of the upper face.</param>
        /// <param name="sideIndices">The indices of the side faces.</param>
        /// <param name="lowerIndices">The indices of the lower face.</param>
        /// <param name="upperVertices">The vertices of the upper face.</param>
        /// <param name="lowerVertices">The vertices of the lower face.</param>
        /// <param name="polygon">The polygon for checking points etc.</param>
        /// <returns>The created mesh.</returns>
        private static Mesh CreateMesh(List<short> upperIndices, List<short> sideIndices, List<short> lowerIndices, List<Vector2> upperVertices, List<Vector3> lowerVertices, Polygon polygon)
        {
            Mesh mesh = new Mesh();

            List<short> indices = new List<short>();
            indices.AddRange(upperIndices);
            indices.AddRange(sideIndices);
            lowerIndices.Reverse();
            indices.AddRange(lowerIndices);

            mesh.indexArray = indices.ToArray();

            List<Vector3> vertices = new List<Vector3>();

            foreach (Vector2 vector in upperVertices)
                vertices.Add(new Vector3(vector.X, 0, vector.Y));

            vertices.AddRange(lowerVertices);

            mesh.vertexArray = CreateUVMap(vertices);
            mesh.polygon = polygon;

            return mesh;
        }

        /// <summary>
        /// Creates the VertexPositionTexture array and uv maps the vertices.
        /// </summary>
        /// <param name="vertices">The vertices to map.</param>
        /// <returns>The VertexPositionTexture array.</returns>
        private static VertexPositionDualTexture[] CreateUVMap(List<Vector3> vertices)
        {
            VertexPositionDualTexture[] vertexArray = new VertexPositionDualTexture[vertices.Count];

            float minX = vertices.OrderBy(v => v.X).ToArray()[0].X;
            float minZ = vertices.OrderBy(v => v.X).ToArray()[0].Z;

            float maxX = vertices.OrderByDescending(v => v.X).ToArray()[0].X;
            float maxZ = vertices.OrderByDescending(v => v.X).ToArray()[0].Z;

            float absX = Math.Abs(minX);
            float absZ = Math.Abs(maxZ);

            for (int i = 0; i < vertices.Count; i++)
            {
                VertexPositionDualTexture vertex = new VertexPositionDualTexture();
                vertex.Position = vertices[i];
                vertex.Texture1 = new Vector2(vertices[i].X + absX, vertices[i].Z + absZ);
                vertex.Texture2 = new Vector2(MathUtils.NormalizeData(minX, maxX, vertices[i].X), MathUtils.NormalizeData(minZ, maxZ, vertices[i].Z));
                vertexArray[i] = vertex;
            }

            return vertexArray;
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
            List<short> lowerFaceIndices = new List<short>(upperPlaneIndices);

            for (int i = 0; i < lowerFaceIndices.Count; i++)
                lowerFaceIndices[i] += (short)vertexCount;

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