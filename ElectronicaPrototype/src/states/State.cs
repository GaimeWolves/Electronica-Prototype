using System;

using Microsoft.Xna.Framework;

namespace Electronica.States
{
    /// <summary>
    /// A base class for game states.
    /// </summary>
    public abstract class State : IDisposable
    {
        public bool Disposed { get; private set; }

        public GraphicsDeviceManager Graphics { get; protected internal set; }

        protected internal abstract void Initialize(GraphicsDeviceManager graphics);

        private protected abstract void LoadContent();

        public abstract void Update(GameTime gameTime, float deltaTime);

        public abstract void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch);

        private protected abstract void UnloadContent();

        ~State() => Dispose(false);

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                    UnloadContent();

                Disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}