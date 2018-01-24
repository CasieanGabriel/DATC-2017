using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class Brewery
    {
        private string href;

        public Brewery(string href)
        {
            this.href = href;
        }

        public string Href
        {
            get { return href; }
            set { href = value; }
        }

        public static explicit operator Brewery(JToken token)
        {
            return new Brewery((string)token["href"]);
        }
    }
}
