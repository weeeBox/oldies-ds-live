using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.graphics
{
    public class Primitive
    {
        private VertexPositionColor[] vertices;
        private short[] indices;
        private VertexDeclaration vertexDeclaration;        
        private PrimitiveType primitiveType;
        private int primitivesCount;

        public Primitive(VertexPositionColor[] vertices, short[] indices, PrimitiveType type, int primitivesCount)
        {            
            this.vertices = vertices;
            this.indices = indices;
            this.primitiveType = type;
            this.primitivesCount = primitivesCount;

            vertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);            
        }

        public void Draw(BasicEffect effect)
        {
            GraphicsDevice.VertexDeclaration = vertexDeclaration;

            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                GraphicsDevice.DrawUserIndexedPrimitives(primitiveType, vertices, 0, vertices.Length, indices, 0, primitivesCount);
                pass.End();
            }
            effect.End();
        }              

        protected VertexPositionColor[] Vertices
        {
            get { return vertices; }
        }

        protected short[] Indices
        {
            get { return indices; }
        }        

        private GraphicsDevice GraphicsDevice
        {
            get { return Application.Instance.GraphicsDevice; }
        }
    }
}
