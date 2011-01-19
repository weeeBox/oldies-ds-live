using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Framework.visual;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Framework.utils
{
    public class FontReader : ContentTypeReader<Font>
    {
        private static GraphicsDevice graphicsDevice;

        protected override Font Read(ContentReader input, Font existingInstance)
        {
            string textureName = input.ReadString();
            Texture2D texture = input.ContentManager.Load<Texture2D>(textureName);

            int charOffset = input.ReadInt32();
            int lineOffset = input.ReadInt32();
            int spaceWidth = input.ReadInt32();
            
            int charsCount = input.ReadInt32();           

            Font font = new Font(texture, charsCount);
            font.setOffsets(charOffset, lineOffset);
            font.setSpaceWidth(spaceWidth);

            for (int charIndex = 0; charIndex < charsCount; ++charIndex)
            {                
                char chr = input.ReadChar();
                int x = input.ReadInt32();
                int y = input.ReadInt32();
                int w = input.ReadInt32();
                int h = input.ReadInt32();
                                
                font.setCharInfo(charIndex, chr, x, y, w, h);
                font.createCharMap();
            }            

            return font;
        }

        public static GraphicsDevice GraphicsDevice
        {
            set { graphicsDevice = value; }
        }
    }
}
