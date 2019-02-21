using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThreadMiner
{
    class Button : UIElement
    {
        public Button(GameWorld currentGame, Vector2 pos) : base(currentGame, pos)
        {
            base.sprite = currentGame.Content.Load<Texture2D>("spritesheet_Walk_Mine");
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
