﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThreadMiner
{
    class HousePanel : UIElement
    {
        Texture2D houseIconSprite;
        SpriteFont spriteFont;
        private string text;

        public HousePanel(GameWorld currentGame, Vector2 pos, string text) : base(currentGame, pos)
        {
            this.text = text;
            base.sprite = currentGame.Content.Load<Texture2D>("Panel");
            houseIconSprite = currentGame.Content.Load<Texture2D>("WorkerIcon");
            spriteFont = currentGame.Content.Load<SpriteFont>("Default");
        }

        public override void Update(GameTime gameTime)
        {

            if (ScreenBounds.Contains(currentGame.inputManager.mouseState.Position))
            {
                col = Color.White;
                if (currentGame.inputManager.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    col = Color.Gray;
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

        }
    }
}
