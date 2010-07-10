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
        private Effect effect;
        private Matrix translate;
        private Matrix rotate;
        private float rotation;
        private Vector2 position;
        private float speed;
        Vector4 upperColor;
        Vector4 lowerColor;
        float width;
        float height;

        public RadialSky(float width, float height, int raysCount, Color upperColor, Color lowerColor) : base(width, height, upperColor, lowerColor)
        {
            this.position = new Vector2(width / 2, height / 2);
            float radius = (float)Math.Sqrt(width * width + height * height);
            raysPrimitive = InitializeRaysGeometry(radius, raysCount);
            circle = new Circle(Vector2.Zero, 20.0f, 100);
            effect = Resources.GetEffect(Res.EFFECT_RADIAL);
            translate = Matrix.CreateTranslation(position.X, position.Y, 0.0f);

            this.upperColor = upperColor.ToVector4();
            this.lowerColor = lowerColor.ToVector4();
        }

        private Primitive InitializeRaysGeometry(float radius, int raysCount)
        {
            int verticesCount = 1 + raysCount * 2;
            int indicesCount = raysCount * 3;

            VertexPositionColor[] vertices = new VertexPositionColor[verticesCount];
            short[] indices = new short[indicesCount];

            // center
            Vector3 position = Vector3.Zero;
            vertices[0] = new VertexPositionColor(position, Color.White);            

            // rays
            float da = MathHelper.TwoPi / (2 * raysCount);
            float angle = -da / 2 - MathHelper.PiOver2;            
            for (int vertexIndex = 1; vertexIndex < verticesCount; vertexIndex++)
            {
                position.X = (float)(radius * Math.Cos(angle));
                position.Y = (float)(radius * Math.Sin(angle));
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
            rotation += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            Matrix.CreateRotationZ(rotation, out rotate);           
        }

        public override void Draw(RenderContext context)
        {
            base.Draw(context);

            Matrix transform = context.BasicEffect.World;
            transform = Matrix.Multiply(transform, rotate);
            transform = Matrix.Multiply(transform, translate);

            effect.Parameters["World"].SetValue(transform);
            effect.Parameters["View"].SetValue(context.BasicEffect.View);
            effect.Parameters["Projection"].SetValue(context.BasicEffect.Projection);
            effect.Parameters["UpperColor"].SetValue(upperColor);
            effect.Parameters["LowerColor"].SetValue(lowerColor);
            effect.Parameters["Height"].SetValue(height);

            raysPrimitive.Draw(effect);
            circle.Draw(effect);
        }
    }
}
