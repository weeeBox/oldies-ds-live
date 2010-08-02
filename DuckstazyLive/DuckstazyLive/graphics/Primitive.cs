using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.framework.graphics;

namespace DuckstazyLive.graphics
{
    public class Primitive
    {
        private VertexPositionColor[] vertices;
        private short[] indices;
        private VertexDeclaration vertexDeclaration;        
        private PrimitiveType primitiveType;
        private int primitivesCount;

        public Primitive()
        {
            vertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);            
        }

        public void SetData(VertexPositionColor[] vertices, short[] indices, PrimitiveType type, int primitivesCount)
        {
            this.vertices = vertices;
            this.indices = indices;
            this.primitiveType = type;
            this.primitivesCount = primitivesCount;
        }

        public void Draw(GameGraphics g)
        {
            g.BeginBasicEffect();
            g.GraphicsDevice.VertexDeclaration = vertexDeclaration;
            g.GraphicsDevice.DrawUserIndexedPrimitives(primitiveType, vertices, 0, vertices.Length, indices, 0, primitivesCount);
        }              

        public void Draw(GameGraphics g, Effect effect)
        {
            GraphicsDevice.VertexDeclaration = vertexDeclaration;

            g.BeginEffect(effect);                        
            GraphicsDevice.DrawUserIndexedPrimitives(primitiveType, vertices, 0, vertices.Length, indices, 0, primitivesCount);            
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
