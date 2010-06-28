using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DuckstazyLive.graphics
{
    public class Circle : Primitive
    {
        public Circle(float radius, int trianglesCount)
        {
            CreateGeometry(radius, trianglesCount);
        }

        private void CreateGeometry(float radius, int trianglesCount)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[trianglesCount + 1];
            short[] indices = new short[2 * (trianglesCount + 1)];

            double da = 2 * Math.PI / trianglesCount;
            double angle = 0;
            Vector3 position = new Vector3(200, 200, 0);
            vertices[0] = new VertexPositionColor(position, Color.White);

            Console.WriteLine("Triangles count: " + trianglesCount + " da: " + MathHelper.ToDegrees((float)da));

            for (int pointIndex = 1; pointIndex <= trianglesCount; pointIndex++)
            {
                position.X = 200 + (float) (radius * Math.Cos(angle));
                position.Y = 200 + (float) (radius * Math.Sin(angle));                             
                angle += da;
                vertices[pointIndex] = new VertexPositionColor(position, Color.White);                
            }

            for (int i = 0; i < indices.Length - 1; i++)
            {                
                indices[i] = (short)(i % 2 == 0 ? 0 : ((i / 2) + 1));
            }
            indices[indices.Length - 1] = 1;

            SetData(vertices, indices, PrimitiveType.TriangleStrip, indices.Length - 2);
        }
    }
        
}
