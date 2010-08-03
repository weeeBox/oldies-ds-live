using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace DuckstazyLive.framework.graphics
{
    public enum GraphicsAnchor
    {
        LEFT = 1,
        RIGHT = 2,
        TOP = 4,
        BOTTOM = 8,
        HCENTER = 16,
        VCENTER = 32,
    }

    enum GraphicsMode
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
        private Matrix world;
        private Matrix view;
        private Matrix projection;

        public GameGraphics(GraphicsDevice graphicsDevice, float width, float height)
        {
            transformationStack = new Stack<Matrix>();
            this.graphicsDevice = graphicsDevice;            

            spriteBatch = new SpriteBatch(graphicsDevice);
            basicEffect = new BasicEffect(graphicsDevice, null);
            world = Matrix.Identity;
            view = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up);
            projection = Matrix.CreateOrthographicOffCenter(0, width, height, 0, 1.0f, 1000.0f);
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = true;

            customEffect = null;
            currentTransform = Matrix.Identity;
        }

        public void PopMatrix()
        {
            End();
            currentTransform = transformationStack.Pop();
        }

        public void PushMatrix()
        {
            transformationStack.Push(currentTransform);
        }

        public void AddTransform(Matrix m)
        {
            End();
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

        public void Rotate(float radians)
        {
            AddTransform(Matrix.CreateRotationZ(radians));
        }

        public void Begin(Effect effect)
        {
            if (graphicsMode == GraphicsMode.SPRITE_BATCH || this.customEffect != effect)
            {
                End();

                BeginEffect(effect);
                graphicsMode = GraphicsMode.CUSTOM_EFFECT;
            }
        }

        public SpriteBatch GetSpriteBatch()
        {
            if (graphicsMode != GraphicsMode.SPRITE_BATCH)
            {
                End();
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, currentTransform);
                graphicsMode = GraphicsMode.SPRITE_BATCH;
            }

            return spriteBatch;
        }

        public void BeginEffect(Effect effect)
        {
            if (graphicsMode != GraphicsMode.CUSTOM_EFFECT || customEffect != effect)
            {
                End();
                BeginEffectHelper(effect);
                graphicsMode = GraphicsMode.CUSTOM_EFFECT;
            }
        }

        public void BeginBasicEffect()
        {
            if (graphicsMode != GraphicsMode.BASIC_EFFECT)
            {
                End();
                BeginEffectHelper(basicEffect);
                graphicsMode = GraphicsMode.BASIC_EFFECT;
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
            graphicsMode = GraphicsMode.UNDEFINED;
        }

        private void BeginEffectHelper(Effect effect)
        {
            customEffect = effect;
            customEffect.Begin();
            customEffect.CurrentTechnique.Passes[0].Begin();
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }

        public Matrix WorldMatrix
        {
            get { return world; }
        }

        public Matrix ViewMatrix
        {
            get { return view; }
        }

        public Matrix ProjectionMatrix
        {
            get { return projection; }
        }
    }
}
