using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ThreadMiner
{
    public class TownHall : Building
    {
        float currGold;

        public float CurrGold { get => currGold; set => currGold = value; }

        public TownHall(GameWorld currentGame, Vector2 pos, string spriteName) : base(500, 0, currentGame, pos, spriteName)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
        public float DepositGold(float amount)
        {
            currGold += amount;
            return amount;
        }
    }
}
