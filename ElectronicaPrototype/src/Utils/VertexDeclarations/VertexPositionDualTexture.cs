using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Electronica.Utils.VertexDeclarations
{

    /// <summary>
    /// A vertex declaration with a position and two textures.
    /// </summary>
    public struct VertexPositionDualTexture
    {
        public Vector3 Position { get; set; }
        public Vector2 Texture1 { get; set; }
        public Vector2 Texture2 { get; set; }

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(float) * 5, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1)
        );

        public VertexPositionDualTexture(Vector3 position, Vector2 texture1, Vector2 texture2)
        {
            Position = position;
            Texture1 = texture1;
            Texture2 = texture2;
        }
    }
}
