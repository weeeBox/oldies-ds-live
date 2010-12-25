using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace DuckstazyLive.app
{
    public struct ResourceBaseInfo
    {
        public int resId;
        public ResourceType resType;
        public string filename;

        public ResourceBaseInfo(int resId, ResourceType resType, String filename)
        {
            this.resId = resId;
            this.resType = resType;
            this.filename = filename;
        }
    }

    public class DuckstazyResourceMgr : ResourceMgr
    {
        private static FontCharInfo[] FONT_BIG_INFO = 
        {	        
            new FontCharInfo(1, 0, 0, 4, 18, 45),
            new FontCharInfo(19, 0, 0, 6, 22, 24),
            new FontCharInfo(41, 0, 0, 7, 30, 42),
            new FontCharInfo(71, 0, 0, 3, 24, 49),
            new FontCharInfo(95, 0, 0, 4, 37, 48),
            new FontCharInfo(132, 0, 0, 6, 35, 42),
            new FontCharInfo(167, 0, 0, 6, 13, 24),
            new FontCharInfo(180, 0, 0, 6, 16, 46),
            new FontCharInfo(196, 0, 0, 3, 18, 46),
            new FontCharInfo(214, 0, 0, 3, 22, 24),
            new FontCharInfo(0, 49, 0, 12, 33, 31),
            new FontCharInfo(33, 49, 0, 34, 15, 20),
            new FontCharInfo(48, 49, 0, 27, 20, 12),
            new FontCharInfo(68, 49, 0, 34, 10, 14),
            new FontCharInfo(78, 49, 0, 0, 27, 48),
            new FontCharInfo(105, 49, 0, 3, 27, 45),
            new FontCharInfo(132, 49, 0, 6, 24, 45),
            new FontCharInfo(156, 49, 0, 5, 28, 43),
            new FontCharInfo(184, 49, 0, 5, 27, 45),
            new FontCharInfo(211, 49, 0, 3, 28, 50),
            new FontCharInfo(0, 99, 0, 5, 28, 43),
            new FontCharInfo(28, 99, 0, 2, 27, 46),
            new FontCharInfo(55, 99, 0, 2, 27, 48),
            new FontCharInfo(82, 99, 0, 0, 27, 47),
            new FontCharInfo(109, 99, 0, 0, 28, 48),
            new FontCharInfo(137, 99, 0, 21, 13, 26),
            new FontCharInfo(150, 99, 0, 22, 16, 32),
            new FontCharInfo(166, 99, 0, 7, 30, 38),
            new FontCharInfo(196, 99, 0, 18, 33, 19),
            new FontCharInfo(0, 147, 0, 7, 30, 38),
            new FontCharInfo(30, 147, 0, 4, 22, 44),
            new FontCharInfo(52, 147, 0, 4, 42, 44),
            new FontCharInfo(94, 147, 0, 1, 31, 48),
            new FontCharInfo(125, 147, 0, 3, 31, 46),
            new FontCharInfo(156, 147, 0, 3, 33, 45),
            new FontCharInfo(189, 147, 0, 3, 32, 46),
            new FontCharInfo(221, 147, 0, 1, 29, 47),
            new FontCharInfo(0, 195, 0, 3, 27, 48),
            new FontCharInfo(27, 195, 0, 4, 33, 50),
            new FontCharInfo(60, 195, 0, 3, 33, 48),
            new FontCharInfo(93, 195, 0, 3, 17, 45),
            new FontCharInfo(110, 195, 0, 3, 25, 43),
            new FontCharInfo(135, 195, 0, 1, 30, 45),
            new FontCharInfo(165, 195, 0, 3, 23, 45),
            new FontCharInfo(188, 195, 0, 2, 38, 45),
            new FontCharInfo(0, 245, 0, 2, 34, 48),
            new FontCharInfo(34, 245, 0, 5, 34, 42),
            new FontCharInfo(68, 245, 0, 2, 28, 48),
            new FontCharInfo(96, 245, 0, 4, 33, 49),
            new FontCharInfo(129, 245, 0, 4, 33, 46),
            new FontCharInfo(162, 245, 0, 2, 25, 48),
            new FontCharInfo(187, 245, 0, 2, 25, 47),
            new FontCharInfo(212, 245, 0, 2, 32, 45),
            new FontCharInfo(0, 294, 0, 4, 29, 47),
            new FontCharInfo(29, 294, 0, 4, 38, 45),
            new FontCharInfo(67, 294, 0, 3, 31, 48),
            new FontCharInfo(98, 294, 0, 6, 28, 45),
            new FontCharInfo(126, 294, 0, 4, 32, 47),
            new FontCharInfo(158, 294, 0, 4, 23, 48),
            new FontCharInfo(181, 294, 0, 3, 27, 48),
            new FontCharInfo(208, 294, 0, 4, 24, 48),
            new FontCharInfo(0, 342, 0, 6, 38, 31),
            new FontCharInfo(38, 342, 0, 44, 33, 11),
            new FontCharInfo(71, 342, 0, 9, 16, 15),
            new FontCharInfo(87, 342, 0, 0, 26, 48),
            new FontCharInfo(113, 342, 0, 3, 12, 52),
            new FontCharInfo(125, 342, 0, 0, 25, 48),
            new FontCharInfo(150, 342, 0, 26, 25, 13),
            new FontCharInfo(175, 342, 0, -8, 20, 1),
        };

        public DuckstazyResourceMgr(ContentManager cm)
            : base(cm)
        {
        }

        public int getPacksCount()
        {
            return Packs.PACKS_COUNT;
        }

        public override int getCapacity()
        {
            return Res.RES_COUNT;
        }

        public void addPackToLoad(int n)
        {
            Debug.Assert(n >= 0 && n < DuckstazyResources.RESOURCES_PACKS.Length);

            ResourceBaseInfo[] pack = DuckstazyResources.RESOURCES_PACKS[n];
            for (int resIndex = 0; resIndex < pack.Length; ++resIndex)
            {
                object[] resParams = null;
                int resId = pack[resIndex].resId;

                if (resId == Res.FNT_BIG)
                {
                    resParams = new Object[(int)FontVariableParams.COUNT];
                    String chars = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`{|}~ ";
                    int len = chars.Length;
                    FontCharInfo[] data = new FontCharInfo[len];
                    for (int j = 0; j < len; j++)
                    {
                        data[j] = FONT_BIG_INFO[j];
                    }
                    resParams[(int)FontVariableParams.FONT_VARIABLE_PARAM_CHARS] = chars;
                    resParams[(int)FontVariableParams.FONT_VARIABLE_PARAM_DATA] = data;
                    resParams[(int)FontVariableParams.FONT_VARIABLE_PARAM_CHAR_OFFSET] = -5;
                    resParams[(int)FontVariableParams.FONT_VARIABLE_PARAM_LINE_OFFSET] = -5; 
                }
                
                ResourceType resType = pack[resIndex].resType;
                string filename = pack[resIndex].filename;
                addResourceToLoadQueue(filename, resType, (int)resId, resParams);
            }
        }

        public void freePack(int n)
        {
            Debug.Assert(n >= 0 && n < DuckstazyResources.RESOURCES_PACKS.Length);

            ResourceBaseInfo[] pack = DuckstazyResources.RESOURCES_PACKS[n];
            for (int resIndex = 0; resIndex < pack.Length; ++resIndex)
            {
                freeResource((int)pack[resIndex].resId);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void freeAll()
        {
            for (int packIndex = 0; packIndex < getPacksCount(); packIndex++)
            {
                freePack(packIndex);
            }
        }
    }
}
