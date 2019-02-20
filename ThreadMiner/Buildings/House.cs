using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ThreadMiner
{
    class House : Building
    {
        public House(GameWorld currentGame, Vector2 pos, string spriteName) : base(120, 80, currentGame, pos, spriteName)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
