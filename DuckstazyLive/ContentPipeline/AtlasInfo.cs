using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentPipeline
{
    public struct AtlasImageInfo
    {
        public int x;
        public int y;
        public int w;
        public int h;
        public int ox;
        public int oy;

        public AtlasImageInfo(int x, int y, int w, int h, int ox, int oy)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.ox = ox;
            this.oy = oy;
        }
    }

    public class AtlasInfo
    {
        private List<AtlasImageInfo> images;
        private String filename;

        public AtlasInfo(String filename)
        {
            this.filename = filename;
            images = new List<AtlasImageInfo>();
        }

        public void addInfo(AtlasImageInfo info)
        {
            images.Add(info);
        }

        public int ImagesCount
        {
            get { return images.Count; }
        }

        public List<AtlasImageInfo> Images
        {
            get { return images; }
        }

        public String Filename
        {
            get { return filename; }
        }
    }
}
