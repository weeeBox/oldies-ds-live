using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Collections.Generic;
using System.IO;

namespace ContentPipeline
{
    public struct CharInfo
    {
        public char chr;
        public int x;
        public int y;
        public int w;
        public int h;

        public CharInfo(char chr, int x, int y, int w, int h)
        {
            this.chr = chr;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }
    }

    public class FontInfo
    {
        private int charOffset;
        private int lineOffset;
        private int spaceWidth;
        private string sourceName;

        private List<CharInfo> chars;

        public FontInfo(string sourcePath)
        {
            this.sourceName = sourcePath;
            chars = new List<CharInfo>();
        }

        public void addCharInfo(CharInfo e)
        {
            chars.Add(e);
        }

        public List<CharInfo> Chars
        {
            get { return chars; }
        }

        public string SourceName
        {
            get { return sourceName; }
        }

        public int LineOffset
        {
            get { return lineOffset; }
            set { lineOffset = value; }
        }

        public int CharOffset
        {
            get { return charOffset; }
            set { charOffset = value; }
        }

        public int CharsCount
        {
            get { return chars.Count; }
        }

        public int SpaceWidth
        {
            get { return spaceWidth; }
            set { spaceWidth = value; }
        }
    }   
}
