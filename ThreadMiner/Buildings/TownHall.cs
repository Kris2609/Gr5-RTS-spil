using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ThreadMiner
{
    class TownHall : Building
    {
        float currGold;

        public TownHall(GameWorld currentGame, Vector2 pos, string spriteName) : base(500, 0, currentGame, pos, spriteName)
        {
            
        }

        public float Health
        {
            get { return health; }
            set { Health = value; }
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
