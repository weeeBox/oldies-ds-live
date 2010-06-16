using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace ContentLibrary
{
    public class PSSourceCode
    {
        private string sourceCode;

        public PSSourceCode(string sourceCode)
        {
            this.sourceCode = sourceCode;
        }

        public string SouceCode 
        {
            get { return sourceCode; }
        }
    }
}
