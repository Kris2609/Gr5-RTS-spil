using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadMiner
{
    public class Camera
    {
        public static float HalfSizeX;
        public static float HalfSizeY;

        public Matrix Transform { get; private set; }
        public Matrix ScreenTransform
        {
            get => Matrix.CreateTranslation
                (
                    HalfSizeX,
                    HalfSizeY,
                    0
                );
        }
        float zoom = 1;
        public float minZoom = 0.5f;
        public float maxZoom = 2f;


        public float Zoom { get => zoom; set { zoom = MathHelper.Clamp(value, minZoom, maxZoom); } }


        public Vector2 camPos;

        public void CalcFullMatrix()
        {
            var position = Matrix.CreateTranslation(
              -camPos.X,
              -camPos.Y,
              0) * GetScaleMatrix(); ;

            var offset = Matrix.CreateTranslation(
                HalfSizeX,
                HalfSizeY,
                0);

            Transform = position * offset;
        }
        public Matrix GetScaleMatrix()
        {
            return Matrix.CreateScale(new Vector3(zoom, zoom, 1));
        }
    }
}
