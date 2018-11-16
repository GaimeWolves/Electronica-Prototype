﻿using System;
using Electronica.Circuits.Modules;
using Electronica.Input;
using Electronica.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Electronica.Base
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        /// <summary>
        /// A global variable for the instance for easy access to GraphicsDeviceManagers, InputHandlers, etc.
        /// </summary>
        public static Main Instance { get; private set; }

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public MouseInputHander MouseInputHandler { get; private set; }
        public KeyboardInputHandler KeyboardInputHandler { get; private set; }

        private StateManager stateManager;

        public Main()
        {
            Instance = this;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

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
            MouseInputHandler = new MouseInputHander();
            KeyboardInputHandler = new KeyboardInputHandler();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            stateManager = new StateManager(new StateGame());
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            stateManager.Dispose();
            Content.Unload();
         }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseInputHandler.Update(gameTime);
            KeyboardInputHandler.Update();

            stateManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            stateManager.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
