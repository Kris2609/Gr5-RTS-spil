﻿using Microsoft.Xna.Framework;
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
            get => selectionBox;
        }
        public bool Selecting { get => selecting;}

        Vector2 startSelectionDrag;
        Vector2 currentSelectionDrag;
        Rectangle selectionBox;
        float timeHeldButton;

        MouseState oldMouseState;
        
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
            StartSelectionDrag(gameWorld.cam, gameTime);
            SelectionDrag(gameWorld.cam, gameTime);

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
        

        void StartSelectionDrag(Camera cam, GameTime gameTime)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == oldMouseState.LeftButton && !selecting)
            {
                startSelectionDrag = Vector2.Zero;
                currentSelectionDrag = Vector2.Zero;

                selecting = true;
                startSelectionDrag = (mousePos - Camera.HalfSize) * (1f / cam.Zoom) + cam.camPos;

            }
        }
        public List<Unit> SelectionDrag(Camera cam, GameTime gameTime)
        {
                List<Unit> selectedUnits = new List<Unit>();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                currentSelectionDrag = (mousePos - Camera.HalfSize) * (1f / cam.Zoom) + cam.camPos;
                selectionBox = new Rectangle(
                    GameWorld.V2P(Vector2.Min(startSelectionDrag, currentSelectionDrag)),
                    GameWorld.V2P(Vector2.Max(startSelectionDrag, currentSelectionDrag) - Vector2.Min(startSelectionDrag, currentSelectionDrag))
                    );

                foreach (Unit unit in gameWorld.units)
                {
                    if (selectionBox.Contains(unit.WorldBounds))
                    {
                        selectedUnits.Add(unit);
                    }
                }
                gameWorld.selectedUnits = selectedUnits;
            }
            else if(oldMouseState.LeftButton == ButtonState.Pressed)
            {
                currentSelectionDrag = (mousePos - Camera.HalfSize) * (1f / cam.Zoom) + cam.camPos;
                selectionBox = new Rectangle(
                    GameWorld.V2P(Vector2.Min(startSelectionDrag, currentSelectionDrag)),
                    GameWorld.V2P(Vector2.Max(startSelectionDrag, currentSelectionDrag) - Vector2.Min(startSelectionDrag, currentSelectionDrag))
                    );
                selectedUnits.Add(gameWorld.units.Find(x => x.WorldBounds.Contains(selectionBox)));
                if (selectedUnits.Count > 2)
                {
                    gameWorld.selectedUnits = selectedUnits;
                }

            }
            else
            {
                startSelectionDrag = Vector2.Zero;
                currentSelectionDrag = Vector2.Zero;
                selecting = false;
            }
            return selectedUnits;
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
