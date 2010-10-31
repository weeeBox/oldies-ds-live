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
            new FontCharInfo(1,0,20,12,18,45),
            new FontCharInfo(19,0,26,14,22,24),
            new FontCharInfo(41,0,19,15,30,42),
            new FontCharInfo(71,0,19,11,24,49),
            new FontCharInfo(95,0,15,12,37,48),
            new FontCharInfo(132,0,20,14,35,42),
            new FontCharInfo(167,0,27,14,13,24),
            new FontCharInfo(180,0,22,14,16,46),
            new FontCharInfo(196,0,17,11,18,46),
            new FontCharInfo(214,0,20,11,22,24),
            new FontCharInfo(0,49,20,20,33,31),
            new FontCharInfo(33,49,19,42,15,20),
            new FontCharInfo(48,49,17,35,20,12),
            new FontCharInfo(68,49,18,42,10,14),
            new FontCharInfo(78,49,20,8,27,48),
            new FontCharInfo(105,49,22,11,27,45),
            new FontCharInfo(132,49,19,14,24,45),
            new FontCharInfo(156,49,16,13,28,43),
            new FontCharInfo(184,49,15,13,27,45),
            new FontCharInfo(211,49,20,11,28,50),
            new FontCharInfo(0,99,19,13,28,43),
            new FontCharInfo(28,99,19,10,27,46),
            new FontCharInfo(55,99,12,10,27,48),
            new FontCharInfo(82,99,22,8,27,47),
            new FontCharInfo(109,99,21,8,28,48),
            new FontCharInfo(137,99,22,29,13,26),
            new FontCharInfo(150,99,16,30,16,32),
            new FontCharInfo(166,99,18,15,30,38),
            new FontCharInfo(196,99,20,26,33,19),
            new FontCharInfo(0,147,20,15,30,38),
            new FontCharInfo(30,147,22,12,22,44),
            new FontCharInfo(52,147,16,12,42,44),
            new FontCharInfo(94,147,16,9,31,48),
            new FontCharInfo(125,147,21,11,31,46),
            new FontCharInfo(156,147,19,11,33,45),
            new FontCharInfo(189,147,17,11,32,46),
            new FontCharInfo(221,147,18,9,29,47),
            new FontCharInfo(0,195,20,11,27,48),
            new FontCharInfo(27,195,19,12,33,50),
            new FontCharInfo(60,195,20,11,33,48),
            new FontCharInfo(93,195,16,11,17,45),
            new FontCharInfo(110,195,16,11,25,43),
            new FontCharInfo(135,195,22,9,30,45),
            new FontCharInfo(165,195,20,11,23,45),
            new FontCharInfo(188,195,17,10,38,45),
            new FontCharInfo(0,245,17,10,34,48),
            new FontCharInfo(34,245,20,13,34,42),
            new FontCharInfo(68,245,19,10,28,48),
            new FontCharInfo(96,245,19,12,33,49),
            new FontCharInfo(129,245,15,12,33,46),
            new FontCharInfo(162,245,19,10,25,48),
            new FontCharInfo(187,245,18,10,25,47),
            new FontCharInfo(212,245,20,10,32,45),
            new FontCharInfo(0,294,17,12,29,47),
            new FontCharInfo(29,294,18,12,38,45),
            new FontCharInfo(67,294,17,11,31,48),
            new FontCharInfo(98,294,19,14,28,45),
            new FontCharInfo(126,294,17,12,32,47),
            new FontCharInfo(158,294,19,12,23,48),
            new FontCharInfo(181,294,17,11,27,48),
            new FontCharInfo(208,294,19,12,24,48),
            new FontCharInfo(0,342,16,14,38,31),
            new FontCharInfo(38,342,16,52,33,11),
            new FontCharInfo(71,342,26,17,16,15),
            new FontCharInfo(87,342,21,8,26,48),
            new FontCharInfo(113,342,29,11,12,52),
            new FontCharInfo(125,342,19,8,25,48),
            new FontCharInfo(150,342,24,34,25,13),            
            new FontCharInfo(175,342,0,0,20,1),
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
                    resParams[(int)FontVariableParams.FONT_VARIABLE_PARAM_CHAR_OFFSET] = -2;
                    resParams[(int)FontVariableParams.FONT_VARIABLE_PARAM_LINE_OFFSET] = 0; 
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
