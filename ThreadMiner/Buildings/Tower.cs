using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMiner
{
    class Tower : Building
    {
        public static int cost = 90;//how much the tower cost to build
        protected int damage;//damage the tower does to the enemy
        protected float radius;//how big range the tower has
        
        
        protected Enemy target; //shows whitch enemy to shoot
        

        /// <summary>
        /// stats of the towers
        /// </summary>
        /// <param name="frameCount"></param>
        /// <param name="animationFPS"></param>
        /// <param name="textureName"></param>
        /// <param name="content"></param>
        /// <param name="position"></param>
        /// <param name="bulletTexture"></param>
        
        public Tower(GameWorld currentGame, Vector2 pos, string spriteName) : base(150, currentGame, pos, spriteName)
        {
            this.radius = 200;
            this.damage = 5;
        }

        /// <summary>
        /// allows the tower to have a target 
        /// </summary>
        public Enemy Target
        {
            get { return target; }
        }
        
        
        /// <summary>
        /// the tower cost cookies to build
        /// </summary>
        public int Cost
        {
            get { return cost; }
        }
        /// <summary>
        /// allows the tower to do some damage
        /// </summary>
        public int Damage
        {
            get { return damage; }
        }
        /// <summary>
        /// the radius of the towers
        /// </summary>
        public float Radius
        {
            get { return radius; }
        }

        public override void Update(GameTime gameTime)
        {
        }
    }

    
}
