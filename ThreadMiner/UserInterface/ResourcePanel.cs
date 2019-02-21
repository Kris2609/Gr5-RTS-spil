﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMiner
{
    class ResourcePanel : UIElement
    {
        SpriteFont spriteFont;
        private string text;
        float displayGold;

        Vector2 size;

        public ResourcePanel(GameWorld currentGame, Vector2 pos, string text, Vector2 size) : base(currentGame, pos)
        {
            this.size = size;
            this.text = text;
            base.sprite = currentGame.Content.Load<Texture2D>("Panel");
            spriteFont = currentGame.Content.Load<SpriteFont>("Default");
        }

        public override void Update(GameTime gameTime)
        {
            displayGold = currentGame.townHall.CurrGold;
        }

        public override Rectangle DestinationRectangle
        {
            get => new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
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
            spriteBatch.DrawString(
                spriteFont,
                text + displayGold.ToString("N0"),
                new Vector2(DestinationRectangle.Center.X, DestinationRectangle.Y + 30),
                Color.DarkKhaki,
                0,
                spriteFont.MeasureString(text + displayGold.ToString("N0")) / 2,
                1,
                SpriteEffects.None,
                0.1f);
        }
    }
}
