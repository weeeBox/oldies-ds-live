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

        private static GraphicsDevice gd;
        private static SpriteBatch spriteBatch;

        private static BatchMode batchMode = BatchMode.None;
        private static Matrix matrix;
        private static Color drawColor;
        private static AppBlendMode blendMode = AppBlendMode.AlphaBlend;

        private static Stack<Matrix> matrixStack;

        private static RenderTarget2D triangleRenderTarget;
        private static RenderTarget2D triangleRenderTarget2;
        private static RenderTarget2D mainRenderTarget;
        private static Texture2D solidTexture;
        private static Texture2D triangleTexture;
        public static Texture2D circleTexture;

        public static SpriteFont arial11;
        public static SpriteFont arial13;

        private static Viewport rtexVp = new Viewport { X = 0, Y = 0, Width = RTexW, Height = RTexH };

        private static Color transpColor = new Color(0, 0, 0, 0);
        private static Vector2 zeroVector = new Vector2(0, 0);

        private static float lineWidth;

        private static RenderTarget2D currentRt;

        private static void BeginSpriteBatch(SpriteBatch sb, AppBlendMode blendMode, Matrix m)
        {
#if WINDOWSPHONE
            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, RasterizerState.CullNone, null, m);
#else
            SpriteBlendMode sbm = (blendMode == AppBlendMode.None) ? SpriteBlendMode.None : SpriteBlendMode.AlphaBlend;
            sb.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, m);
#if WINDOWS
            gd.RenderState.CullMode = CullMode.None;
#endif
#endif
        }

        private static RenderTarget2D CreateRenderTarget(int w, int h)
        {
#if WINDOWSPHONE
            return new RenderTarget2D(gd, w, h, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
#else
            return new RenderTarget2D(gd, w, h, 1, SurfaceFormat.Color, RenderTargetUsage.PreserveContents);
#endif
        }

        private static void SetRenderTarget(RenderTarget2D rt)
        {
            currentRt = rt;
#if WINDOWSPHONE
            gd.SetRenderTarget(rt);
#else
            gd.SetRenderTarget(0, rt);
            if (rt != null)
                gd.Viewport = new Viewport { X = 0, Y = 0, Width = rt.Width, Height = rt.Height };
            else
                gd.Viewport = new Viewport { X = 0, Y = 0, Width = Constants.SCREEN_WIDTH, Height = Constants.SCREEN_HEIGHT };
#endif
        }

        private static Texture2D GetRtTexture(RenderTarget2D rt)
        {
#if WINDOWSPHONE
            return rt;
#else
            return rt.GetTexture();
#endif
        }

        private static void DrawInternal(Texture2D t, Vector2 p, Color c)
        {
            //drawColor = c;
            if (blendMode == AppBlendMode.Additive)
            {
                //GetSpriteBatch().Draw(t, p, c);

#if WINDOWSPHONE
                EndBatch();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, RasterizerState.CullNone, null, matrix);
                spriteBatch.Draw(t, p, new Color(0, 0, 0, c.A));
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, RasterizerState.CullNone, null, matrix);
                spriteBatch.Draw(t, p, new Color(c.R, c.G, c.B, 255));
                spriteBatch.End();

#else
                EndBatch();
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, matrix);
#if WINDOWS
                gd.RenderState.CullMode = CullMode.None;
#endif
                spriteBatch.Draw(t, p, new Color(0, 0, 0, c.A));
                spriteBatch.End();

                spriteBatch.Begin(SpriteBlendMode.Additive, SpriteSortMode.Immediate, SaveStateMode.SaveState, matrix);
#if WINDOWS
                gd.RenderState.CullMode = CullMode.None;
#endif
                spriteBatch.Draw(t, p, new Color(c, 1.0f));
                spriteBatch.End();
#endif
            }
            else
            {
                GetSpriteBatch().Draw(t, p, c);
            }
        }

        private static void DrawInternal(Texture2D t, Vector2 p, Rectangle s, Color c, float size)
        {
            if (blendMode == AppBlendMode.Additive)
            {
                //GetSpriteBatch().Draw(t, p, s, c, 0, zeroVector, size, SpriteEffects.None, 0);
#if WINDOWSPHONE
                EndBatch();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, RasterizerState.CullNone, null, matrix);
                spriteBatch.Draw(t, p, new Color(0, 0, 0, c.A));
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, RasterizerState.CullNone, null, matrix);
                spriteBatch.Draw(t, p, new Color(c.R, c.G, c.B, 255));
                spriteBatch.End();

