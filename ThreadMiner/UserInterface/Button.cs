using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThreadMiner
{
    class Button : UIElement
    {
        private Texture2D pressedSprite;
        private Texture2D currSprite;

        public Button(GameWorld currentGame, Vector2 pos) : base(currentGame, pos)
        {
            base.sprite = currentGame.Content.Load<Texture2D>("Button_Open");
            pressedSprite = currentGame.Content.Load<Texture2D>("Button_Pressed");
        }

        public override void Update(GameTime gameTime)
        {
            if (ScreenBounds.Contains(currentGame.inputManager.mouseState.Position))
            {
                currSprite = sprite;
                col = Color.White;
                if (currentGame.inputManager.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    currSprite = pressedSprite;
                    col = Color.Gray;
                }
            }
            else
            {
                currSprite = sprite;
                col = Color.LightGray;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                    currSprite,                                 //texture
                    DestinationRectangle,                   //destinationRectangle (pos, size)
                    null,                                   //sourceRectangle
                    col,                                    //color
                    0f,                                     //rotation
                    Vector2.Zero,                           //pivot (half tex-size for middle pivot)
                    SpriteEffects.None,                     //effects
                    0);                                     //layerDepth
        }
    }
}
