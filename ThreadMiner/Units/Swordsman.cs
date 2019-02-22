using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMiner
{
    class Swordsman : Unit
    {
        public enum WarriorJob { Fight, WDown, WLeft, WRight, WUp, Idle }
        public WarriorJob job;
        public enum WarriorState { Walking, Idling, Fighting }
        public WarriorState state;

        public Building targetBuilding;
        public Enemy targetEnemy;

        public Swordsman(GameWorld currentGame, Vector2 pos, int animationFPS, WarriorJob job) : base("Warrior", currentGame, pos, animationFPS)
        {
            this.job = job;
            spriteSheet = currentGame.Content.Load<Texture2D>("spritesheet_Warrior");
        }

        public override void DrawAnimated(GameTime gameTime, SpriteBatch spriteBatch, int spriteIndex)
        {
            base.DrawAnimated(gameTime, spriteBatch, (int)job);
        }

        public override void Update(GameTime gameTime)
        {
            canSee();
            if (targetEnemy != null)
            {
                if (canReach(targetEnemy))
                {
                    Attack(gameTime);
                    if (targetEnemy.Health <= 0)
                    {
                        targetEnemy = null;
                    }
                }
                else
                {
                    Move(targetEnemy.Pos, gameTime);
                }
            }
            else
            {
                if (canReach(targetBuilding))
                {
                    targetBuilding = currentGame.buildings[new Random().Next(0, currentGame.buildings.Count)];

                }
                else
                {
                    Move(targetBuilding.Pos, gameTime);
                }
            }
        }
        bool canSee()
        {
            foreach (Enemy enemy in currentGame.enemies)
            {
                if (Vector2.Distance(enemy.Pos, pos) < 950)
                {
                    if (targetEnemy == null || targetEnemy.Health <= 0 && enemy != null || enemy.Health > 0)
                    {
                        targetEnemy = enemy;
                    }
                    return false;
                }
            }
            return false;
        }

        private void Attack(GameTime gameTime)
        {
            job = WarriorJob.Fight;
            targetEnemy.Health -= damage * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private bool canReach(Building building)
        {
            return Vector2.Distance(pos, building.Pos) <= building.DestinationRectangle.Width / 4;
        }
        private bool canReach(Enemy enemy)
        {
            return Vector2.Distance(pos, enemy.Pos) <= enemy.DestinationRectangle.Width / 4;
        }
        public bool Move(Vector2 target, GameTime gameTime)
        {
            Vector2 moveVec = CalcTowards(target, gameTime);
            double angle = Math.Atan2(moveVec.Y, moveVec.X);
            if ((target - pos).Length() < 1)
            {
                job = WarriorJob.Idle;
                return true;
            }
            else
            {

                moveVec = MoveTowards(target, gameTime);

                if (Math.Abs(angle) >= (Math.PI * (3d / 4d)))
                {
                    job = WarriorJob.WLeft;
                }
                else if (Math.Abs(angle) <= Math.PI / 4)
                {
                    job = WarriorJob.WRight;
                }
                else if (angle <= 0)
                {
                    job = WarriorJob.WUp;
                }
                else if (angle >= 0)
                {
                    job = WarriorJob.WDown;
                }
                return false;
            }
        }
    }
}
