﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ThreadMiner
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWorld : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public List<Unit> units;
        public List<UIElement> uiElements;
        public List<Building> buildings;

        public List<Unit> selectedUnits;

        public Camera cam;
        public InputManager inputManager;

        Texture2D debugRectangle;

        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            cam = new Camera();
            Camera.HalfSizeX = graphics.PreferredBackBufferWidth / 2;
            Camera.HalfSizeY = graphics.PreferredBackBufferHeight / 2;

            buildings = new List<Building>();
            buildings.Add(new TownHall(this, Vector2.Zero, "TownHall"));
            buildings.Add(new Mine(this, new Vector2(0, 500), "Mine"));
            units = new List<Unit>();
            uiElements = new List<UIElement>();
            uiElements.Add(new Button(this, new Vector2(40, 40)));
            uiElements.Add(new Panel(this, new Vector2(GraphicsDevice.Viewport.Bounds.Width - 100, GraphicsDevice.Viewport.Bounds.Height - 100), "House"));

            spriteBatch = new SpriteBatch(GraphicsDevice);
            inputManager = new InputManager(this);

            debugRectangle = new Texture2D(GraphicsDevice, 1, 1);
            debugRectangle.SetData(new[] {Color.White});

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            inputManager.HandleInput(gameTime);

            foreach (Building building in buildings)
            {
                building.Update(gameTime);
            }

            foreach (Unit unit in units)
            {
                unit.Update(gameTime);
            }
            foreach (UIElement ui in uiElements)
            {
                ui.Update(gameTime);
            }

            cam.CalcFullMatrix();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);
            spriteBatch.Begin(transformMatrix: cam.Transform, samplerState: SamplerState.PointClamp);
            foreach (Building building in buildings)
            {
                building.Draw(gameTime, spriteBatch);
                spriteBatch.Draw(debugRectangle, building.WorldBounds, new Color(255,0, 55, 10));
            }

            foreach (Unit unit in units)
            {
                unit.DrawAnimated(gameTime, spriteBatch);
                //spriteBatch.Draw(debugRectangle,unit.WorldBounds ,Color.Green);
            }

            if (inputManager.Selecting)
            {
                spriteBatch.Draw(debugRectangle, inputManager.SelectionBox, new Color(0, 50, 10, 1));
            }
            foreach (Unit unit in inputManager.SelectionDrag(cam))
            {
                spriteBatch.Draw(debugRectangle, unit.WorldBounds, new Color(0,255,55,10));
            }

            spriteBatch.End();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            foreach (UIElement ui in uiElements)
            {
                ui.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        
        public static Point V2P(Vector2 V)
        {
            return new Point((int)V.X, (int)V.Y);
        }
        public static Vector2 P2V(Point V)
        {
            return new Vector2(V.X, V.Y);
        }
    }
}
