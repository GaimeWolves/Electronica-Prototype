using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Electronica.States
{
    /// <summary>
    /// A manager class for easy managment of game states.
    /// </summary>
    public class StateManager : IDisposable
    {
        public static State CurrentState { get; private set; }

        private bool disposed = false;

        public StateManager(State startingState)
        {
            CurrentState = startingState;
            CurrentState.Initialize();
        }

        /// <summary>
        /// Updates the current active state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime) => CurrentState.Update(gameTime);

        /// <summary>
        /// Draws the current active state.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw.</param>
        public void Draw(SpriteBatch spriteBatch) => CurrentState.Draw(spriteBatch);

        /// <summary>
        /// Disposes the old state and sets currentState to the new one.
        /// </summary>
        /// <param name="state">The new state.</param>
        public void SetState(State state)
        {
            CurrentState.Dispose();
            CurrentState = state;
            CurrentState.Initialize();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                CurrentState.Dispose();

                disposed = true;
            }
        }

        ~StateManager() => Dispose(false);

        public void Dispose()
        {   
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
