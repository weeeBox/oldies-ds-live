using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive
{
    class PSContent
    {
        private string data;

        public PSContent(string data)
        {
            this.data = data;
        }

        public string Data
        {
            get { return data; }
        }
    }
}
