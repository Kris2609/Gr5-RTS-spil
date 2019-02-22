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

        int camMoveSpeed = 590;

        bool selecting = false;

        public Rectangle buildingInterSection;

        public Rectangle SelectionBox
        {
            get => selectionBox;
        }
        public bool Selecting { get => selecting;}

        Vector2 startSelectionDrag;
        Vector2 currentSelectionDrag;
        Rectangle selectionBox;


        public MouseState oldMouseState;
        
        Vector2 mousePos;

        public enum RightButtonUse { PlaceWorker, ControlUnits, PlaceHouse, PlaceMine, PlaceBarrack};
        public RightButtonUse currentRightUse= RightButtonUse.ControlUnits;

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
            //Placeworker(gameWorld.cam);
            switch (currentRightUse)
            {
                case RightButtonUse.ControlUnits:
                    ControlUnits(gameWorld.cam);
                    buildingInterSection = new Rectangle(0, 0, 0, 0);
                    break;
                case RightButtonUse.PlaceWorker:
                    PlaceWorker(gameWorld.cam);
                    break;
                case RightButtonUse.PlaceHouse:
                    PlaceHouse(gameWorld.cam);
                    break;
                case RightButtonUse.PlaceMine:
                    PlaceMine(gameWorld.cam);
                    break;
                case RightButtonUse.PlaceBarrack:
                    PlaceBarrack(gameWorld.cam);
                    break;
            }
            StartSelectionDrag(gameWorld.cam);
            SelectionDrag(gameWorld.cam);
            EndSelectionDrag(gameWorld.cam);

        }
        public void ResetValues()
        {
            oldScrollVal = scrollVal;
            oldMouseState = mouseState;
        }
        private void PlaceBarrack(Camera cam)
        {
            if (gameWorld.townHall.CurrGold >= House.cost)
            {
                Vector2 vec = (mousePos - Camera.HalfSize) * (1f / cam.Zoom) + cam.CamPos;
                Barrack barrack = new Barrack(gameWorld, vec, "Barrack");
                bool intersects = false;
                foreach (Building building in gameWorld.buildings)
                {
                    if (barrack.WorldBounds.Intersects(building.WorldBounds))
                    {
                        intersects = true;
                        buildingInterSection = Rectangle.Intersect(barrack.WorldBounds, building.WorldBounds);
                    }

                }
                if (!intersects)
                {
                    if (mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
                    {
                        gameWorld.buildings.Add(barrack);

                        currentRightUse = RightButtonUse.ControlUnits;
                        gameWorld.townHall.CurrGold -= Barrack.cost;
                    }
                    buildingInterSection = new Rectangle(0, 0, 0, 0);
                }
            }
        }

        private void PlaceHouse(Camera cam)
        {
            if (gameWorld.townHall.CurrGold >= House.cost)
            {
                    Vector2 vec = (mousePos - Camera.HalfSize) * (1f / cam.Zoom) + cam.CamPos;
                    House house = new House(gameWorld, vec, "House");
                    bool intersects = false;
                    foreach (Building building in gameWorld.buildings)
                    {
                        if (house.WorldBounds.Intersects(building.WorldBounds))
                        {
                            intersects = true;
                            buildingInterSection = Rectangle.Intersect(house.WorldBounds, building.WorldBounds);
                        }

                }
                if (!intersects)
                {
                    if (mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
                    {

                        gameWorld.buildings.Add(house);

                        currentRightUse = RightButtonUse.ControlUnits;
                        gameWorld.townHall.CurrGold -= House.cost;
                    }
                    buildingInterSection = new Rectangle(0, 0, 0, 0);
                }
            }
        }

        private void PlaceMine(Camera cam)
        {
            if (gameWorld.townHall.CurrGold >= Mine.cost)
            {

                Vector2 vec = (mousePos - Camera.HalfSize) * (1f / cam.Zoom) + cam.CamPos;
                Mine mine = new Mine(gameWorld, vec, "Mine");
                bool intersects = false;
                foreach (Building building in gameWorld.buildings)
                {
                    if (mine.WorldBounds.Intersects(building.WorldBounds))
                    {
                        intersects = true;
                        buildingInterSection = Rectangle.Intersect(mine.WorldBounds, building.WorldBounds);
                    }

                }
                if (!intersects)
                {
                    if (mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
                    {

                        gameWorld.buildings.Add(mine);

                        currentRightUse = RightButtonUse.ControlUnits;
                        gameWorld.townHall.CurrGold -= Mine.cost;
                    }
                    buildingInterSection = new Rectangle(0, 0, 0, 0);
                }
            }
        }

        void PlaceWorker(Camera cam)
        {
            if (gameWorld.townHall.CurrGold >= Unit.cost && gameWorld.units.Count < gameWorld.townHall.UnitCap)
            {
                Vector2 vec = gameWorld.townHall.Pos + 150 * new Vector2((float)Math.Sin((new Random()).Next()), (float)Math.Cos((new Random()).Next()));
                Worker worker = new Worker(gameWorld, vec, 30, Worker.WorkerJob.Idle);

                gameWorld.units.Add(worker);
                gameWorld.townHall.CurrGold -= Unit.cost;
            }
            currentRightUse = RightButtonUse.ControlUnits;

        }

        void ControlUnits(Camera cam)
        {
            if (mouseState.RightButton == ButtonState.Pressed && mouseState.RightButton != oldMouseState.RightButton)
            {
                Vector2 vec = (mousePos - Camera.HalfSize) * (1f / cam.Zoom) + cam.CamPos;
                Building building = gameWorld.buildings.Find(x => x.WorldBounds.Contains(vec));
                if (building != null )
                {
                    if (building.GetType() == typeof(Mine))
                    {
                        foreach (Unit unit in gameWorld.selectedUnits)
                        {
                            if (unit != null)
                            {

                                if (unit.GetType() == typeof(Worker))
                                {
                                    ((Worker)unit).CurrMine = (Mine)building;
                                }
                            }
                        }
                    }
                    else if(building.GetType() == typeof(Barrack))
                    {
                        foreach (Unit unit in gameWorld.selectedUnits)
                        {
                            if (unit != null)
                            {
                                if (unit.GetType() == typeof(Worker))
                                {
                                    ((Worker)unit).CurrBarrack = (Barrack)building;
                                }
                            }
                        }
                    }
                }
                Enemy enemy = gameWorld.enemies.Find(x => x.WorldBounds.Contains(vec));
                if (enemy != null)
                {
                    foreach (Unit unit in gameWorld.selectedUnits)
                    {
                        if (unit.GetType() == typeof(Swordsman))
                        {
                            ((Swordsman)unit).targetEnemy = enemy;
                        }
                    }
                }
            }
        }
                
        public void SelectSingle(Camera cam)
        {
            List<Unit> selectedUnits = new List<Unit>();
            if (oldMouseState.LeftButton == ButtonState.Pressed)
            {
                currentSelectionDrag = (mousePos - Camera.HalfSize) * (1f / cam.Zoom) + cam.CamPos;
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
            //currentRightUse = RightButtonUse.ControlUnits;
        }

        void StartSelectionDrag(Camera cam)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == oldMouseState.LeftButton && !selecting)
            {
                startSelectionDrag = Vector2.Zero;
                currentSelectionDrag = Vector2.Zero;
                
                selecting = true;
                startSelectionDrag = (mousePos - Camera.HalfSize) * (1f / cam.Zoom) + cam.CamPos;

            }
        }
        public List<Unit> SelectionDrag(Camera cam)
        {
            List<Unit> selectedUnits = new List<Unit>();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                currentSelectionDrag = (mousePos - Camera.HalfSize) * (1f / cam.Zoom) + cam.CamPos;
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
            return selectedUnits;
        }
        List<Unit> EndSelectionDrag(Camera cam)
        {
            List<Unit> selectedUnits = new List<Unit>();
            if (mouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                currentSelectionDrag = (mousePos - Camera.HalfSize) * (1f / cam.Zoom) + cam.CamPos;
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

                if (selectedUnits.Count < 1)
                {
                    Unit unit = gameWorld.units.Find(x => x.WorldBounds.Contains(currentSelectionDrag));

                    selectedUnits.Add(unit);
                }
                gameWorld.selectedUnits = selectedUnits;
                startSelectionDrag = Vector2.Zero;
                currentSelectionDrag = Vector2.Zero;
                selecting = false;
            }
            return selectedUnits;
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

            Vector2 camMove = (camMoveAW + camMoveDS) * (float)gameTime.ElapsedGameTime.TotalSeconds * camMoveSpeed * (1f / cam.Zoom);
            cam.CamPos += camMove;

            cam.CalcFullMatrix();
        }
    }
}
