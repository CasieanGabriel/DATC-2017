using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class Self
    {
        private string href;

        public Self(string href)
        {
            this.href = href;
        }

        public string Href
        {
            get { return href; }
            set { href = value; }
        }

        public static explicit operator Self(JToken token)
        {
            return new Self((string)token["href"]);
        }
    }
}
