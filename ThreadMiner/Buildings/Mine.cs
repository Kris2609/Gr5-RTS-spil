using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThreadMiner
{
    class Mine : Building
    {
        SpriteFont font;
        private float gold;
        public float GoldLeft { get => gold;}

        public Semaphore accessLimit;

        public static new int cost = 100;

        public Mine(GameWorld currentGame, Vector2 pos, string spriteName) : base(100, currentGame, pos, spriteName)
        {
            this.gold = 10000;
            accessLimit = new Semaphore(0, 5);
            accessLimit.Release(5);
            font = currentGame.Content.Load<SpriteFont>("Default");
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(font, accessLimit.Release().ToString(), pos, Color.White);
            base.Draw(gameTime, spriteBatch);
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
