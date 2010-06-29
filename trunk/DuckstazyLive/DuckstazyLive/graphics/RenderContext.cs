using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.graphics
{
    public sealed class RenderContext
    {        
        private SpriteBatch spriteBatch;
        private BasicEffect basicEffect;

        public RenderContext(SpriteBatch spriteBatch, BasicEffect basicEffect)
        {            
            this.spriteBatch = spriteBatch;
            this.basicEffect = basicEffect;
        }              

        public BasicEffect BasicEffect
        {
            get { return basicEffect; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }
    }
}
