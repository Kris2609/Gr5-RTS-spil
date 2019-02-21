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
        internal Mine CurrMine { get => currMine; set => currMine = value; }

        public float MinePerSec;
        public float currCarry;
        public float carryCap;

        bool changedState;


        public Worker(GameWorld currentGame, Vector2 pos, int animationFPS, WorkerJob job) : base("Worker", currentGame, pos, animationFPS)
        {
            this.job = job;
            this.MinePerSec = 10;
            this.carryCap = 50;
            spriteSheet = currentGame.Content.Load<Texture2D>("spritesheet_Walk_Mine");
        }
        
        public override void DrawAnimated(GameTime gameTime, SpriteBatch spriteBatch, int spriteIndex)
        {
            base.DrawAnimated(gameTime, spriteBatch, (int)job);
        }

        public override void Update(GameTime gameTime)
        {
            if (currMine == null || currMine.GoldLeft >=0)
            {
                currMine = (Mine)currentGame.buildings.Find(x => x.GetType() == typeof(Mine) && ((Mine)x).GoldLeft > 0);
                
            }
            else
            {
                job = WorkerJob.Idle;
            }
            changedState = (oldState == state);

            if (currCarry < carryCap)
            {
                if (canReach(currMine))
                {
                    if (!Mine(currMine, gameTime))
                    {
                        state = WorkerState.Mining;
                        //throw new Exception();
                    }
                    else
                    {
                        job = WorkerJob.Idle;
                        state = WorkerState.Idling;
                        //throw new Exception();
                    }
                }
                else
                {

                    Move(currMine.Pos, gameTime);
                    state = WorkerState.Walking;
                    //throw new Exception();
                }
            }
            else
            {
                if (canReach(currentGame.buildings[0]))
                {
                    Deposit((TownHall)currentGame.buildings[0]);
                    state = WorkerState.Idling;
                    //throw new Exception();
                }
                else
                {

                    Move(currentGame.buildings[0].Pos, gameTime);
                    state = WorkerState.Idling;
                    //throw new Exception();
                }
            }
            oldState = state;
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
