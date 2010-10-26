using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game
{
    public class DrawMatrix
    {
        public static DrawMatrix IDENTITY = new DrawMatrix();

        public Vector2 POSITION;
        public Vector2 ORIGIN;
        public Vector2 SCALE;
        public float ROTATION;        

        public DrawMatrix()
        {
            identity();
        }

        public void identity()
        {
            POSITION.X = POSITION.Y = 0.0f;
            ORIGIN.X = ORIGIN.Y = 0.0f;
            SCALE.X = SCALE.Y = 1.0f;
            ROTATION = 0;            
        }

        public float tx
        {            
            set { ORIGIN.X = -value; }
        }

        public float ty
        {           
            set { ORIGIN.Y = -value; }
        }

        public void scale(float sx, float sy)
        {
            SCALE.X = sx;
            SCALE.Y = sy;
        }

        public void translate(float tx, float ty)
        {
            POSITION.X = tx;
            POSITION.Y = ty;
        }

        public void rotate(float angle)
        {
            ROTATION = angle;
        }
    }
}
