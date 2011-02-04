using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Framework.visual;
using Framework.core;

namespace Framework.utils
{    
    public class AtlasReader : ContentTypeReader<Atlas>
    {
        protected override Atlas Read(ContentReader input, Atlas existingInstance)
        {
            string textureName = input.ReadString();
            Texture2D texture = Application.sharedResourceMgr.loadTexture(input.ContentManager, textureName);            

            int imagesCount = input.ReadInt32();

            Atlas atlas = new Atlas(texture, imagesCount);
            for (int imageIndex = 0; imageIndex < imagesCount; ++imageIndex)
            {                
                int x = input.ReadInt32();
                int y = input.ReadInt32();
                int w = input.ReadInt32();
                int h = input.ReadInt32();
                int ox = input.ReadInt32();
                int oy = input.ReadInt32();
                atlas.setQuad(imageIndex, x, y, w, h, ox, oy);
            }

            return atlas;
        }
    }
}
