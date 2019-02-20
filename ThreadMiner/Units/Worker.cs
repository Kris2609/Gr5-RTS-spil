using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ThreadMiner
{
    class Worker:Unit
    {
        public Worker(GameWorld currentGame, Vector2 pos, int animationFPS) : base("Worker", currentGame, pos, animationFPS)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
