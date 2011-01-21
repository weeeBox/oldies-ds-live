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
    [ContentProcessor(DisplayName = "Pixel Font Processor")]
    public class PixelFontProcessor : ContentProcessor<PixelFontInfo, PixelFontInfo>
    {
        public override PixelFontInfo Process(PixelFontInfo input, ContentProcessorContext context)
        {
            return input;
        }
    }
}