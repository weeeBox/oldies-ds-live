using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Framework.core;

namespace Framework.visual
{
    public class Font : Image
    {
        public int charOffset;
        public int lineOffset;
        String chars;
        Dictionary<char, int> charMap;

        public Font(String chars, Texture2D texture)
            : base(texture, chars.Length)
        {
            this.chars = chars;
            charOffset = 0;
            lineOffset = 0;
            createCharMap();
        }

        public Font(String chars, Texture2D texture, int itemWidth, int itemHeight)
            : base(texture, itemWidth, itemHeight, false)
        {
            this.chars = chars;
            charOffset = 0;
            lineOffset = 0;
            createCharMap();
        }

        private void createCharMap()
        {
            charMap = new Dictionary<char, int>();
            for (int i = 0; i < chars.Length; i++)
            {
                if (charMap.ContainsKey(chars[i]))
                {
                    char c1 = chars[i];
                    char c2 = chars[i];
                }
                else
                {
                    charMap.Add(chars[i], i);
                }
            }
        }

        public int getCharQuad(char c)
        {
            if (charMap.ContainsKey(c))
                return charMap[c];
            else
                return Constants.UNDEFINED;
        }

        public int stringWidth(String s)
        {
            int strWidth = 0;
            int len = s.Length;
            for (int c = 0; c < len; c++)
            {
                int quadIndex = getCharQuad(s[c]);
                int itemWidth = quads[quadIndex].Width;

                strWidth += itemWidth + charOffset;
            }
            strWidth -= charOffset;
            return strWidth;
        }

        public void setOffsets(int charOffset, int lineOffset)
        {
            this.charOffset = charOffset;
            this.lineOffset = lineOffset;
        }

        public int fontHeight()
        {
            return quads[0].Height;
        }
    }
}
