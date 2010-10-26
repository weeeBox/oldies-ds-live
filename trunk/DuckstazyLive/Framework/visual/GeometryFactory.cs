using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Framework.visual
{
    public class GeometryFactory
    {
        public static CustomGeomerty createGradient(float x, float y, float w, float h, Color upperColor, Color lowerColor)
        {
            VertexPositionColor[] vertexData = new VertexPositionColor[4];
            vertexData[0] = new VertexPositionColor(new Vector3(x,     y + h, 0), lowerColor);
            vertexData[1] = new VertexPositionColor(new Vector3(x,     y,     0), upperColor);
            vertexData[2] = new VertexPositionColor(new Vector3(x + w, y + h, 0), lowerColor);
            vertexData[3] = new VertexPositionColor(new Vector3(x + w, y,     0), upperColor);
            CustomGeomerty geomerty = new CustomGeomerty(vertexData, PrimitiveType.TriangleStrip);
            geomerty.x = x;
            geomerty.y = y;
            geomerty.width = (int)(w);
            geomerty.height = (int)(h);
            return geomerty;
        }

        public static CustomGeomerty createSolidRect(float x, float y, float w, float h, Color fillColor)
        {            
            return createGradient(x, y, w, h, fillColor, fillColor);
        }
    }
}
