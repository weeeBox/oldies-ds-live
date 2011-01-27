using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Framework.core;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Framework.visual
{
    public class PixelFont : Image, Font
    {
        private int charOffset;
        private int lineOffset;
        private int fontOffset;
        private int spaceWidth;

        Dictionary<char, int> charMap;

        private char[] chars;
        private int[] quadOffsetX;
        private int[] quadOffsetY;        

        public PixelFont(Texture2D texture, int charsCount)
            : base(texture, charsCount + 1)
        {
            quadOffsetX = new int[charsCount + 1];
            quadOffsetY = new int[charsCount + 1];
            chars = new char[charsCount + 1];            
            
            charOffset = 0;
            lineOffset = 0;            
        }        

        public void createCharMap()
        {
            // add space
            setCharInfo(chars.Length - 1, ' ', 0, 0, spaceWidth, 1, 0, 0);

            charMap = new Dictionary<char, int>();
            for (int i = 0; i < chars.Length; i++)
            {
                if (!charMap.ContainsKey(chars[i]))
                {                    
                    charMap.Add(chars[i], i);
                }
            } 
        }

        public int getCharQuad(char c)
        {
            if (charMap.ContainsKey(c))
                return charMap[c];

            c = Char.IsLower(c) ? Char.ToUpper(c) : Char.ToLower(c);
            if (charMap.ContainsKey(c))
                return charMap[c];

            return FrameworkConstants.UNDEFINED;
        }

        public int getCharWidth(char c)
        {
            int quadIndex = getCharQuad(c);            
            int charWidth = quads[quadIndex].Width;
            return charWidth;
        }

        public void drawString(String str, float x, float y)
        {
            drawString(str, x, y, TextAlign.LEFT | TextAlign.TOP);
        }

        public void drawString(String str, float x, float y, TextAlign textAlign)
        {
            Debug.Assert(str != null);

            float dx = x;
            float dy = y;

            if ((textAlign & TextAlign.RIGHT) != 0)
            {
                dx -= stringWidth(str);
            }
            else if ((textAlign & TextAlign.HCENTER) != 0)
            {
                dx -= 0.5f * stringWidth(str);
            }
            if ((textAlign & TextAlign.BOTTOM) != 0)
            {
                dy -= fontHeight();
            }
            else if ((textAlign & TextAlign.VCENTER) != 0)
            {
                dy -= 0.5f * fontHeight();
            }

            for (int charIndex = 0; charIndex < str.Length; charIndex++)
            {
                char c = str[charIndex];
                int quadIndex = getCharQuad(c);
                if (quadIndex == -1)
                {
                    c = '?';
                    quadIndex = getCharQuad(c);
                    Debug.Assert(quadIndex != -1, "No '?' in font");
                }
                int itemWidth = quads[quadIndex].Width;

                drawQuad(quadIndex, dx, dy);
                dx += itemWidth + charOffset;
            }
        }

        public override void drawQuad(int n, float x, float y)
        {
            int offX = quadOffsetX[n];
            int offY = quadOffsetY[n];
            base.drawQuad(n, x + offX, y + offY);
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

        public void setOffsets(int charOffset, int lineOffset, int fontOffset)
        {
            this.charOffset = charOffset;
            this.lineOffset = lineOffset;
            this.fontOffset = fontOffset;
        }

        public void setSpaceWidth(int spaceWidth)
        {
            this.spaceWidth = spaceWidth;
        }

        public int fontHeight()
        {
            return quads[0].Height;
        }

        public void setCharInfo(int pos, char c, int x, int y, int w, int h, int ox, int oy)
        {
            Rectangle rect = new Rectangle(x, y, w, h);
            setQuad(rect, pos);
            chars[pos] = c;
            quadOffsetX[pos] = ox;
            quadOffsetY[pos] = oy + fontOffset;
        }

        public String[] wrapString(String text, int wrapWidth)
        {
            return wrapString(text, wrapWidth, 200);
        }

        public String[] wrapString(String text, int wrapWidth, int idxBufferSize)
        {
            int strLen = text.Length;
            int dataIndex = 0; // индекс текущего элемента в возвращаемом массиве
            int xc = 0;
            int wordWidth = 0; // ширина текущего слова
            int strStartIndex = 0; // индекс начала текущей строки
            int wordLastCharIndex = 0; // индекс последнего символа текущего слова
            int stringWidth = 0; // ширина текущей строки
            int charIndex = 0; // индекс рассматриваемого символа
            short[] strIdx = new short[idxBufferSize];
            while (charIndex < strLen)
            {
                int curCharIndex = charIndex;
                char curChar = text[curCharIndex];
                charIndex++;

                if (curChar == ' ' || curChar == '\n')
                {
                    wordLastCharIndex = curCharIndex; // запоминаем end-позицию для substring
                    if (stringWidth == 0 && wordWidth > 0)
                        wordWidth -= charOffset;

                    stringWidth += wordWidth;
                    wordWidth = 0;
                    xc = charIndex;

                    if (curChar == ' ')
                    {
                        xc--;
                        wordWidth = getCharWidth(curChar) + charOffset;
                    }
                }
                else
                {
                    wordWidth += getCharWidth(curChar) + charOffset;
                }

                if ((stringWidth + wordWidth) > wrapWidth && wordLastCharIndex != strStartIndex || curChar == '\n')
                {
                    strIdx[dataIndex++] = (short)strStartIndex;
                    strIdx[dataIndex++] = (short)wordLastCharIndex;

                    char tempChar;
                    while (xc < text.Length && (tempChar = text[xc]) == ' ')
                    {
                        wordWidth -= getCharWidth(tempChar) + charOffset;
                        xc++;
                    }
                    wordWidth -= charOffset;

                    strStartIndex = xc;
                    wordLastCharIndex = strStartIndex;
                    stringWidth = 0;
                }
            }

            if (wordWidth != 0)
            {
                strIdx[dataIndex++] = (short)strStartIndex;
                strIdx[dataIndex++] = (short)strLen;
            }
            
            int strCount = dataIndex / 2;
            String[] strings = new String[strCount];
            for (int i = 0; i < strCount; i++)
            {
                int index = 2 * i;
                int start = strIdx[index];
                int end = strIdx[index + 1];

                strings[i] = text.Substring(start, end - start);                
            }

            return strings;
        }

        public int LineOffset
        {
            get { return lineOffset; }
        }

        public int CharOffset
        {
            get { return charOffset; }
        }
    }    
}
