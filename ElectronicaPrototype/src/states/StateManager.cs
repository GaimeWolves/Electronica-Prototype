using System;

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

        public StateManager(State startingState, GraphicsDeviceManager graphics)
        {
            CurrentState = startingState;
            CurrentState.Initialize(graphics);
        }

        /// <summary>
        /// Updates the current active state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime) => CurrentState.Update(gameTime, (float)gameTime.ElapsedGameTime.TotalSeconds);

        /// <summary>
        /// Draws the current active state.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw.</param>
        public void Draw(SpriteBatch spriteBatch) => CurrentState.Draw(spriteBatch);

        /// <summary>
        /// Disposes the old state and sets currentState to the new one.
        /// </summary>
        /// <param name="state">The new state.</param>
        public void SetState(State state, GraphicsDeviceManager graphics)
        {
            CurrentState.Dispose();
            CurrentState = state;
            CurrentState.Initialize(graphics);
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