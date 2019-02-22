using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThreadMiner
{
    class Wolf : Enemy
    {
        float sightRange = 400;

        public enum WolfJob { Mine, WDown, WLeft, WRight, WUp, Idle }
        public WolfJob job;

        public Wolf(string name, GameWorld currentGame, Vector2 pos, int animationFPS) : base(name, currentGame, pos, animationFPS)
        {
            spriteSheet = currentGame.Content.Load<Texture2D>("spritesheet_Warrior");
        }
        

        bool canSee()
        {
            foreach (Unit unit in currentGame.units)
            {
                if (Vector2.Distance(unit.Pos, pos)<sightRange)
                {
                    target = unit;
                    return false;
                }
            }
            return false;
        }

        
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
        public bool Move(Vector2 target, GameTime gameTime)
        {
            Vector2 moveVec = CalcTowards(target, gameTime);
            double angle = Math.Atan2(moveVec.Y, moveVec.X);
            if ((target - pos).Length() < 1)
            {
                job = WolfJob.Idle;
                return true;
            }
            else
            {

                moveVec = MoveTowards(target, gameTime);

                if (Math.Abs(angle) >= (Math.PI * (3d / 4d)))
                {
                    job = WolfJob.WLeft;
                }
                else if (Math.Abs(angle) <= Math.PI / 4)
                {
                    job = WolfJob.WRight;
                }
                else if (angle <= 0)
                {
                    job = WolfJob.WUp;
                }
                else if (angle >= 0)
                {
                    job = WolfJob.WDown;
                }
                return false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (canSee())
            {

            }
        }
    }
}
