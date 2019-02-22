using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMiner
{
    public abstract class Enemy
    {
        protected float health;
        public float Health { get => health;
            set
            {
                if (value <= 0)
                {
                    currentGame.enemies.Remove(this);
                    EnemySpawner.currEnemies--;
                }
                health = value;
            }
        }
        protected float damage;
        protected int movementSpeed;
        protected GameWorld currentGame;
        protected Vector2 pos;
        protected int animationFPS;

        protected List<Enemy> enemyGroup;
        protected Unit target;
        protected Texture2D spriteSheet;
        protected float animationIndex;

        protected Enemy(string name, GameWorld currentGame, Vector2 pos, int animationFPS)
        {
            this.health = 20;
            this.damage = 2;

            this.movementSpeed = 100;
            this.currentGame = currentGame;
            this.pos = pos;
            this.animationFPS = animationFPS;
        }

        public abstract void Update(GameTime gameTime);

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

        public virtual void DrawAnimated(GameTime gameTime, SpriteBatch spriteBatch, int spriteSheetRow = 0)
        {
            int frameCount = spriteSheet.Width / 128;
            animationIndex += ((float)gameTime.ElapsedGameTime.TotalSeconds * animationFPS);
            animationIndex = (animationIndex % frameCount);
            Rectangle frameRect = new Rectangle((spriteSheet.Width / frameCount) * (int)animationIndex, 128 * spriteSheetRow, 128, 128);
            //Rectangle temp = new Rectangle(0, 0, 64, 64);

            spriteBatch.Draw(
                    spriteSheet,                                    //texture
                    DestinationRectangle,                           //destinationRectangle (pos, size)
                    frameRect,                                      //sourceRectangle
                    Color.White,                                    //color
                    0f,                                             //rotation
                    Vector2.One * 64,                               //pivot (half tex-size for middle pivot)
                    SpriteEffects.None,                             //effects
                    (pos.Y + 5000f) / 10000f);                                          //layerDepth
        }

        /// <summary>
        /// Used for graphics alongside textures pivot point.
        /// </summary>
        public Rectangle DestinationRectangle
        {
            get => new Rectangle((int)pos.X, (int)pos.Y, 128, 128);
        }

        /// <summary>
        /// Gets the actual corner-positions of the texture in the world.
        /// </summary>
        public Rectangle WorldBounds
        {
            get => new Rectangle((int)pos.X - 64, (int)pos.Y - 64, 128, 128);
        }
        public Vector2 Pos { get => pos; set => pos = value; }
    }
}
