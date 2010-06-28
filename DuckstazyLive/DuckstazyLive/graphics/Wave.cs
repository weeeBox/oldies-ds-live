using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.graphics
{
    class Wave : Primitive
    {
        private float elapsed;        
        private int pointsCount;

        public Wave(float x, float y, float width, float height, int pointsCount)
        {            
            this.pointsCount = pointsCount;
            GenerateGeometry(x, y, width, height, pointsCount);
        }

        private void GenerateGeometry(float x, float y, float width, float height, int pointsCount)
        {
            int vertexCount = 2 * pointsCount;
            VertexPositionColor[] vertices = new VertexPositionColor[vertexCount];
            short[] indices = new short[vertexCount];

            float dx = width / (pointsCount - 1);
            Vector3 upperPosition = new Vector3(x, y, 0);
            Vector3 lowerPosition = new Vector3(x, y + height, 0);            

            int vertexIndex = 0;
            for (int pointIndex = 0; pointIndex < pointsCount; pointIndex++)
            {       
                // lower vertices;
                vertices[vertexIndex] = new VertexPositionColor(lowerPosition, Color.White);
                indices[vertexIndex] = (short)vertexIndex;
                vertexIndex++;

                // upper vertices;
                vertices[vertexIndex] = new VertexPositionColor(upperPosition, Color.White);
                indices[vertexIndex] = (short)vertexIndex;
                vertexIndex++;

                // shift position
                upperPosition.X += dx;
                lowerPosition.X += dx;
            }

            SetData(vertices, indices, PrimitiveType.TriangleStrip, vertexCount - 2);
        }                      

        //private void GenerateGeometry(float x, float y, float width, float height, int pointsCount)
        //{            
        //    int vertexCount = pointsCount * 2;
        //    VertexPositionColor[] vertices = new VertexPositionColor[vertexCount];
        //    short[] indices = new short[vertexCount];

        //    float dx = (float)(width - 1) / (pointsCount - 1);
        //    Vector3 upperPosition = new Vector3(x, y, 0);
        //    Vector3 lowerPosition = new Vector3(x, y + height, 0);
        //    int vertexIndex = 0;
        //    for (int pointIndex = 0; pointIndex < pointsCount; pointIndex++)
        //    {
        //        // lower vertices
        //        vertices[vertexIndex] = new VertexPositionColor(lowerPosition, Color.White);
        //        indices[vertexIndex] = (short)vertexIndex;
        //        vertexIndex++;

        //        // upper vertices;
        //        vertices[vertexIndex] = new VertexPositionColor(upperPosition, Color.White);
        //        indices[vertexIndex] = (short)vertexIndex;
        //        vertexIndex++;

        //        // shift position
        //        upperPosition.X += dx;
        //        lowerPosition.X += dx;
        //    }

        //    SetData(vertices, indices, PrimitiveType.TriangleStrip, vertexCount - 2);
        //}                      

        public void Update(float dt)
        {
            elapsed += dt;
            UpdateGeometry(elapsed);
        }

        private void UpdateGeometry(float t)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[pointsCount];
            short[] indices = new short[pointsCount];

            Vector3 position = new Vector3();
            float periodLength = 400.0f;
            float dx = periodLength / pointsCount;
            float lambda = Application.Instance.Width;
            double omega = 2*Math.PI;

            for (int pointIndex = 0; pointIndex < pointsCount; pointIndex++)
            {
                double a = 50.0f;
                position.Y = (float) (a * Math.Sin(2 * Math.PI * position.X / lambda - omega * t)) + 200;
                vertices[pointIndex] = new VertexPositionColor(position, Color.White);
                indices[pointIndex] = (short)pointIndex;

                position.X += dx;
            }

            SetData(vertices, indices, PrimitiveType.LineStrip, vertices.Length - 2);
        }
    }
}
