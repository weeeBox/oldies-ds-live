using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class ColorTransform
    {
        public static ColorTransform NONE = new ColorTransform();

        public float alphaMultiplier;
        public float redMultiplier;
        public float greenMultiplier;
        public float blueMultiplier;

        public ColorTransform() : this (1.0f, 1.0f, 1.0f, 1.0f)
        {            
        }

        public ColorTransform(uint color) : this (((color >> 16) & 0xff) / 255.0f, ((color >> 8) & 0xff) / 255.0f, (color & 0xff) / 255.0f, 1.0f)
        {

        }

        public ColorTransform(float redMultiplier, float greenMultiplier, float blueMultiplier) : this(redMultiplier, greenMultiplier, blueMultiplier, 1.0f)
        {            
        }

        public ColorTransform(float redMultiplier, float greenMultiplier, float blueMultiplier, float alphaMultiplier)
        {
            this.redMultiplier = redMultiplier;
            this.greenMultiplier = greenMultiplier;
            this.blueMultiplier = blueMultiplier;
            this.alphaMultiplier = alphaMultiplier;
        }
    }
}
