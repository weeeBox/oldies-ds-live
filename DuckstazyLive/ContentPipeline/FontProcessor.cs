using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace ContentPipeline
{
    [ContentProcessor(DisplayName = "FontProcessor")]
    public class FontProcessor : ContentProcessor<FontInfo, FontInfo>
    {
        public override FontInfo Process(FontInfo input, ContentProcessorContext context)
        {
            return input;
        }
    }
}