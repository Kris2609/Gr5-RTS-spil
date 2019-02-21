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
        public float GoldLeft { get => gold;}

        public Mine(GameWorld currentGame, Vector2 pos, string spriteName) : base(100, 100, currentGame, pos, spriteName)
        {
            this.gold = 10000;
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
