using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMiner
{
    public class Background 
    {
        Texture2D backgroundSprite;
        Rectangle background;

        public Background(GameWorld currentGame)
        {
            this.backgroundSprite = currentGame.Content.Load<Texture2D>("Background");
            this.background = new Rectangle(-backgroundSprite.Width * 5, -backgroundSprite.Width * 5, backgroundSprite.Width, backgroundSprite.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                backgroundSprite,                                 //texture
                new Rectangle(background.Location, (background.Size.ToVector2() * 10).ToPoint()),                   //destinationRectangle (pos, size)
                new Rectangle(background.Location, (background.Size.ToVector2() * 10).ToPoint()),     //sourceRectangle
                Color.White,                            //color
                0f,                                     //rotation
                Vector2.Zero,                           //pivot (half tex-size for middle pivot)
                SpriteEffects.None,                     //effects
                0);                                     //layerDepth
        }
    }
}
