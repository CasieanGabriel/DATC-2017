using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class Style
    {
        private string href;

        public string Href
        {
            get { return href; }
            set { href = value; }
        }
        public Style(string href)
        {
            this.href = href;
        }

        public static explicit operator Style(JToken token)
        {
            return new Style((string)token["href"]);
        }
    }
}
