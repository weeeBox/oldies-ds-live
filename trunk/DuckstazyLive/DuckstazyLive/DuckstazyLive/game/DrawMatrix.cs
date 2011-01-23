using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game
{
    public class DrawMatrix
    {
        public static DrawMatrix IDENTITY = new DrawMatrix(false);

        public Vector2 POSITION;
        public Vector2 ORIGIN;
        public Vector2 SCALE;
        public float ROTATION;
        public Vector2 FLIP;
        private bool useScale;

        private static DrawMatrix instance = new DrawMatrix(false);
        private static DrawMatrix scaledInstance = new DrawMatrix(true);

        public static DrawMatrix Instance
        {
            get 
            {
                instance.identity();
                return instance; 
            }
        }

        public static DrawMatrix ScaledInstance
        {
            get 
            {
                scaledInstance.identity();
                return scaledInstance; 
            }
        }

        private DrawMatrix(bool useScale)
        {
            this.useScale = useScale;
            identity();        
        }

        public void identity()
        {            
            POSITION.X = POSITION.Y = 0.0f;
            ORIGIN.X = ORIGIN.Y = 0.0f;
            SCALE.X = SCALE.Y = 1.0f;
            ROTATION = 0;
            FLIP.X = FLIP.Y = 0.0f;            
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

        public void flip(bool x, bool y)
        {
            FLIP.X = x ? 1 : 0;
            FLIP.Y = y ? 1 : 0;
        }

        public bool UseScale
        {
            get { return useScale; }
        }
    }
}
