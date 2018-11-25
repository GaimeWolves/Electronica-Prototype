using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Electronica.Circuits.Modules;
using Microsoft.Xna.Framework;

namespace Electronica.Raycast
{
    public class RayCast
    {
        public Vector3 Position { get; private set; }
        public Vector3 Direction { get; private set; }
        public Vector3? Hit { get; private set; }

        public RayCast(Vector3 position, Vector3 direction)
        {
            Position = position;
            Direction = direction;
            Direction.Normalize();
        }

        /// <summary>
        /// Casts the ray checking for the circuit board.
        /// </summary>
        /// <param name="length">The length to check.</param>
        /// <param name="iterations">The amount of iterations.</param>
        /// <param name="board">The board to check.</param>
        /// <returns>The hit position if theres one.</returns>
        public Vector3? CastBoard(float length, int iterations, Board board)
        {
            if (Direction.Y == 0 && (Position.Y >= 0 || Position.Y <= -board.Thickness))
                return Hit = null;

            Vector3 step = Direction * (length / iterations);
            Vector3 currentPosition = Position;

            for (int i = 0; i < iterations; i++)
            {
                currentPosition += step;

                if (board.IsPointInside(currentPosition))
                    return Hit = currentPosition;
            }

            return Hit = null;
        }
    }
}
