using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMiner
{
    public abstract class Unit
    {
        protected float health;
        protected string name;
        protected float damage;
        public static readonly int cost = 90;
        
        public float Health { get => health;
            set
            {
                if (value <= 0)
                {
                    currentGame.units.Remove(this);
                }
                health = value;
            }
        }

        protected Unit(string name, GameWorld currentGame, Vector2 pos, int animationFPS)
        {
            this.name = name;
            this.health = 20;
            this.damage = 2;

            this.movementSpeed = 100;
            this.currentGame = currentGame;
            this.pos = pos;
            this.animationFPS = animationFPS;
        }


        protected GameWorld currentGame;
        protected Vector2 pos;
        public Vector2 Pos { get => pos; }

        protected float movementSpeed;
        


        public abstract void Update(GameTime gameTime);

        protected Texture2D spriteSheet;
        protected float animationIndex;
        protected int animationFPS;

        /// <summary>
        /// Should not be used directly as movement alongside animation is handled in the Move() method.
        /// </summary>
        /// <param name="target">The position to move towards.</param>
        /// <param name="gameTime">The GameTime, used to get the elapsed time.</param>
        /// <returns></returns>
        public virtual Vector2 MoveTowards(Vector2 target, GameTime gameTime)
        {
            Vector2 moveVec = target - pos;
            if (moveVec.LengthSquared() > 1)
            {
                moveVec.Normalize();
            }
            moveVec *= (float)gameTime.ElapsedGameTime.TotalSeconds * movementSpeed;
            pos += moveVec;
            return moveVec;
        }
        /// <summary>
        /// Returns the Vector that would be moved by MoveTowards without actually moving the Unit.
        /// </summary>
        /// <param name="target">The position to move towards.</param>
        /// <param name="gameTime">The GameTime, used to get the elapsed time.</param>
        /// <returns></returns>
        public virtual Vector2 CalcTowards(Vector2 target, GameTime gameTime)
        {
            Vector2 moveVec = target - pos;
            if (moveVec.LengthSquared() > 1)
            {
                moveVec.Normalize();
            }
            moveVec *= (float)gameTime.ElapsedGameTime.TotalSeconds * movementSpeed;
            return moveVec;
        }
        /// <summary>
        /// Used for graphics alongside textures pivot point.
        /// </summary>
        public Rectangle DestinationRectangle
        {
            get => new Rectangle((int)pos.X, (int)pos.Y, 64, 64);
        }

        /// <summary>
        /// Gets the actual corner-positions of the texture in the world.
        /// </summary>
        public Rectangle WorldBounds
        {
            get => new Rectangle((int)pos.X-32, (int)pos.Y-32, 64, 64);
        }
        /// <summary>
        /// The base Draw method. Should be overloaded by a Move method to handle animations via the spriteSheetRow.
        /// </summary>
        /// <param name="gameTime">The GameTime, used to get the elapsed time.</param>
        /// <param name="spriteBatch"> The spriteBatch that should render this unit.</param>
        /// <param name="spriteSheetRow"> The row of the spritesheet to be rendered.</param>
        public virtual void DrawAnimated(GameTime gameTime, SpriteBatch spriteBatch, int spriteSheetRow = 0)
        {
            int frameCount = spriteSheet.Width / 64;
            animationIndex += ((float)gameTime.ElapsedGameTime.TotalSeconds * animationFPS);
            animationIndex = (animationIndex % frameCount);
            Rectangle frameRect = new Rectangle((spriteSheet.Width / frameCount) * (int)animationIndex, 64 * spriteSheetRow, 64, 64);
            //Rectangle temp = new Rectangle(0, 0, 64, 64);

            spriteBatch.Draw(
                    spriteSheet,                                    //texture
                    DestinationRectangle,                           //destinationRectangle (pos, size)
                    frameRect,                                      //sourceRectangle
                    Color.White,                                    //color
                    0f,                                             //rotation
                    Vector2.One * 32,                               //pivot (half tex-size for middle pivot)
                    SpriteEffects.None,                             //effects
                    (pos.Y+5000f )/ 10000f);                                          //layerDepth
        }
    }
}

