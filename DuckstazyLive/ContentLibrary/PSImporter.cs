using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

// TODO: replace this with the type you want to import.
using TImport = System.String;

namespace ContentLibrary
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
    [ContentImporter(".txt", DisplayName = "Custom Importer", DefaultProcessor = "PSProcessor")]
    public class PSImporter : ContentImporter<PSSourceCode>
    {
        public override PSSourceCode Import(string filename, ContentImporterContext context)
        {
            string sourceCode = System.IO.File.ReadAllText(filename);
            return new PSSourceCode(sourceCode);
        }
    }
}
