using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMiner
{
    class PopulationPanel : UIElement
    {
        Texture2D unitIconSprite;
        SpriteFont spriteFont;
        private string text;
        int displayPopulation;
        int displayUnitCap;

        Vector2 size;

        public PopulationPanel(GameWorld currentGame, Vector2 pos, string text, Vector2 size) : base(currentGame, pos)
        {
            this.size = size;
            this.text = text;
            base.sprite = currentGame.Content.Load<Texture2D>("Panel");
            spriteFont = currentGame.Content.Load<SpriteFont>("Default");
            unitIconSprite = currentGame.Content.Load<Texture2D>("UnitIcon");
        }

        public override void Update(GameTime gameTime)
        {
            displayUnitCap = currentGame.townHall.UnitCap;
            displayPopulation = currentGame.units.Count;

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

            spriteBatch.Draw(
                    unitIconSprite,                                 //texture
                    new Rectangle(DestinationRectangle.X + 50, DestinationRectangle.Center.Y, (int)(unitIconSprite.Width / 1.2f), (int)(unitIconSprite.Height / 1.2f)),                  //destinationRectangle (pos, size)
                    null,                                   //sourceRectangle
                    col,                                    //color
                    0f,                                     //rotation
                    new Vector2(unitIconSprite.Width/2, unitIconSprite.Height/2),                           //pivot (half tex-size for middle pivot)
                    SpriteEffects.None,                     //effects
                    0);                                     //layerDepth
                                                            //
            spriteBatch.DrawString(
                spriteFont,
                displayPopulation.ToString("N0") + text + displayUnitCap.ToString("N0"),
                new Vector2(DestinationRectangle.X + DestinationRectangle.Width - 30, DestinationRectangle.Y + 30),
                Color.DarkKhaki,
                0,
                spriteFont.MeasureString(displayPopulation.ToString("N0") + text + displayUnitCap.ToString("N0")) * new Vector2(1,0.5f),
                1,
                SpriteEffects.None,
                0.1f);


        }
    }
}
