using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DuckstazyLive.framework.graphics;

namespace DuckstazyLive.core.graphics
{
    public class GDebug
    {
        private static readonly int VERTICES_PER_CIRCLE = 20;

        private static VertexPositionColor[] vertices = new VertexPositionColor[100];        
        private static int verticesCount;

        private static Color color = Color.White;        
        private static VertexDeclaration vertexDeclaration;        
        
        public static void Init(GraphicsDevice device)
        {            
            vertexDeclaration = new VertexDeclaration(device, VertexPositionColor.VertexElements);
        }

        public static Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public static void DrawLine(float x1, float y1, float x2, float y2)
        {
            AddVertex(x1, y1);
            AddVertex(x2, y2);            
        }

        public static void DrawRect(float x, float y, float width, float height)
        {
            DrawLine(x, y, x + width, y);
            DrawLine(x + width, y, x + width, y + height);
            DrawLine(x + width, y + height, x, y + height);
            DrawLine(x, y + height, x, y);
        }
        
        public static void DrawCircle(float x, float y, float radius)
        {
            float da = MathHelper.TwoPi / VERTICES_PER_CIRCLE;
            float angle = 0;
            for (int i = 0; i < VERTICES_PER_CIRCLE; i++)
            {
                float vx = x + (float)(radius * Math.Cos(angle));
                float vy = y + (float)(radius * Math.Sin(angle));
                AddVertex(vx, vy);

                angle += da;
                vx = x + (float)(radius * Math.Cos(angle));
                vy = y + (float)(radius * Math.Sin(angle));
                AddVertex(vx, vy);
            }
        }

        public static void Flush(GameGraphics g)
        {
            if (verticesCount > 0)
            {
                g.BeginBasicEffect();
                g.GraphicsDevice.VertexDeclaration = vertexDeclaration;                
                g.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, verticesCount / 2);
                verticesCount = 0;
            }
        }                

        public static void Clear()
        {
            verticesCount = 0;
        }

        private static void AddVertex(float x, float y)
        {
            if (verticesCount == vertices.Length)
                GrowVertexArray(vertices.Length * 2);

            vertices[verticesCount] = new VertexPositionColor(new Vector3(x, y, 0), color);
            verticesCount++;
        }

        private static void GrowVertexArray(int size)
        {
            VertexPositionColor[] temp = new VertexPositionColor[size];
            Array.Copy(vertices, temp, vertices.Length);
            vertices = temp;
        }
    }
}
