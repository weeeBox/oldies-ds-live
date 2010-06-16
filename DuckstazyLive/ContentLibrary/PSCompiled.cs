using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentLibrary
{
    class PSCompiled
    {
        private string data;

        public PSCompiled(String data)
        {
            this.data = data;
        }

        public string Data
        {
            get { return data; }
        }
    }
}
