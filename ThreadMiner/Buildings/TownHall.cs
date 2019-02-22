using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ThreadMiner
{
    public class TownHall : Building
    {
        float currGold;
        int unitCap = 10;
        public int UnitCap { get => unitCap; set => unitCap = value; }

        public Mutex accesLimiter;

        public float CurrGold { get => currGold; set => currGold = value; }

        public TownHall(GameWorld currentGame, Vector2 pos, string spriteName) : base(500, currentGame, pos, spriteName)
        {
            this.currGold = 9999;
            accesLimiter = new Mutex();
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
