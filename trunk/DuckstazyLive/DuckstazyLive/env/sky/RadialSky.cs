using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DuckstazyLive.graphics;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.env.sky
{
    public class RadialSky : AbstractSky
    {
        private Primitive raysPrimitive;
        private Circle circle;

        public RadialSky(int raysCount, Vector2 position, float radius, Color upperColor, Color lowerColor) : base(upperColor, lowerColor)
        {
            raysPrimitive = InitializeRaysGeometry(position, radius, raysCount);
            circle = new Circle(position, 20.0f, 100);
        }

        private Primitive InitializeRaysGeometry(Vector2 center, float radius, int raysCount)
        {
            int verticesCount = 1 + raysCount * 2;
            int indicesCount = raysCount * 3;

            VertexPositionColor[] vertices = new VertexPositionColor[verticesCount];
            short[] indices = new short[indicesCount];

            // center
            Vector3 position = new Vector3(center, 0.0f);
            vertices[0] = new VertexPositionColor(position, Color.White);            

            // rays
            float da = MathHelper.TwoPi / (2 * raysCount);
            float angle = -da / 2 - MathHelper.PiOver2;            
            for (int vertexIndex = 1; vertexIndex < verticesCount; vertexIndex++)
            {
                position.X = center.X + (float)(radius * Math.Cos(angle));
                position.Y = center.Y + (float)(radius * Math.Sin(angle));
                vertices[vertexIndex] = new VertexPositionColor(position, Color.White);                
                angle += da;                
            }
            
            int i = 0;
            short index = 1;
            for (int rayIndex = 0; rayIndex < raysCount; rayIndex++)
            {                
                indices[i++] = 0;
                indices[i++] = index++;
                indices[i++] = index++;
            }

            Primitive primitive = new Primitive();
            primitive.SetData(vertices, indices, PrimitiveType.TriangleList, raysCount);
            return primitive;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(RenderContext context)
        {
            raysPrimitive.Draw(context.BasicEffect);
            circle.Draw(context.BasicEffect);
        }
    }
}
