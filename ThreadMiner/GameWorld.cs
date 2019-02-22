using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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

        public Background background;

        public TownHall townHall;

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

            background = new Background(this);

            buildings = new List<Building>();
            townHall = new TownHall(this, Vector2.Zero, "TownHall");
            buildings.Add(townHall);
            buildings.Add(new Mine(this, new Vector2(0, 500), "Mine"));
            units = new List<Unit>();
            uiElements = new List<UIElement>();
            uiElements.Add(new ResourcePanel(this, new Vector2(GraphicsDevice.Viewport.Bounds.Width - 200, 10), "", new Vector2(200, 50)));
            uiElements.Add(new PopulationPanel(this, new Vector2(GraphicsDevice.Viewport.Bounds.Width - 200, 60), "/", new Vector2(200, 50)));
            uiElements.Add(new UnitPanel(this, new Vector2(GraphicsDevice.Viewport.Bounds.Width - 100, GraphicsDevice.Viewport.Bounds.Height - 100), "Unit"));
            uiElements.Add(new HousePanel(this, new Vector2(GraphicsDevice.Viewport.Bounds.Width - 200, GraphicsDevice.Viewport.Bounds.Height - 100), "House"));
            uiElements.Add(new MinePanel(this, new Vector2(GraphicsDevice.Viewport.Bounds.Width - 300, GraphicsDevice.Viewport.Bounds.Height - 100), "Mine"));
            uiElements.Add(new BarrackPanel(this, new Vector2(GraphicsDevice.Viewport.Bounds.Width - 400, GraphicsDevice.Viewport.Bounds.Height - 100), "Barrack"));

            spriteBatch = new SpriteBatch(GraphicsDevice);
            inputManager = new InputManager(this);

            debugRectangle = new Texture2D(GraphicsDevice, 1, 1);
            debugRectangle.SetData(new[] {Color.White});
            selectedUnits = new List<Unit>();
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
            inputManager.ResetValues();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            spriteBatch.Begin(transformMatrix: cam.Transform, samplerState: SamplerState.PointWrap, blendState: BlendState.NonPremultiplied, sortMode: SpriteSortMode.FrontToBack);
            background.Draw(spriteBatch);
            spriteBatch.End();


            spriteBatch.Begin(transformMatrix: cam.Transform, samplerState: SamplerState.PointClamp, blendState: BlendState.NonPremultiplied, sortMode: SpriteSortMode.FrontToBack);
            foreach (Building building in buildings)
            {
                building.Draw(gameTime, spriteBatch);
                //spriteBatch.Draw(debugRectangle, building.WorldBounds, new Color(255,0, 55, 10));
            }

            foreach (Unit unit in units)
            {
                unit.DrawAnimated(gameTime, spriteBatch);
                //spriteBatch.Draw(debugRectangle,unit.WorldBounds ,Color.Green);
            }
            spriteBatch.End();

            spriteBatch.Begin(transformMatrix: cam.Transform);
            if (inputManager.Selecting)
            {
                spriteBatch.Draw(debugRectangle, inputManager.SelectionBox, new Color(0, 50, 10, 1));
            }
            if (selectedUnits != null && selectedUnits.Count>0)
            {
                foreach (Unit unit in selectedUnits)
                {
                    if (unit != null)
                    {
                        spriteBatch.Draw(debugRectangle, unit.WorldBounds, new Color(0, 255, 55, 10));
                    }
                }
            }

            spriteBatch.Draw(debugRectangle, inputManager.buildingInterSection, new Color(255, 0, 0, 1));

            spriteBatch.End();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(debugRectangle, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), new Color((byte)0, (byte)0, (byte)0, (byte)(80*(1+Math.Cos(Math.PI+ 0.05f*gameTime.TotalGameTime.TotalSeconds)))));
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
