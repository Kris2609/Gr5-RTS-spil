using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThreadMiner
{
    class Worker:Unit
    {
        public enum WorkerJob { Mine, WDown, WLeft, WRight, WUp, Idle }
        public WorkerJob job;
        public enum WorkerState { Mining, Walking, Idling, Building }
        public WorkerState state;

        WorkerState oldState;

        Mine currMine;

        public float MinePerSec;
        public float currCarry;
        public float carryCap;

        bool changedState;
        private Vector2 targetPos;

        public Worker(GameWorld currentGame, Vector2 pos, int animationFPS) : base("Worker", currentGame, pos, animationFPS)
        {
        }
        
        public void DrawAnimated(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.DrawAnimated(gameTime, spriteBatch, (int)job);
        }
        public override void Update(GameTime gameTime)
        {
            changedState = (oldState == state);

            if (currCarry < carryCap)
            {
                if (canReach(currMine))
                {
                    if (!Mine(currMine, gameTime))
                    {
                        state = WorkerState.Mining;
                    }
                    else
                    {
                        job = WorkerJob.Idle;
                        state = WorkerState.Idling;
                    }
                }
                else
                {
                    ValidatePos(currMine);

                    Move(targetPos, gameTime);
                    state = WorkerState.Walking;
                }
            }
            else
            {
                if (canReach(currentGame.buildings[0])
                {
                    Deposit((TownHall)currentGame.buildings[0]);
                    state = WorkerState.Idling;
                }
                else
                {
                    ValidatePos(currentGame.buildings[0]);

                    Move(currentGame.buildings[0].Pos, gameTime);
                    state = WorkerState.Mining;
                }
            }
            oldState = state;
        }

        void ValidatePos(Building building)
        {
            if (!building.DestinationRectangle.Contains(targetPos))
            {
                targetPos = building.Pos;
            }
        }

        private bool canReach(Building building)
        {
            return Vector2.Distance(pos, building.Pos) <= building.DestinationRectangle.Width / 4;
        }

        public bool Move(Vector2 target, GameTime gameTime)
        {
            Vector2 moveVec = CalcTowards(target, gameTime);
            double angle = Math.Atan2(moveVec.Y, moveVec.X);
            if ((target - pos).Length() < 1)
            {
                job = WorkerJob.Idle;
                return true;
            }
            else
            {

                moveVec = MoveTowards(target, gameTime);

                if (Math.Abs(angle) >= (Math.PI * (3d / 4d)))
                {
                    job = WorkerJob.WLeft;
                }
                else if (Math.Abs(angle) <= Math.PI / 4)
                {
                    job = WorkerJob.WRight;
                }
                else if (angle <= 0)
                {
                    job = WorkerJob.WUp;
                }
                else if (angle >= 0)
                {
                    job = WorkerJob.WDown;
                }
                return false;
            }
        }

        public bool Mine(Mine mine, GameTime gameTime)
        {
            job = WorkerJob.Mine;
            float carryWeightLeft = (carryCap - currCarry);
            float tmp = MinePerSec * (float)gameTime.ElapsedGameTime.TotalSeconds;
            float mineAmount = carryWeightLeft < tmp ? carryWeightLeft : tmp;
            currCarry += mine.MineGold(mineAmount);
            if (currCarry >= carryCap || mine.GoldLeft <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Deposit(TownHall hall)
        {
            job = WorkerJob.Idle;
            currCarry -= hall.DepositGold(currCarry);
            return true;
        }
    }
}
