using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ThreadMiner
{
    class Mine : Building
    {
        private float gold;
        private float worktime;
        private static int workersInMine = 5;
        static object thislock;
        public float GoldLeft { get => gold;}

        public Mine(GameWorld currentGame, Vector2 pos, string spriteName) : base(100, 100, currentGame, pos, spriteName)
        {
            this.gold = 10000;
        }

        public void MineGold()
        {
            this.worktime = 5;
            Thread mWorker = new Thread(WorkingInMine);
            for (int i = 0; workersInMine > 5; i++)
            {
                mWorker.Start();
            }
            mWorker.IsBackground = true;
        }

        public static void WorkingInMine()
        {
               
            while (workersInMine > 5)
            {
                Monitor.Enter(thislock);
                try
                { workersInMine++; }
                finally
                { Monitor.Exit(thislock);}
                
                
            }
            
        }


        public override void Update(GameTime gameTime)
        {
        }
        
        public float MineGold(float amount)
        {
            float tmp = CalcGold(amount);
            gold -= tmp;
            return tmp;
        }
        float CalcGold(float amount)
        {
            return amount < GoldLeft ? amount : GoldLeft;
        }
    }
}
