using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMiner
{
    public static class GraphicInfo
    {
        static Rectangle screenBounds;
        static int screenWidth;
        static int screenHeight;
        public static Vector2 HalfSize
        {
            get => (new Vector2(screenWidth / 2, screenHeight / 2));
        }

        public static Rectangle ScreenBounds { get => screenBounds; set => screenBounds = value; }
        public static int ScreenHeight { get => screenHeight; set => screenHeight = value; }
        public static int ScreenWidth { get => screenWidth; set => screenWidth = value; }
    }
}
