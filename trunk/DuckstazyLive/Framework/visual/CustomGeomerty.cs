using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Framework.visual
{
    public class CustomGeomerty : BaseElement
    {        
        private VertexDeclaration vertexDeclaration;
        private VertexPositionColor[] vertexData;
        private short[] indexData;
        int primitiveCount;
        PrimitiveType primitiveType;
        
        public CustomGeomerty(Vector2[] vertices, Color vertexColor, PrimitiveType primitiveType)
        {
            throw new NotImplementedException();            
        }

        public CustomGeomerty(VertexPositionColor[] vertexData, PrimitiveType primitiveType)             
        {
            init(vertexData, null, primitiveType);
        }

        public CustomGeomerty(VertexPositionColor[] vertexData, short[] indexData, PrimitiveType primitiveType)
        {
            init(vertexData, indexData, primitiveType);
        }

        private void init(VertexPositionColor[] vertexData, short[] indexData, PrimitiveType primitiveType)
        {
            this.vertexData = vertexData;
            this.indexData = indexData;            
            this.vertexDeclaration = VertexPositionColor.VertexDeclaration;
            this.primitiveType = primitiveType;
            primitiveCount = getPrimitiveCount(primitiveType, indexData == null ? vertexData.Length : indexData.Length);
        }

        private int getPrimitiveCount(PrimitiveType type, int indicesCount)
        {
            switch (type)
            {
                case PrimitiveType.TriangleStrip:
                    {
                        return indicesCount - 2;
                    }
                default:
                    throw new NotImplementedException();                    
            }            
        }

        public override void draw()
        {
            preDraw();

            AppGraphics.DrawGeomerty(this);

            postDraw();
        }

        public void colorize(Color color)
        {
            for (int i = 0; i < vertexData.Length; ++i)
            {
                vertexData[i].Color = color;
            }
        }

        public VertexDeclaration VertexDeclaration
        {
            get { return vertexDeclaration; }
        }

        public PrimitiveType PrimitiveType
        {
            get { return primitiveType; }
        }

        public VertexPositionColor[] VertexData
        {
            get { return vertexData; }
        }       

        public short[] IndexData
        {
            get { return indexData; }
        }

        public int PrimitiveCount
        {
            get { return primitiveCount; }
        }
    }
}
