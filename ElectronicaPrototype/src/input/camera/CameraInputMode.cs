using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Electronica.Graphics.Output;
using Microsoft.Xna.Framework;

namespace Electronica.Input.CameraInput
{

    /// <summary>
    /// A container for translation and rotation matrices;
    /// </summary>
    public class CameraTranslation
    {
        public Matrix translation = Matrix.CreateTranslation(0f, 0f, 0f);
        public Matrix rotation = Matrix.CreateRotationZ(0f);
    }

    /// <summary>
    /// A base class for easiliy customizeable camera movement logic.
    /// </summary>
    public abstract class CameraInputMode
    {
        public static FreeMovement FreeMovement = new FreeMovement();

        protected CameraTranslation currentTranslation;

        public abstract void Update(Camera camera, GameTime gameTime);
    }
}
