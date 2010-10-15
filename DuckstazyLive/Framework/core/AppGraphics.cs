using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Framework.core
{
    public enum AppBlendMode
    {
        None,
        AlphaBlend,
        Additive,
    }

    public class AppGraphics
    {
        private const int RTexW = 128;
        private const int RTexH = 128;
        private const int TTexW = 64;
        private const int TTexH = 64;
        private const int STexW = 8;
        private const int STexH = 8;

        public enum Mode
        {
            GL_TRIANGLE_FAN,
            GL_TRIANGLE_STRIP
        }

        enum BatchMode
        {
            None,
            Sprite,
        }

        private static GraphicsDevice graphicsDevice;
        private static SpriteBatch spriteBatch;

        private static BatchMode batchMode = BatchMode.None;
        private static Matrix matrix;
        private static Color drawColor;
        private static AppBlendMode blendMode = AppBlendMode.AlphaBlend;

        private static Stack<Matrix> matrixStack = new Stack<Matrix>();        

        private static Color transpColor = new Color(0, 0, 0, 0);
        private static Vector2 zeroVector = new Vector2(0, 0);

        private static void BeginSpriteBatch(SpriteBatch sb, AppBlendMode blendMode, Matrix m)
        {
            SpriteBlendMode sbm = (blendMode == AppBlendMode.None) ? SpriteBlendMode.None : SpriteBlendMode.AlphaBlend;
            sb.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, m);
            batchMode = BatchMode.Sprite;
        }        

        private static SpriteBatch GetSpriteBatch()
        {
            if (batchMode != BatchMode.Sprite)
            {
                EndBatch();

                BeginSpriteBatch(spriteBatch, blendMode, matrix);
                batchMode = BatchMode.Sprite;
            }
            return spriteBatch;
        }

        private static void EndBatch()
        {
            if (batchMode == BatchMode.Sprite)
            {
                spriteBatch.End();
            }

            batchMode = BatchMode.None;
        }

        public static void SetBlendMode(AppBlendMode bm)
        {
            if (blendMode != bm)
            {
                EndBatch();
                blendMode = bm;
            }
        }

        public static void SetColor(Color color)
        {
            drawColor = color;
        }

        public static void SetMatrix(Matrix _matrix)
        {
            matrix = _matrix;
            EndBatch();
        }

        public static void Begin(GraphicsDevice gd)
        {
            matrixStack.Clear();
            matrix = Matrix.Identity;
            batchMode = BatchMode.None;
            drawColor = Color.White;
            blendMode = AppBlendMode.AlphaBlend;

            if (graphicsDevice != gd)
            {
                graphicsDevice = gd;
                spriteBatch = new SpriteBatch(graphicsDevice);
                
                BeginSpriteBatch(spriteBatch, AppBlendMode.None, Matrix.Identity);                
            }            
        }

        public static void End()
        {
            EndBatch();
        }        

        public static void PushMatrix()
        {
            matrixStack.Push(matrix);
        }

        public static void PopMatrix()
        {
            EndBatch();
            matrix = matrixStack.Pop();
        }

        public static void SetIdentity()
        {
            EndBatch();
            matrix = Matrix.Identity;
        }

        private static void AddTransform(Matrix t)
        {
            EndBatch();
            matrix = Matrix.Multiply(t, matrix);
        }

        public static void Translate(double tx, double ty, double tz)
        {
            Translate((float)tx, (float)ty, (float)tz);
        }

        public static void Translate(float tx, float ty, float tz)
        {
            AddTransform(Matrix.CreateTranslation(tx, ty, tz));
        }

        public static void Rotate(float grad, float ax, float ay, float az)
        {
            Matrix r;
            float rad = (float)(Math.PI * grad / 180);
            if (ax == 1 && ay == 0 && az == 0)
                r = Matrix.CreateRotationX(rad);
            else if (ax == 0 && ay == 1 && az == 0)
                r = Matrix.CreateRotationY(rad);
            else if (ax == 0 && ay == 0 && az == 1)
                r = Matrix.CreateRotationZ(rad);
            else
                throw new NotImplementedException();

            AddTransform(r);
        }

        public static void Scale(float sx, float sy, float sz)
        {
            Matrix r = Matrix.CreateScale(sx, sy, sz);
            AddTransform(r);
        }

        public static void DrawString(SpriteFont font, double x, double y, String text)
        {
            GetSpriteBatch().DrawString(font, text, new Vector2((float)x, (float)y), Color.Red);
        }

        public static void DrawImage(Texture2D tex, double x, double y)
        {
            DrawImage(tex, (float)x, (float)y);
        }

        public static void DrawImagePart(Texture2D tex, Rectangle src, double x, double y)
        {
            DrawImagePart(tex, src, (float)x, (float)y);
        }

        public static void DrawImage(Texture2D tex, float x, float y)
        {            
            GetSpriteBatch().Draw(tex, new Vector2(x, y), drawColor);
        }

        public static void DrawImage(Texture2D tex, float x, float y, float opacity)
        {         
            GetSpriteBatch().Draw(tex, new Vector2(x, y), new Color(1.0f, 1.0f, 1.0f, opacity));
        }

        public static void DrawImage(Texture2D tex, float x, float y, Color color)
        {            
            GetSpriteBatch().Draw(tex, new Vector2(x, y), color);
        }

        public static void DrawImagePart(Texture2D tex, Rectangle src, float x, float y)
        {            
            GetSpriteBatch().Draw(tex, new Vector2(x, y), src, drawColor);
        }

        public static void DrawImagePart(Texture2D tex, Rectangle src, float x, float y, Color dc, float size)
        {            
            GetSpriteBatch().Draw(tex, new Vector2(x, y), src, drawColor);
        }

        public static void Clear(Color color)
        {
            EndBatch();
            graphicsDevice.Clear(color);
        }        
    }
}
