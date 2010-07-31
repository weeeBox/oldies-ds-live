using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.framework.graphics
{
    class GameGraphics
    {
        private Stack<Matrix> transformationStack;
        private Matrix currentTransform;
        private GraphicsDevice graphicsDevice;

        GameGraphics(GraphicsDevice graphicsDevice)
        {
            transformationStack = new Stack<Matrix>();
            this.graphicsDevice = graphicsDevice;
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
    }
}
