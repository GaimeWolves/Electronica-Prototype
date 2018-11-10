using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Electronica.States
{
    public class StateManager : IDisposable
    {
        public static State CurrentState { get; private set; }

        private bool disposed = false;

        public StateManager(State startingState)
        {
            CurrentState = startingState;
            CurrentState.Initialize();
        }

        public void Update(GameTime gameTime) => CurrentState.Update(gameTime);
        public void Draw(SpriteBatch spriteBatch) => CurrentState.Draw(spriteBatch);

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
                if (disposing)
                {
                    CurrentState.Dispose();
                }

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
