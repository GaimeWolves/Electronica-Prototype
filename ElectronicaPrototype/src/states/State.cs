using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronica.States
{
    public abstract class State : IDisposable
    {
        public bool Disposed { get; private set; }

        protected internal abstract void Initialize();    
        private protected abstract void LoadContent();
        public abstract void Update(Microsoft.Xna.Framework.GameTime gameTime);
        public abstract void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch);
        private protected abstract void UnloadContent();

        ~State() => Dispose(false);
        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    UnloadContent();
                }

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
