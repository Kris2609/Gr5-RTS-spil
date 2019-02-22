using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ThreadMiner
{
    class HousePanel : UIElement
    {
        Texture2D houseIconSprite;
        Texture2D goldIconSprite;
        SpriteFont spriteFont;
        private string text;

        public HousePanel(GameWorld currentGame, Vector2 pos, string text) : base(currentGame, pos)
        {
            this.text = text;
            base.sprite = currentGame.Content.Load<Texture2D>("Panel");
            houseIconSprite = currentGame.Content.Load<Texture2D>("WorkerIcon");
            goldIconSprite = currentGame.Content.Load<Texture2D>("GoldIcon");
            spriteFont = currentGame.Content.Load<SpriteFont>("Default");
        }

        public override void Update(GameTime gameTime)
        {

            if (ScreenBounds.Contains(currentGame.inputManager.mouseState.Position))
            {
                col = Color.White;
                if (currentGame.inputManager.mouseState.LeftButton == ButtonState.Pressed && currentGame.inputManager.oldMouseState.LeftButton == ButtonState.Released)
                {
                    col = Color.Gray;
                    currentGame.inputManager.currentRightUse = InputManager.RightButtonUse.PlaceHouse;
                }
            }
            else
            {
                col = Color.LightGray;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                    sprite,                                 //texture
                    DestinationRectangle,                   //destinationRectangle (pos, size)
                    null,                                   //sourceRectangle
                    col,                                    //color
                    0f,                                     //rotation
                    Vector2.Zero,                           //pivot (half tex-size for middle pivot)
                    SpriteEffects.None,                     //effects
                    0);                                     //layerDepth
                                                            //

            spriteBatch.Draw(
                    houseIconSprite,                                 //texture
                    new Rectangle(DestinationRectangle.Center.X, DestinationRectangle.Center.Y + 10, houseIconSprite.Width, houseIconSprite.Height),                  //destinationRectangle (pos, size)
                    null,                                   //sourceRectangle
                    col,                                    //color
                    0f,                                     //rotation
                    new Vector2(houseIconSprite.Width / 2, houseIconSprite.Height / 2),                           //pivot (half tex-size for middle pivot)
                    SpriteEffects.None,                     //effects
                    0);                                     //layerDepth
                                                            //
            spriteBatch.DrawString(
                spriteFont,
                text,
                new Vector2(DestinationRectangle.Center.X, DestinationRectangle.Y + 25),
                Color.DarkKhaki,
                0,
                spriteFont.MeasureString(text) / 2,
                1,
                SpriteEffects.None,
                0.1f);

            if (ScreenBounds.Contains(currentGame.inputManager.mouseState.Position))
            {
                spriteBatch.Draw(
                        sprite,                                 //texture
                        CostRectangle,                   //destinationRectangle (pos, size)
                        null,                                   //sourceRectangle
                        col,                                    //color
                        0f,                                     //rotation
                        Vector2.Zero,                           //pivot (half tex-size for middle pivot)
                        SpriteEffects.None,                     //effects
                        0);                                     //layerDepth
                                                                //
                spriteBatch.Draw(
                        goldIconSprite,                                 //texture
                        new Rectangle(CostRectangle.X + 25, CostRectangle.Center.Y, (int)(goldIconSprite.Width / 2f), (int)(goldIconSprite.Height / 2f)),                  //destinationRectangle (pos, size)
                        null,                                   //sourceRectangle
                        col,                                    //color
                        0f,                                     //rotation
                        new Vector2(goldIconSprite.Width / 2, goldIconSprite.Height / 2),                           //pivot (half tex-size for middle pivot)
                        SpriteEffects.None,                     //effects
                        0);                                     //layerDepth
                                                                //

                spriteBatch.DrawString(
                        spriteFont,
                        House.cost.ToString("N0"),
                        new Vector2(CostRectangle.X + CostRectangle.Width - 20, CostRectangle.Center.Y),
                        Color.DarkKhaki,
                        0,
                        spriteFont.MeasureString(text) / 2,
                        1,
                        SpriteEffects.None,
                        0.1f);
            }

        }
    }
}
