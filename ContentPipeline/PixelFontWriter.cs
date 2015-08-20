using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace ContentPipeline
{
    [ContentTypeWriter]
    public class PixelFontWriter : ContentTypeWriter<PixelFontInfo>
    {
        protected override void Write(ContentWriter output, PixelFontInfo value)
        {
            output.Write(value.SourceName);

            output.Write(value.CharOffset);
            output.Write(value.LineOffset);
            output.Write(value.SpaceWidth);
            output.Write(value.FontOffset);
            
            output.Write(value.CharsCount);
            
            List<CharInfo> chars = value.Chars;
            foreach (CharInfo c in chars)
            {
                output.Write(c.chr);
                output.Write(c.x);
                output.Write(c.y);
                output.Write(c.w);
                output.Write(c.h);
                output.Write(c.ox);
                output.Write(c.oy);
            }            
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(PixelFontInfo).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            // TODO: change this to the name of your ContentTypeReader
            // class which will be used to load this data.
            return "Framework.utils.PixelFontReader, Framework," + " Version=1.0.0.0, Culture=neutral";
        }
    }
}
