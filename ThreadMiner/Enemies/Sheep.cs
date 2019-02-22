using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ThreadMiner
{
    class Sheep : Enemy
    {
        public Sheep(string name, GameWorld currentGame, Vector2 pos, int animationFPS) : base(name, currentGame, pos, animationFPS)
        {
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
