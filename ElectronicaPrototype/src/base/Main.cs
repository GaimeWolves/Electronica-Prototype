using Electronica.Input;
using Electronica.States;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Electronica.Base
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        private static Main mInstance;

        private GraphicsDeviceManager mGraphics;
        private SpriteBatch mSpriteBatch;

        private StateManager mStateManager;

        public Main()
        {
            mInstance = this;

            mGraphics = new GraphicsDeviceManager(this);
            mGraphics.PreferredBackBufferWidth = 800;
            mGraphics.PreferredBackBufferHeight = 600;

            Content.RootDirectory = "Content";

            Window.Title = "Electronica";

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(GraphicsDevice);

            mStateManager = new StateManager(new StateGame(), mGraphics);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            mStateManager.Dispose();
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputHandler.Update(gameTime);
            mStateManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            mStateManager.Draw(mSpriteBatch);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Closes the game.
        /// </summary>
        public static void Close() => mInstance.Exit();
    }
}