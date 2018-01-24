using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class BeerSelf
    {
        private string href;

        public BeerSelf(string href)
        {
            this.href = href;
        }

        public string Href
        {
            get { return href; }
            set { href = value; }
        }

        public static explicit operator BeerSelf(JToken token)
        {
            return new BeerSelf((string)token["href"]);
        }

    }
}
