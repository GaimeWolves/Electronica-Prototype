using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Electronica.Events
{
    /// <summary>
    /// Args for the mouse moved event.
    /// </summary>
    public class MouseMovedEventArgs : EventArgs
    {
        public Vector2 Position { get; set; }
        public Vector2 DeltaPosition { get; set; }
        public Vector2 Speed { get; set; }
        public bool LeftButtonPressed { get; set; }
        public bool RightButtonPressed { get; set; }
        public bool MiddleButtonPressed { get; set; }
    }
}
