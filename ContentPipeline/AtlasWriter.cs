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
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class AtlasWriter : ContentTypeWriter<AtlasInfo>
    {
        protected override void Write(ContentWriter output, AtlasInfo value)
        {
            output.Write(value.Filename);
            output.Write(value.ImagesCount);

            List<AtlasImageInfo> images = value.Images;
            foreach (AtlasImageInfo e in images)
            {                
                output.Write(e.x);
                output.Write(e.y);
                output.Write(e.w);
                output.Write(e.h);
                output.Write(e.ox);
                output.Write(e.oy);
            }
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(AtlasInfo).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            // TODO: change this to the name of your ContentTypeReader
            // class which will be used to load this data.
            return "Framework.utils.AtlasReader, Framework," + " Version=1.0.0.0, Culture=neutral";
        }
    }
}
