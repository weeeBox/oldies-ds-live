using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System.Xml;
using System.IO;

namespace ContentPipeline
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    /// 
    /// This should be part of a Content Pipeline Extension Library project.
    /// 
    /// TODO: change the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.
    /// </summary>
    [ContentImporter(".atlas", DisplayName = "Atlas Importer", DefaultProcessor = "AtlasProcessor")]
    public class AtlasImporter : ContentImporter<AtlasInfo>
    {
        public override AtlasInfo Import(string filename, ContentImporterContext context)
        {
            AtlasInfo atlasInfo = null;

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

                            if (nodeName == "atlas")
                            {
                                string textureName = attributes["filename"];
                                atlasInfo = new AtlasInfo(textureName);
                            }
                            else if (nodeName == "image")
                            {                                
                                int imageX = int.Parse(attributes["x"]);
                                int imageY = int.Parse(attributes["y"]);
                                int imageWidth = int.Parse(attributes["w"]);
                                int imageHeight = int.Parse(attributes["h"]);
                                int imageOx = int.Parse(attributes["ox"]);
                                int imageOy = int.Parse(attributes["oy"]);

                                AtlasImageInfo imageInfo = new AtlasImageInfo(imageX, imageY, imageWidth, imageHeight, imageOx, imageOy);
                                atlasInfo.addInfo(imageInfo);
                            }
                        }
                        break;
                    }
                }
            }

            return atlasInfo;
        }
    }
}
