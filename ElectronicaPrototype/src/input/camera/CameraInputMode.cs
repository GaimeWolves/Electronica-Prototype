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
    /// A base class for easiliy customizeable camera movement logic.
    /// </summary>
    public abstract class CameraInputMode
    {
        public static FreeMovement FreeMovement = new FreeMovement();
        public static TargetedMovement TargetedMovement = new TargetedMovement();

        public abstract void Update(Camera camera, GameTime gameTime);
    }
}