#else
                EndBatch();
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, matrix);
#if WINDOWS
                gd.RenderState.CullMode = CullMode.None;
#endif
                spriteBatch.Draw(t, p, s, new Color(0, 0, 0, c.A), 0, zeroVector, size, SpriteEffects.None, 0); // Draw(t, p, new Color(0, 0, 0, c.A));
                spriteBatch.End();

                spriteBatch.Begin(SpriteBlendMode.Additive, SpriteSortMode.Immediate, SaveStateMode.SaveState, matrix);
#if WINDOWS
                gd.RenderState.CullMode = CullMode.None;
#endif
                spriteBatch.Draw(t, p, s, new Color(c, 1.0f), 0, zeroVector, size, SpriteEffects.None, 0); // Draw(t, p, new Color(c, 1.0f));
                spriteBatch.End();
#endif
            }
            else
            {
                GetSpriteBatch().Draw(t, p, s, c, 0, zeroVector, size, SpriteEffects.None, 0);
            }
        }

        private static void DrawInternal(Texture2D t, Vector2 p, Rectangle s, Color c)
        {
            if (blendMode == AppBlendMode.Additive)
            {
                GetSpriteBatch().Draw(t, p, s, c);
            }
            else
            {
                GetSpriteBatch().Draw(t, p, s, c);
            }
        }

        private static void DrawInternal(Texture2D t, Rectangle d, Color c)
        {
            if (blendMode == AppBlendMode.Additive)
            {
                GetSpriteBatch().Draw(t, d, Color.White);
            }
            else
            {
                GetSpriteBatch().Draw(t, d, c);
            }
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

        public static void Begin(GraphicsDevice _gd)
        {
            matrixStack = new Stack<Matrix>();

            if (gd != _gd)
            {
                gd = _gd;
                spriteBatch = new SpriteBatch(gd);

                triangleRenderTarget = CreateRenderTarget(RTexW, RTexH);
                triangleRenderTarget2 = CreateRenderTarget(RTexW, RTexH);

                RenderTarget2D solidRenderTarget = CreateRenderTarget(STexW, STexH);
                SetRenderTarget(solidRenderTarget);
                gd.Clear(Color.White);
                SetRenderTarget(null);
                solidTexture = GetRtTexture(solidRenderTarget);

                RenderTarget2D solidTriangleRenderTarget = CreateRenderTarget(TTexW, TTexH);
                SetRenderTarget(solidTriangleRenderTarget);
                gd.Viewport = new Viewport { X = 0, Y = 0, Width = TTexW, Height = TTexH };
                gd.Clear(new Color(0, 0, 0, 0));
                Matrix m = CreateTriangleTransform(
                    STexW, 0, 0, 0, 0, STexH,
                    TTexW, TTexH, TTexW, 0, 0, TTexH
                    );
                BeginSpriteBatch(spriteBatch, AppBlendMode.None, m);
                spriteBatch.Draw(solidTexture, zeroVector, Color.White);
                spriteBatch.End();
                SetRenderTarget(null);
                triangleTexture = GetRtTexture(solidTriangleRenderTarget);

                mainRenderTarget = CreateRenderTarget(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT);
                //gd.Viewport = new Viewport { X = 0, Y = 0, Width = Constants.SCREEN_WIDTH, Height = Constants.SCREEN_HEIGHT };
            }

            matrix = Matrix.Identity;
            batchMode = BatchMode.None;
            drawColor = Color.White;
            blendMode = AppBlendMode.AlphaBlend;

            SetRenderTarget(mainRenderTarget);
            //gd.Viewport = new Viewport { X = 0, Y = 0, Width = Constants.SCREEN_WIDTH, Height = Constants.SCREEN_HEIGHT };

            SetLineWidth(1);
        }

        public static void End()
        {
            EndBatch();
            //matrixStack = null;
            matrix = Matrix.Identity;
            matrixStack = new Stack<Matrix>();

            SetRenderTarget(null);
            gd.Viewport = new Viewport { X = 0, Y = 0, Width = Constants.SCREEN_WIDTH, Height = Constants.SCREEN_HEIGHT };
#if WINDOWS
            gd.Clear(Color.Black);
#else
            gd.Clear(Color.White);
#endif
            BeginSpriteBatch(spriteBatch, AppBlendMode.None, Matrix.Identity);
#if WINDOWS
            spriteBatch.Draw(GetRtTexture(mainRenderTarget), new Rectangle(24, 0, 272, 480), new Rectangle(24, 0, 272, 480), Color.White);
#else
            spriteBatch.Draw(GetRtTexture(mainRenderTarget), new Vector2(- Constants.XOFFSET, 0), Color.White);
#endif
            spriteBatch.End();
        }

        public static void BeginCustom(int width, int height)
        {
            EndBatch();

            PushMatrix();
            SetIdentity();

            RenderTarget2D rt = CreateRenderTarget(width, height);
            SetRenderTarget(rt);
            gd.Viewport = new Viewport { X = 0, Y = 0, Width = width, Height = height };
            gd.Clear(transpColor);
        }

        public static Texture2D EndCustom()
        {
            EndBatch();

            PopMatrix();

            RenderTarget2D rt = currentRt;
            SetRenderTarget(mainRenderTarget);

            return GetRtTexture(rt);
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
            DrawInternal(tex, new Vector2(x, y), drawColor);
            //GetSpriteBatch().Draw(tex, new Vector2(x, y), drawColor);
        }

        public static void DrawImage(Texture2D tex, float x, float y, float opacity)
        {
            DrawInternal(tex, new Vector2(x, y), new Color(1.0f, 1.0f, 1.0f, opacity));
            //GetSpriteBatch().Draw(tex, new Vector2(x, y), new Color(1.0f, 1.0f, 1.0f, opacity));
        }

        public static void DrawImage(Texture2D tex, float x, float y, Color color)
        {
            DrawInternal(tex, new Vector2(x, y), color);
            //GetSpriteBatch().Draw(tex, new Vector2(x, y), color);
        }

        public static void DrawImagePart(Texture2D tex, Rectangle src, float x, float y)
        {
            DrawInternal(tex, new Vector2(x, y), src, drawColor);
            //GetSpriteBatch().Draw(tex, new Vector2(x, y), src, drawColor);
        }

        public static void DrawImagePart(Texture2D tex, Rectangle src, float x, float y, Color dc, float size)
        {
            DrawInternal(tex, new Vector2(x, y), src, dc, size);
            //GetSpriteBatch().Draw(tex, new Vector2(x, y), src, drawColor);
        }

        public static Texture2D createTexture(String str, int w, int h, HAlign align, String fontName, float fontSize)
        {
            SpriteFont font = null;
            if (fontSize == 11)
                font = arial11;
            else if (fontSize == 13)
                font = arial13;
            else
                return null;

            BeginCustom(w, h);
            Vector2 size = font.MeasureString(str);
            float px = 0;
            if (align == HAlign.ALIGN_CENTER)
                px = (w - size.X) / 2;
            else if (align == HAlign.ALIGN_RIGHT)
                px = w - size.X;
            float py = 0;//(h - size.Y) / 2;
            GetSpriteBatch().DrawString(font, str, new Vector2(px, py), Color.White);

            return EndCustom();
        }

        public static void DrawTexturedLine(float x1, float y1, float x2, float y2, float size, Texture2D image)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            float len = (float)Math.Sqrt(dx * dx + dy * dy);
            float ax = -dy * size / len;
            float ay = dx * size / len;

            int w = (int)len;
            if (w < 1)
                w = 1;

            int h = (int)(size * 2);
            Matrix m = CreateTriangleTransform(
                0, h, 0, 0, w, h,
                x1 + ax, y1 + ay, x1 - ax, y1 - ay, x2 + ax, y2 + ay
                );

            Rectangle rect = new Rectangle(3, 3, w, (int)(size * 2));

            PushMatrix();
            AddTransform(m);
            DrawInternal(image, zeroVector, rect, Color.White);
            //GetSpriteBatch().Draw(image, zeroVector, rect, Color.White);
            PopMatrix();
        }

        public static void DrawSolidCircle(double x, double y, double radius, int numVerts, Color border, Color fill)
        {
            if (circleTexture == null)
            {
                float[] vertices = calcCircle((float)x, (float)y, (float)radius, numVerts);
                DrawSolidPolygon(vertices, numVerts, border, fill);
            }
            else
            {
                DrawInternal(circleTexture, new Rectangle((int)(x - radius), (int)(y - radius), (int)(radius * 2), (int)(radius * 2)), fill);
                //GetSpriteBatch().Draw(circleTexture, new Rectangle((int)(x - radius), (int)(y - radius), (int)(radius * 2), (int)(radius * 2)), fill);
            }
        }

        public static void DrawTexturedCircle(double x, double y, double radius, float[] texels, int numVerts, Texture2D image, float opacity)
        {
            float[] vertices = calcCircle((float)x, (float)y, (float)radius, numVerts);
            SetBlendMode(AppBlendMode.AlphaBlend);
            DrawTexturedPolygon(vertices, texels, numVerts, Mode.GL_TRIANGLE_FAN, image, opacity);
        }

        public static void DrawTexturedPolygonShape(float[] vertices, int vertCount, float size, Texture2D image)
        {
            for (int i = 0; i <= (vertCount * 2) - 4; i += 2)
            {
                DrawTexturedLine(vertices[i], vertices[i + 1], vertices[i + 2], vertices[i + 3], size, image);
            }
            DrawTexturedLine(vertices[(vertCount * 2) - 2], vertices[(vertCount * 2) - 1], vertices[0], vertices[1], size, image);
        }

        public static void DrawTexturedPolygon(float[] vertices, float[] texels, int vertexCount, Mode mode, Texture2D image, float opacity)
        {
            float[] v = new float[6];
            float[] c = new float[6];

            Color clr = new Color(1.0f, 1.0f, 1.0f, opacity);

            if (mode == Mode.GL_TRIANGLE_FAN)
            {
                v[0] = texels[0];
                v[1] = texels[1];
                c[0] = vertices[0];
                c[1] = vertices[1];

                for (int i = 1; i < vertexCount - 1; i++)
                {
                    int pos = i * 2;
                    int p = 2;
                    for (int j = 0; j < 4; j++)
                    {
                        v[p] = texels[pos];
                        c[p] = vertices[pos];
                        pos++;
                        p++;
                    }

                    DrawTexturedTriangle(image, v, c, clr);
                }
            }
            else if (mode == Mode.GL_TRIANGLE_STRIP)
            {
                for (int i = 0; i < vertexCount - 2; i++)
                {
                    int pos = i * 2;
                    int p = 0;
                    for (int j = 0; j < 6; j++)
                    {
                        v[p] = texels[pos];
                        c[p] = vertices[pos];
                        pos++;
                        p++;
                    }

                    DrawTexturedTriangle(image, v, c, clr);
                }
            }

            /*
            // This is not valid on ZuneHD (? on WP7 ?)

            BasicEffect be = GetBasicEffect(image, opacity);
            int sz = vertices.Length / 2;
            VertexPositionTexture[] v = new VertexPositionTexture[sz];
            //be.TextureEnabled = true;
            //be.Texture = image;
            int pos = 0;
            for (int i = 0; i < sz; i++)
            {
                v[i].Position = new Vector3(vertices[pos], vertices[pos + 1], 0);
                v[i].TextureCoordinate.X = texels[pos];
                v[i].TextureCoordinate.Y = texels[pos + 1];
                pos += 2;
            }

            gd.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleFan, v, 0, vertexCount - 2);

            // Very tricky
            EndBatch();
             */
        }

        public static void DrawPolygon(float[] vertices, int vertexCount, Color color)
        {
            int pos = 0;
            float px = vertices[vertexCount * 2 - 2];
            float py = vertices[vertexCount * 2 - 1];
            for (int i = 0; i < vertexCount; i++)
            {
                float nx = vertices[pos++];
                float ny = vertices[pos++];
                DrawLine(px, py, nx, ny, color);
                px = nx;
                py = ny;
            }
        }

        public static void DrawSolidPolygon(float[] vertices, int vertexCount, Color border, Color fill)
        {
            for (int i = 1; i < vertexCount - 1; i++)
            {
                DrawSolidTriangle(vertices[0], vertices[1], vertices[i * 2], vertices[i * 2 + 1], vertices[i * 2 + 2], vertices[i * 2 + 3], fill);
            }
        }

        public static void DrawSolidPolygonWOBorder(float[] vertices, int vertexCount, Color fill)
        {
            for (int i = 1; i < vertexCount - 1; i++)
            {
                DrawSolidTriangle(vertices[0], vertices[1], vertices[i * 2], vertices[i * 2 + 1], vertices[i * 2 + 2], vertices[i * 2 + 3], fill);
            }
        }

        public static void DrawCircle(float x, float y, float radius, int numVerts, Color color)
        {
            float[] vertices = calcCircle((float)x, (float)y, (float)radius, numVerts);
            DrawPolygon(vertices, numVerts, color);
        }

        public static void DrawRect(float x, float y, float w, float h, Color color)
        {
            DrawLine(x, y, x + w, y, color);
            DrawLine(x + w, y, x + w, y + h, color);
            DrawLine(x + w, y + h, x, y + h, color);
            DrawLine(x, y + h, x, y, color);
        }

        public static void DrawPoint(double x, double y, double size, Color color)
        {
            DrawSolidCircle(x, y, size / 2, 0, color, color);
        }

        public static void DrawSegment(double x1, double y1, double x2, double y2, Color color)
        {
            DrawLine((float)x1, (float)y1, (float)x2, (float)y2, color);
        }

        public static void DrawImageColored(Texture2D tex, int x, float y, Color color)
        {
            DrawInternal(tex, new Vector2(x, y), color);
            //GetSpriteBatch().Draw(tex, new Vector2(x, y), color);
        }

        public static void SetLineWidth(float p)
        {
            lineWidth = p;// *0.9f;
        }

        public static void drawSolidRectWOBorder(float x, float y, int w, int h, Color color)
        {
            float[] verts = { x, y, x + w, y, x + w, y + h, x, y + h };
            DrawSolidPolygonWOBorder(verts, 4, color);
        }

        public static void Clear(Color color)
        {
            EndBatch();
            gd.Clear(color);
        }

        public static void DrawSolidRect(float x, float y, float w, float h, Color border, Color fill)
        {
            float[] verts = { x, y, x + w, y, x + w, y + h, x, y + h };
            DrawSolidPolygon(verts, 4, border, fill);
        }

        private static float[] calcCircle(float x, float y, float radius, int vertexCount)
        {
            float[] vertices = new float[vertexCount * 2];

            float theta = 0.0f;
            float k_increment = 2.0f * (float)Math.PI / vertexCount;

            for (int i = 0; i < vertexCount; ++i)
            {
                vertices[i * 2] = x + radius * (float)Math.Cos(theta);
                vertices[i * 2 + 1] = y + radius * (float)Math.Sin(theta);
                theta += k_increment;
            }

            return vertices;
        }

        public static void DrawTexturedTriangle(Texture2D image, float[] texels, float[] coords, Color fill)
        {
            EndBatch();

            float w = image.Width;
            float h = image.Height;

            RenderTarget2D oldRt = currentRt;

            SetRenderTarget(triangleRenderTarget);
            gd.Viewport = rtexVp;
            Matrix m = CreateTriangleTransform(
                texels[0] * w, texels[1] * h, texels[2] * w, texels[3] * h, texels[4] * w, texels[5] * h,
                0, RTexH - 1, RTexW - 1, 0, RTexW - 1, RTexH - 1
                //0, 0, RTexW, 0, 0, RTexH
                );
            gd.Clear(transpColor);
            BeginSpriteBatch(spriteBatch, AppBlendMode.None, m);
            spriteBatch.Draw(image, zeroVector, Color.White);
            spriteBatch.End();

            SetRenderTarget(triangleRenderTarget2);
            gd.Viewport = rtexVp;
            m = CreateTriangleTransform(
                0, RTexH - 1, RTexW - 1, 0, RTexW - 1, RTexH - 1,
                RTexW - 1, 0, RTexW - 1, RTexH - 1, 0, RTexH - 1
                );
            gd.Clear(transpColor);

            BeginSpriteBatch(spriteBatch, AppBlendMode.None, m);
            spriteBatch.Draw(GetRtTexture(triangleRenderTarget), zeroVector, Color.White);
            spriteBatch.End();

            SetRenderTarget(oldRt);

            m = CreateTriangleTransform(
                RTexW - 0.00001f, 0, RTexW - 0.00001f, RTexH - 0.00001f, 0, RTexH - 0.00001f,
                coords[0], coords[1], coords[2], coords[3], coords[4], coords[5]
                //x1, y1, x2, y2, x3, y3
                );

            PushMatrix();
            AddTransform(m);
            DrawInternal(GetRtTexture(triangleRenderTarget2), zeroVector, fill);
            //GetSpriteBatch().Draw(GetRtTexture(triangleRenderTarget2), zeroVector, fill);
            PopMatrix();
        }

        public static void DrawSolidTriangle(float x1, float y1, float x2, float y2, float x3, float y3, Color fill)
        {
            Matrix m = CreateTriangleTransform(
                TTexW - 1, 0, TTexW - 1, TTexH - 1, 0, TTexH - 1,
                x1, y1, x2, y2, x3, y3
                );

            PushMatrix();
            AddTransform(m);
            DrawInternal(triangleTexture, zeroVector, fill);
            //GetSpriteBatch().Draw(triangleTexture, zeroVector, fill);
            PopMatrix();
        }

        public static Matrix CreateTriangleTransform(float x1, float y1, float x2, float y2, float x3, float y3, float xn1, float yn1, float xn2, float yn2, float xn3, float yn3)
        {
            float a1 = ((xn2 - xn1) * y3 + (xn1 - xn3) * y2 + (xn3 - xn2) * y1) / ((x2 - x1) * y3 + (x1 - x3) * y2 + (x3 - x2) * y1);
            float a2 = ((x2 - x1) * xn3 + (x1 - x3) * xn2 + (x3 - x2) * xn1) / ((x2 - x1) * y3 + (x1 - x3) * y2 + (x3 - x2) * y1);
            float a3 = -((x1 * xn2 - x2 * xn1) * y3 + (x3 * xn1 - x1 * xn3) * y2 + (x2 * xn3 - x3 * xn2) * y1) / ((x2 - x1) * y3 + (x1 - x3) * y2 + (x3 - x2) * y1);
            float a4 = -((y2 - y1) * yn3 + (y1 - y3) * yn2 + (y3 - y2) * yn1) / ((x2 - x1) * y3 + (x1 - x3) * y2 + (x3 - x2) * y1);
            float a5 = ((x2 - x1) * yn3 + (x1 - x3) * yn2 + (x3 - x2) * yn1) / ((x2 - x1) * y3 + (x1 - x3) * y2 + (x3 - x2) * y1);
            float a6 = ((x1 * y2 - x2 * y1) * yn3 + (x3 * y1 - x1 * y3) * yn2 + (x2 * y3 - x3 * y2) * yn1) / ((x2 - x1) * y3 + (x1 - x3) * y2 + (x3 - x2) * y1);

            return new Matrix(
                a1, a4, 0, 0,
                a2, a5, 0, 0,
                0, 0, 1, 0,
                a3, a6, 0, 1
                );
        }

        public static void DrawLine(float x1, float y1, float x2, float y2, Color fill)
        {
            const float lineCoef = 2.0f;

            if ((x2 < x1) || (x2 == x1 && y2 < y1))
            {
                float b = x2;
                x2 = x1;
                x1 = b;
                b = y2;
                y2 = y1;
                y1 = b;
            }


            //x1 += 0.5f;
            //y1 += 0.5f;
            //x2 += 0.5f;
            //y2 += 0.5f;

            float dx = x2 - x1;
            float dy = y2 - y1;
            float len = (float)Math.Sqrt(dx * dx + dy * dy);
            dy = dy * lineWidth / lineCoef / len;
            dx = dx * lineWidth / lineCoef / len;

            x2 += dx;
            y2 += dy;
            x1 -= dx;
            y1 -= dy;

            Matrix m = CreateTriangleTransform(
                0, 0, 0, STexH, STexW, 0,
                x1 + dy, y1 - dx, x1 - dy, y1 + dx, x2 + dy, y2 - dx
                );

            PushMatrix();
            AddTransform(m);
            DrawInternal(solidTexture, zeroVector, fill);
            //GetSpriteBatch().Draw(solidTexture, zeroVector, fill);
            PopMatrix();
        }
    }
}
