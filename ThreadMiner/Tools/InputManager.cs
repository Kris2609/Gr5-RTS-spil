using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMiner
{
    public class InputManager
    {
        public MouseState mouseState;

        public KeyboardState keyboardState;

        public GameWorld gameWorld;

        int scrollVal;
        int oldScrollVal;

        int camMoveSpeed = 500;

        bool selecting = false;

        public Rectangle SelectionBox
        {
            get => new Rectangle(GameWorld.V2P(startSelectionDrag), GameWorld.V2P(currentSelectionDrag - startSelectionDrag));
        }

        Vector2 startSelectionDrag;
        Vector2 currentSelectionDrag;
        Rectangle selectionBox;

        MouseState oldMouseState;

        Texture2D workerOutline;
        Vector2 mousePos;

        public InputManager(GameWorld gameWorld)
        {
            this.gameWorld = gameWorld;
        }

        public void HandleInput(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            mousePos = new Vector2(mouseState.X, mouseState.Y);
            keyboardState = Keyboard.GetState();
            scrollVal = mouseState.ScrollWheelValue;
            ControlCamera(gameTime, gameWorld.cam);

            Placeworker(gameWorld.cam);
            StartSelectionDrag(gameWorld.cam);
            SelectionDrag(gameWorld.cam);

            oldScrollVal = scrollVal;
            oldMouseState = mouseState;
        }

        void ControlCamera(GameTime gameTime, Camera cam)
        {
            cam.Zoom += 0.001f * (scrollVal - oldScrollVal);
            Vector2 camMoveAW = new Vector2(
                keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left) ? -1 : 0,
                keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up) ? -1 : 0
                );
            Vector2 camMoveDS = new Vector2(
                keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right) ? 1 : 0,
                keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down) ? 1 : 0
                );

            Vector2 camMove = (camMoveAW + camMoveDS) * (float)gameTime.ElapsedGameTime.TotalSeconds * camMoveSpeed;
            cam.camPos += camMove;

            cam.CalcFullMatrix();
        }

        void StartSelectionDrag(Camera cam)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == oldMouseState.LeftButton && ! selecting)
            {
                startSelectionDrag = (mousePos - Camera.HalfSize) * (1f / cam.Zoom) + cam.camPos;
            }
        }
        List<Unit> SelectionDrag(Camera cam)
        {
            List<Unit> selectedUnit = new List<Unit>();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                selecting = true;
                currentSelectionDrag = (mousePos - Camera.HalfSize) * (1f / cam.Zoom) + cam.camPos;
                selectionBox = (new Rectangle(GameWorld.V2P(startSelectionDrag), GameWorld.V2P(currentSelectionDrag-startSelectionDrag)));

                foreach (Unit unit in gameWorld.units)
                {
                    if (selectionBox.Contains(unit.WorldBounds))
                    {
                        selectedUnit.Add(unit);
                    }
                }
            }
            else
            {
                selecting = false;
            }
            return selectedUnit;
        }

        void Placeworker(Camera cam)
        {
            if (mouseState.RightButton == ButtonState.Pressed && mouseState.RightButton != oldMouseState.RightButton)
            {
                Vector2 vec = (mousePos - Camera.HalfSize) * (1f / cam.Zoom) + cam.camPos;
                Worker worker = new Worker(gameWorld, vec, 30, Worker.WorkerJob.Mine);

                gameWorld.units.Add(worker);
            }
        }
    }
}
