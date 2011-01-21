using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using ContentPipeline;

using System.IO;
using System.Xml;

namespace ContentPipeline
{    
    [ContentImporter(".pixelfont", DisplayName = "Pixel Font Importer", DefaultProcessor = "PixelFontProcessor")]
    public class PixelFontImporter : ContentImporter<PixelFontInfo>
    {
        public override PixelFontInfo Import(string filename, ContentImporterContext context)
        {
            PixelFontInfo fontInfo = null;

            using (XmlTextReader reader = new XmlTextReader(File.Open(filename, FileMode.Open)))
            {
                while (reader.Read())
                {
                    string nodeName = reader.Name;
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                Dictionary<string, string> attributes = new Dictionary<string, string>();
                                while (reader.MoveToNextAttribute())
                                {
                                    attributes.Add(reader.Name, reader.Value);
                                }

                                if (nodeName == "font")
                                {
                                    string sourceFilename = attributes["filename"];
                                    int charOffset = int.Parse(attributes["charOffset"]);
                                    int lineOffset = int.Parse(attributes["lineOffset"]);
                                    int spaceWidth = int.Parse(attributes["spaceWidth"]);
                                    int fontOffset = int.Parse(attributes["fontOffset"]);

                                    int index = sourceFilename.LastIndexOf('.');
                                    string sourceName = index == -1 ? sourceFilename : sourceFilename.Substring(0, index);
                                    fontInfo = new PixelFontInfo(sourceName);
                                    fontInfo.LineOffset = lineOffset;
                                    fontInfo.CharOffset = charOffset;
                                    fontInfo.SpaceWidth = spaceWidth;
                                    fontInfo.FontOffset = fontOffset;
                                }
                                else if (nodeName == "char")
                                {
                                    char charValue = attributes["value"][0];
                                    int charX = int.Parse(attributes["x"]);
                                    int charY = int.Parse(attributes["y"]);
                                    int charWidth = int.Parse(attributes["w"]);
                                    int charHeight = int.Parse(attributes["h"]);
                                    int charOx = int.Parse(attributes["ox"]);
                                    int charOy = int.Parse(attributes["oy"]);

                                    CharInfo charInfo = new CharInfo(charValue, charX, charY, charWidth, charHeight, charOx, charOy);
                                    fontInfo.addCharInfo(charInfo);
                                }
                            }
                            break;
                    }
                }
            }            

            return fontInfo;
        }
    }
}
