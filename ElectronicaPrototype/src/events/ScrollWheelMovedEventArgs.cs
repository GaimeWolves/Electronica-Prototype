using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Electronica.Events
{
    /// <summary>
    /// Args for the scrolled event
    /// </summary>
    public class ScrollWheelMovedEventArgs : EventArgs
    {
        public int Position { get; set; }
        public int Delta { get; set; }
        public float Speed { get; set; }
    }
}
