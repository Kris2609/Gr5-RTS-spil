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
        protected Color col = Color.White;

        protected UIElement(GameWorld currentGame, Vector2 pos)
        {
            this.currentGame = currentGame;
            this.pos = pos;
        }

        public Vector2 Pos { get => pos; }

        /// <summary>
        /// Used for graphics alongside textures pivot point.
        /// </summary>
        public virtual Rectangle DestinationRectangle
        {
            get => new Rectangle((int)pos.X, (int)pos.Y, sprite.Bounds.Width, sprite.Bounds.Height);
        }

        /// <summary>
        /// Gets the actual corner positions of the texture on the screen.
        /// </summary>
        public Rectangle ScreenBounds
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
                    col,                                    //color
                    0f,                                     //rotation
                    Vector2.Zero,                           //pivot (half tex-size for middle pivot)
                    SpriteEffects.None,                     //effects
                    0);                                     //layerDepth
        }
    }
}
