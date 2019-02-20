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
        private int gold;
        private float worktime;
        private static int workersInMine = 5;

        public Mine(int health, int cost, GameWorld currentGame, Vector2 pos, string spriteName) : base(100, 100, currentGame, pos, spriteName)
        {

        }

        public void MineGold()
        {
            this.gold = 10000;
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
                workersInMine++;
                
            }
            
        }


        public override void Update(GameTime gameTime)
        {
        }
    }
}
