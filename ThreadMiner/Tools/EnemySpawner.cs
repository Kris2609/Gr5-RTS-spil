using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadMiner
{
    public class EnemySpawner
    {
        Enemy typeToSpawn;
        GameWorld gameWorld;

        public static int EnemyCap = 5;
        public static int currEnemies = 0;
        public static int WaveID = 0;
        Random rand;

        public EnemySpawner(GameWorld gameWorld)
        {
            this.gameWorld = gameWorld;
            Thread t = new Thread(delegate () { SpawnEnemy(); });
            rand = new Random();
            t.IsBackground = true;
            t.Start();
        }
        public void SpawnEnemy()
        {
            Thread.Sleep(500);
            while (true)
            {
                WaveID++;
                while (currEnemies < EnemyCap)
                {
                    currEnemies++;
                    float tmp = rand.Next();
                    gameWorld.enemies.Add(new Wolf(gameWorld, new Vector2(2560 * (float)Math.Sin(tmp), 2560 * (float)Math.Cos(tmp)), 30));

                }
                if (WaveID %10 ==0)
                {
                    EnemyCap += WaveID/2;
                }
                Thread.Sleep(30000);
            }
        }



    }
}
