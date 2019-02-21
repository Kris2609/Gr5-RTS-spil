using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMiner.Tools
{
    class Background 
    {
        GraphicsDeviceManager graphics;
        Texture2D backgroundSprite;
        SpriteFont spriteFont;
        private string text;

        public Background(GameWorld currentGame, Vector2 pos)
        {
            backgroundSprite = currentGame.Content.Load<Texture2D>("Background");
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            //GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            //GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
        }
    }
}
