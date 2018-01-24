using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class BreweryBeer
    {
        private string href;

        public BreweryBeer(string href)
        {
            this.href = href;
        }

        public string Href
        {
            get { return href; }
            set { href = value; }
        }

        public static explicit operator BreweryBeer(JToken token)
        {
            return new BreweryBeer((string)token["href"]);
        }
    }
}
