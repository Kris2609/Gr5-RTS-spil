using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public bool Selected = false;

        bool changedState;

        Thread t;

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

        public delegate void MineOnThread(Mine m);

        public override void Update(GameTime gameTime)
        {
            if (currMine == null || currMine.GoldLeft >=0)
            {
                if (currCarry < carryCap&& currMine != null)
                {
                    if (canReach(currMine))
                    {
                        if (t == null || !t.IsAlive)
                        {
                            t = new Thread(delegate () { Mine(currMine); });
                            if (t != null)
                            {
                                t.Start();
                                t.IsBackground = true;
                            }
                            else
                            {
                                currMine = null;
                            }
                            state = WorkerState.Idling;
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
                        if (t == null || !t.IsAlive)
                        {
                            t = new Thread(delegate () { Deposit(currentGame.townHall); });
                            t.Start();
                            t.IsBackground = true;
                            state = WorkerState.Idling;
                        }
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
            }
            else
            {
                job = WorkerJob.Idle;
            }
            changedState = (oldState == state);


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

        public void Mine(Mine mine)
        {
            job = WorkerJob.Mine;
            float carryWeightLeft = (carryCap - currCarry);
            float tmp = MinePerSec * 0.005f;
            float mineAmount = carryWeightLeft < tmp ? carryWeightLeft : tmp;
            if (!currMine.accessLimit.WaitOne(50))
            {
                currMine = null;
                t.Abort();
            }
            while (!(currCarry >= carryCap || mine.GoldLeft <= 0))
            {
                lock (mine)
                {
                    //throw new Exception();
                    currCarry += mine.MineGold(mineAmount);
                }
                Thread.Sleep(5);
            }
            mine.accessLimit.Release();
        }

        public void Deposit(TownHall hall)
        {
            job = WorkerJob.Idle;
            hall.accesLimiter.WaitOne();
            currCarry -= hall.DepositGold(currCarry);
            Thread.Sleep(500);
            hall.accesLimiter.ReleaseMutex();
        }
    }
}
