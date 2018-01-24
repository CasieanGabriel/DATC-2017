using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class Beers
    {
        private string href;

        public Beers(string href)
        {
            this.href = href;
        }

        public string Href
        {
            get { return href; }
            set { href = value; }
        }

        public static explicit operator Beers(JToken token)
        {
            return new Beers((string)token["href"]);
        }
    }
}
