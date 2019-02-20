using Microsoft.Xna.Framework;
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
    }
}
