using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMiner
{
    public abstract class UIElement
    {
        protected GameWorld currentGame;
        protected Vector2 pos;
        protected Texture2D sprite;

        protected UIElement(GameWorld currentGame, Vector2 pos)
        {
            this.currentGame = currentGame;
            this.pos = pos;
        }

        public Vector2 Pos { get => pos; }

        public Rectangle DestinationRectangle
        {
            get => new Rectangle((int)pos.X, (int)pos.Y, sprite.Bounds.Width, sprite.Bounds.Height);
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                    sprite,                                 //texture
                    DestinationRectangle,                   //destinationRectangle (pos, size)
                    null,                                   //sourceRectangle
                    Color.White,                            //color
                    0f,                                     //rotation
                    Vector2.One * sprite.Bounds.Width / 2,  //pivot (half tex-size for middle pivot)
                    SpriteEffects.None,                     //effects
                    0);                                     //layerDepth
        }
    }
}
