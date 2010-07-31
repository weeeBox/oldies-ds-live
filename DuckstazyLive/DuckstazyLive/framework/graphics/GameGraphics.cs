using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace DuckstazyLive.framework.graphics
{
    public enum GraphicsMode
    {
        UNDEFINED, // uninitialized graphics mode
        SPRITE_BATCH, // begin drawing sprite batch
        BASIC_EFFECT, // drawing using effect or basic effect
        CUSTOM_EFFECT, // drawing using custom effect
    }

    public class GameGraphics
    {
        private Stack<Matrix> transformationStack;
        private Matrix currentTransform;
        private GraphicsDevice graphicsDevice;
        private GraphicsMode graphicsMode;
        private SpriteBatch spriteBatch;
        private BasicEffect basicEffect;
        private Effect customEffect;

        GameGraphics(GraphicsDevice graphicsDevice)
        {
            transformationStack = new Stack<Matrix>();
            this.graphicsDevice = graphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDevice);
            basicEffect = new BasicEffect(graphicsDevice, null);
            customEffect = null;
        }

        public void PopMatrix()
        {
            currentTransform = transformationStack.Pop();
        }

        public void PushMatrix()
        {
            transformationStack.Push(currentTransform);
        }

        public void AddTransform(Matrix m)
        {
            Matrix.Multiply(ref currentTransform, ref m, out currentTransform);
        }

        public void Translate(float dx, float dy)
        {
            AddTransform(Matrix.CreateTranslation(dx, dy, 0.0f));
        }

        public void Scale(float scaleX, float scaleY)
        {
            AddTransform(Matrix.CreateScale(scaleX, scaleY, 1.0f));
        }

        public void Scale(float scale)
        {
            Scale(scale, scale);
        }        

        public void Begin(GraphicsMode mode)
        {
            if (mode != graphicsMode)
            {
                End();
                if (mode == GraphicsMode.SPRITE_BATCH)
                {
                    spriteBatch.Begin();
                }
                else if (mode == GraphicsMode.BASIC_EFFECT)
                {
                    customEffect = basicEffect;
                    customEffect.Begin();
                    customEffect.CurrentTechnique.Passes[0].Begin();
                }
                graphicsMode = mode;
            }

        }

        public void End()
        {
            if (graphicsMode == GraphicsMode.SPRITE_BATCH)
            {
                spriteBatch.End();
            }
            else if (graphicsMode == GraphicsMode.BASIC_EFFECT || graphicsMode == GraphicsMode.CUSTOM_EFFECT)
            {
                Debug.Assert(customEffect != null);
                customEffect.CurrentTechnique.Passes[0].End();
                customEffect.End();
                customEffect = null;
            }
        }
    }
}
