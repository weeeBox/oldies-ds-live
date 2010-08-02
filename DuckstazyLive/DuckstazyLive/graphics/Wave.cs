using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DuckstazyLive.framework.graphics;

namespace DuckstazyLive.graphics
{
    class Wave
    {
        private readonly int pointsCount = 100;

        private float elapsed;                
        private Primitive wave;

        public Wave(float x, float y, float width, float height)
        {            
            wave = GenerateGeometry(x, y, width, height, pointsCount);
        }

        private Primitive GenerateGeometry(float x, float y, float width, float height, int pointsCount)
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

            Primitive primitive = new Primitive();
            primitive.SetData(vertices, indices, PrimitiveType.TriangleStrip, vertexCount - 2);

            return primitive;
        }                      
          
        public void Update(float dt)
        {
            elapsed += dt;        
        }             

        public void Draw(GameGraphics g)
        {
            Effect customEffect = Resources.GetEffect(Res.EFFECT_WAVE);
            customEffect.Parameters["World"].SetValue(g.WorldMatrix);
            customEffect.Parameters["View"].SetValue(g.ViewMatrix);
            customEffect.Parameters["Projection"].SetValue(g.ProjectionMatrix);
            customEffect.Parameters["Timer"].SetValue(elapsed);            
            customEffect.Parameters["Amplitude"].SetValue(20.0f);
            customEffect.Parameters["WaveLength"].SetValue(Application.Instance.Width);
            customEffect.Parameters["Omega"].SetValue(4 * MathHelper.Pi);
            customEffect.Parameters["Top"].SetValue(0);
            customEffect.Parameters["Phase"].SetValue(0.3f);
            customEffect.Parameters["Color"].SetValue(new Color(93, 49, 12).ToVector4());
            wave.Draw(g, customEffect);            
        }        
    }
}
