using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DuckstazyLive.graphics;

namespace DuckstazyLive
{
    public class GradientRect
    {
        private float x;
        private float y;
        private float width;
        private float height;

        private VertexPositionColor[] vertices;        
                
        private GraphicsDevice device;
        private BasicEffect effect;

        private Primitive primitive;
        
        public GradientRect(GraphicsDevice device, float x, float y, float width, float height, Color upperColor, Color lowerColor)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.device = device;

            InitVertices(ref upperColor, ref lowerColor);
        }

        private void InitVertices(ref Color upperColor, ref Color lowerColor)
        {
            vertices = new VertexPositionColor[4];
            vertices[0] = new VertexPositionColor(new Vector3(x, y, 0), upperColor);
            vertices[1] = new VertexPositionColor(new Vector3(x + width, y, 0), upperColor);
            vertices[2] = new VertexPositionColor(new Vector3(x + width, y + height, 0), lowerColor);
            vertices[3] = new VertexPositionColor(new Vector3(x, y + height, 0), lowerColor);

            short[] indices = new short[4] {3, 0, 2, 1};                 

            effect = new BasicEffect(device, null);
            effect.VertexColorEnabled = true;

            primitive = new Primitive(device, vertices, indices, PrimitiveType.TriangleStrip, indices.Length - 2);
        }              

        public void fillWith(Color color)
        {
            for (int vertexIndex = 0; vertexIndex < vertices.Length; vertexIndex++)
            {
                vertices[vertexIndex].Color = color;                
            }
        }

        public void Draw(ref Matrix view, ref Matrix projection, ref Matrix world)
        {
            effect.View = view;
            effect.Projection = projection;
            effect.World = world;

            primitive.Draw(effect);
        }

        public float GetX()
        {
            return x;
        }

        public float GetY()
        {
            return y;
        }

        public float GetWidth()
        {
            return width;
        }

        public float GetHeight()
        {
            return height;
        }
    }
}
