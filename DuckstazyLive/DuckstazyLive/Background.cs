using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive
{
    public class Background
    {
        private VertexDeclaration vertexDeclaration;
        private BasicEffect basicEffect;
        private VertexBuffer vertexBuffer;

        private VertexPositionColor[] vertices;
        private short[] indices;
        private static Color SKY_UPPER_COLOR = new Color(63, 181, 242);
        private static Color SKY_LOWER_COLOR = new Color(221, 242, 255);

        public Background(GraphicsDevice device, ref Matrix worldMatrix, ref Matrix viewMatrix, ref Matrix projectionMatrix)
        {
            float width = device.Viewport.Width;
            float height = device.Viewport.Height;

            vertices = new VertexPositionColor[4];
            vertices[0] = new VertexPositionColor(new Vector3(0.0f, height, 0.0f), SKY_LOWER_COLOR);
            vertices[1] = new VertexPositionColor(new Vector3(0.0f, 0.0f, 0.0f), SKY_UPPER_COLOR);
            vertices[2] = new VertexPositionColor(new Vector3(width, height, 0.0f), SKY_LOWER_COLOR);
            vertices[3] = new VertexPositionColor(new Vector3(width, 0.0f, 0.0f), SKY_UPPER_COLOR);

            indices = new short[4] { 0, 1, 2, 3};

            vertexDeclaration = new VertexDeclaration(device, VertexPositionColor.VertexElements);
            basicEffect = new BasicEffect(device, null);
            basicEffect.VertexColorEnabled = true;
            basicEffect.World = worldMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.Projection = projectionMatrix;            

            vertexBuffer = new VertexBuffer(device, VertexPositionColor.SizeInBytes * vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
        }

        public void Draw(GraphicsDevice device)
        {
            device.VertexDeclaration = vertexDeclaration;
            basicEffect.Begin();

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();
                device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, 0, vertices.Length, indices, 0, indices.Length - 2);
                pass.End();
            }    

            basicEffect.End();
        }
    }
}
