using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Framework.visual;
using System.Diagnostics;

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
        enum BatchMode
        {
            None,
            Sprite,
            Geometry,
        }

        private static GraphicsDevice graphicsDevice;
        private static SpriteBatch spriteBatch;
        private static BasicEffect basicEffect;

        private static SpriteFont infoFont;

        private static BatchMode batchMode = BatchMode.None;
        private static Matrix matrix;
        private static Color drawColor;
        private static AppBlendMode blendMode = AppBlendMode.AlphaBlend;

        private static Stack<Matrix> matrixStack = new Stack<Matrix>();        

        private static Color transpColor = new Color(0, 0, 0, 0);
        private static Vector2 zeroVector = new Vector2(0, 0);
        private static Camera camera;

        private static void BeginSpriteBatch(SpriteBatch sb, AppBlendMode blendMode, Matrix m, BatchMode mode)
        {
            Debug.Assert(mode != BatchMode.None);

            if (mode == BatchMode.Sprite)
            {
                SpriteBlendMode sbm = (blendMode == AppBlendMode.None) ? SpriteBlendMode.None : SpriteBlendMode.AlphaBlend;
                sb.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, m);
            }
            else if (mode == BatchMode.Geometry)
            {
                basicEffect.Begin();
                basicEffect.CurrentTechnique.Passes[0].Begin();
            }
            batchMode = mode;
        }        

        private static SpriteBatch GetSpriteBatch(BatchMode mode)
        {
            if (batchMode != mode)
            {
                EndBatch();
                BeginSpriteBatch(spriteBatch, blendMode, matrix, mode);                
            }
            return spriteBatch;
        }

        private static void EndBatch()
        {
            if (batchMode == BatchMode.Geometry)
            {           
                basicEffect.CurrentTechnique.Passes[0].End();
                basicEffect.End();                
            }
            else if (batchMode == BatchMode.Sprite)
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

        public static void SetInfoFont(SpriteFont font)
        {
            infoFont = font;
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
                basicEffect = new BasicEffect(graphicsDevice, null);

                Matrix worldMatrix = Matrix.Identity;
                Matrix viewMatrix = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up);
                Matrix projection = Matrix.CreateOrthographicOffCenter(0.0f, FrameworkConstants.SCREEN_WIDTH, FrameworkConstants.SCREEN_HEIGHT, 0, 1.0f, 1000.0f);
                camera = new Camera(worldMatrix, viewMatrix, projection);

                basicEffect.World = worldMatrix;
                basicEffect.View = viewMatrix;
                basicEffect.Projection = projection;
                basicEffect.VertexColorEnabled = true;
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

        public static void DrawString(float x, float y, String text)
        {
            GetSpriteBatch(BatchMode.Sprite).DrawString(infoFont, text, new Vector2((float)x, (float)y), Color.Red);
        }

        public static void DrawString(SpriteFont font, float x, float y, String text)
        {
            GetSpriteBatch(BatchMode.Sprite).DrawString(font, text, new Vector2((float)x, (float)y), Color.Red);
        }
      
        public static void DrawImage(Texture2D tex, float x, float y)
        {
            GetSpriteBatch(BatchMode.Sprite).Draw(tex, new Vector2(x, y), drawColor);
        }

        public static void DrawImage(Texture2D tex, float x, float y, float opacity)
        {
            GetSpriteBatch(BatchMode.Sprite).Draw(tex, new Vector2(x, y), new Color(1.0f, 1.0f, 1.0f, opacity));
        }

        public static void DrawImage(Texture2D tex, float x, float y, Color color)
        {
            GetSpriteBatch(BatchMode.Sprite).Draw(tex, new Vector2(x, y), color);
        }

        public static void DrawImagePart(Texture2D tex, Rectangle src, float x, float y)
        {
            GetSpriteBatch(BatchMode.Sprite).Draw(tex, new Vector2(x, y), src, drawColor);
        }

        public static void DrawImagePart(Texture2D tex, Rectangle src, float x, float y, Color dc, float size)
        {
            GetSpriteBatch(BatchMode.Sprite).Draw(tex, new Vector2(x, y), src, drawColor);
        }        

        public static void DrawImage(Texture2D tex, float x, float y, SpriteEffects flip)
        {
            GetSpriteBatch(BatchMode.Sprite).Draw(tex, new Vector2(x, y), null, drawColor, 0.0f, Vector2.Zero, 1.0f, flip, 0.0f);
        }

        public static void DrawImageTiled(Texture2D tex, ref Rectangle src, ref Rectangle dest)
        {
            // TODO: implement with texture repeat
            int destWidth = dest.Width;
            int destHeight = dest.Height;
            int srcWidth = src.Width;
            int srcHeight = src.Height;
            int numTilesX = destWidth / srcWidth + (destWidth % srcWidth != 0 ? 1 : 0);
            int numTilesY = destHeight / srcHeight + (destHeight % srcHeight != 0 ? 1 : 0);
            int x = dest.X;
            int y = dest.Y;
            for (int tileY = 0; tileY < numTilesY; ++tileY)
            {
                for (int tileX = 0; tileX < numTilesX; ++tileX)
                {
                    DrawImagePart(tex, src, x, y);
                    x += srcWidth;                    
                }
                y += srcHeight;
            }
        }

        public static void DrawCircle(float x, float y, float r, Color color)
        {
            GetSpriteBatch(BatchMode.Geometry);
            graphicsDevice.VertexDeclaration = new VertexDeclaration(graphicsDevice, VertexPositionColor.VertexElements);

            int numVertex = 10;
            VertexPositionColor[] vertexData = new VertexPositionColor[numVertex];
            float da = MathHelper.TwoPi / (numVertex - 1);
            float angle = 0;
            for (int i = 0; i < numVertex - 1; ++i)
            {
                float vx = (float)(x + r * Math.Cos(angle));
                float vy = (float)(y + r * Math.Sin(angle));
                vertexData[i] = new VertexPositionColor(new Vector3(vx, vy, 0), color);
                angle += da;
            }
            vertexData[numVertex - 1] = vertexData[0];
            graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertexData, 0, numVertex - 1);
        }

        public static void DrawRect(float x, float y, float width, float height, Color color)
        {
            GetSpriteBatch(BatchMode.Geometry);
            graphicsDevice.VertexDeclaration = new VertexDeclaration(graphicsDevice, VertexPositionColor.VertexElements);
            VertexPositionColor[] vertexData = new VertexPositionColor[4];
            vertexData[0] = new VertexPositionColor(new Vector3(x, y, 0), color);
            vertexData[1] = new VertexPositionColor(new Vector3(x + width, y, 0), color);
            vertexData[2] = new VertexPositionColor(new Vector3(x + width, y + height, 0), color);
            vertexData[3] = new VertexPositionColor(new Vector3(x, y + height, 0), color);
            short[] indexData = new short[] {0, 1, 2, 3, 0};

            graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineStrip, vertexData, 0, 4, indexData, 0, 4);
        }

        public static void DrawGeomerty(CustomGeomerty geometry)
        {
            GetSpriteBatch(BatchMode.Geometry);

            graphicsDevice.VertexDeclaration = geometry.VertexDeclaration;
            if (geometry.IndexData == null)
            {
                graphicsDevice.DrawUserPrimitives(geometry.PrimitiveType, geometry.VertexData, 0, geometry.PrimitiveCount);
            }
            else
            {
                graphicsDevice.DrawUserIndexedPrimitives(geometry.PrimitiveType, geometry.VertexData, 0, geometry.VertexData.Length, geometry.IndexData, 0, geometry.PrimitiveCount);
            }
        }

        public static void Clear(Color color)
        {
            EndBatch();
            graphicsDevice.Clear(color);
        }        

        public static GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }
    }
}
