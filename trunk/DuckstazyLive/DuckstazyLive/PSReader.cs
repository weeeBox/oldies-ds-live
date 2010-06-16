using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace DuckstazyLive
{
    class PSReader : ContentTypeReader<PSContent>
    {
        protected override PSContent Read(ContentReader input, PSContent existingInstance)
        {
            string data = input.ReadString();
            return new PSContent(data);
        }
    }
}
