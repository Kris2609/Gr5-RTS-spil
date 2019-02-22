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
        public List<Enemy> enemies;

        public List<Unit> selectedUnits;

        public Camera cam;
        public InputManager inputManager;
        public EnemySpawner enemySpawner;

        Texture2D debugRectangle;

        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        


        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// Here the basic level design is set up.
        /// </summary>
        protected override void LoadContent()
        {
            //Initializing all "starter" objects.
            //Also initialising list that are run through to call update functions.
            cam = new Camera();
            Camera.HalfSizeX = graphics.PreferredBackBufferWidth / 2;
            Camera.HalfSizeY = graphics.PreferredBackBufferHeight / 2;
            
            uiElements = new List<UIElement>();
            buildings = new List<Building>();
            enemies = new List<Enemy>();
            units = new List<Unit>();
            
            enemySpawner = new EnemySpawner(this);
            background = new Background(this);

            units.Add(new Worker(this, new Vector2(0, 16), 30, Worker.WorkerJob.Idle));

            townHall = new TownHall(this, Vector2.Zero, "TownHall");
            buildings.Add(townHall);
            buildings.Add(new Mine(this, new Vector2(0, 500), "Mine"));

            uiElements.Add(new ResourcePanel(this, new Vector2(GraphicsDevice.Viewport.Bounds.Width - 200, 10), "", new Vector2(200, 50)));
            uiElements.Add(new PopulationPanel(this, new Vector2(GraphicsDevice.Viewport.Bounds.Width - 200, 60), "/", new Vector2(200, 50)));
            uiElements.Add(new UnitPanel(this, new Vector2(GraphicsDevice.Viewport.Bounds.Width - 100, GraphicsDevice.Viewport.Bounds.Height - 100), "Unit"));
            uiElements.Add(new HousePanel(this, new Vector2(GraphicsDevice.Viewport.Bounds.Width - 200, GraphicsDevice.Viewport.Bounds.Height - 100), "House"));
            uiElements.Add(new MinePanel(this, new Vector2(GraphicsDevice.Viewport.Bounds.Width - 300, GraphicsDevice.Viewport.Bounds.Height - 100), "Mine"));
            uiElements.Add(new BarrackPanel(this, new Vector2(GraphicsDevice.Viewport.Bounds.Width - 400, GraphicsDevice.Viewport.Bounds.Height - 100), "Barrack"));
            
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Inputmanager handles all input the User brings to the game including keypresses and mouse movement.
            inputManager = new InputManager(this);


            //Creating a 1x1 texture, meaning a colored square for use in selection and the debugging process.
            debugRectangle = new Texture2D(GraphicsDevice, 1, 1);
            debugRectangle.SetData(new[] {Color.White});
            //A list to hold the units to be selected by inputmanager.
            selectedUnits = new List<Unit>();
        }

       

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// This encapsulates the game loop.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Each loop the user input is handled first, the each list of gameobjects is run through.

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
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);
            }
            //Lastly the camera-matrix is applied and the inputmanagers "between loop"-fields are set.
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

            //Same principle as Update except there are different uses of spritebatch parameters depending on needs.
            // Here SamplerState.PointWrap is used in order to allow the background to be tiled.
            spriteBatch.Begin(transformMatrix: cam.Transform, samplerState: SamplerState.PointWrap, blendState: BlendState.NonPremultiplied, sortMode: SpriteSortMode.FrontToBack);
            background.Draw(spriteBatch);
            spriteBatch.End();

            // In here all normal objects are drawn, the camera matrix is applied and alpha blending is applied.
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
            foreach (Enemy enemy in enemies)
            {
                enemy.DrawAnimated(gameTime, spriteBatch);
            }

            spriteBatch.End();

            // This is where the "world bound" UI is drawn.
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

            //This is the UI spritebatch, no cameramatrix is applied.
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(debugRectangle, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), new Color((byte)0, (byte)0, (byte)0, (byte)(80*(1+Math.Cos(Math.PI+ 0.05f*gameTime.TotalGameTime.TotalSeconds)))));
            foreach (UIElement ui in uiElements)
            {
                ui.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        //Functions implemented before realizing Point.ToVector2 and vice versa existed.
        /// <summary>
        /// Returns a Point from a given Vector2
        /// </summary>
        [Obsolete("V2P is deprecated, please use Vector2.ToPoint()")]
        public static Point V2P(Vector2 V)
        {
            return new Point((int)V.X, (int)V.Y);
        }
        /// <summary>
        /// Returns a Vector2 from a given Point
        /// </summary>
        [Obsolete("P2V is deprecated, please use Point.ToVector2()")]
        public static Vector2 P2V(Point V)
        {
            return new Vector2(V.X, V.Y);
        }
    }
}
