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

        public bool IsTargetedMovementMode { get; protected set; }

        public abstract void Update(Camera camera, float deltaTime);

        public virtual void SetTarget(Vector3? target) { }
    }
}