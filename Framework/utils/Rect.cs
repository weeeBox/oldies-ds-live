using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.utils
{
    public struct Rect : IEquatable<Rect>
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;        

        public Rect(float x, float y, float w, float h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }

        #region IEquatable<Rect> Members

        public bool Equals(Rect other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height; 
        }

        #endregion
    }
}
