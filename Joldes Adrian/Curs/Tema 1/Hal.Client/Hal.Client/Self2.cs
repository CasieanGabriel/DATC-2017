using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class Self2
    {
        private string href;

        public string Href
        {
            get { return href; }
            set { href = value; }
        }

        public Self2(string href)
        {
            this.href = href;
        }

        public static explicit operator Self2(JToken token)
        {
            return new Self2((string)token["href"]);
        }

      
    }
}
