using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ThreadMiner
{
    class Barrack : Building
    {
        private int health;
        private int cost;
        private float trainingTime;

        public Barrack(GameWorld currentGame, Vector2 pos, string spriteName) : base(200, 100, currentGame, pos, spriteName)
        {
            this.trainingTime = 10;
        }

        public void trainingUnits()
        {

        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
