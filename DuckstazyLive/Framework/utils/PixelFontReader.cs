using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Framework.visual;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Framework.core;

namespace Framework.utils
{
    public class PixelFontReader : ContentTypeReader<PixelFont>
    {
        protected override PixelFont Read(ContentReader input, PixelFont existingInstance)
        {
            string textureName = input.ReadString();
            Texture2D texture = Application.sharedResourceMgr.loadTexture(input.ContentManager, textureName);

            int charOffset = input.ReadInt32();
            int lineOffset = input.ReadInt32();
            int spaceWidth = input.ReadInt32();
            int fontOffset = input.ReadInt32();
            int charsCount = input.ReadInt32();            
            
            PixelFont font = new PixelFont(new SingleTexture(texture), charsCount);
            font.setOffsets(charOffset, lineOffset, fontOffset);
            font.setSpaceWidth(spaceWidth);            

            for (int charIndex = 0; charIndex < charsCount; ++charIndex)
            {                
                char chr = input.ReadChar();
                int x = input.ReadInt32();
                int y = input.ReadInt32();
                int w = input.ReadInt32();
                int h = input.ReadInt32();
                int ox = input.ReadInt32();
                int oy = input.ReadInt32();
                                
                font.setCharInfo(charIndex, chr, x, y, w, h, ox, oy);
                font.createCharMap();
            }            

            return font;
        }     
    }
}
