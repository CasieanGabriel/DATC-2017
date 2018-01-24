using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPI
{
    public class ClassofBeerSelf3
    {
        public ClassofBeerSelf3(string href)
        {
            this.href = href;
        }

        public string href { get; set; }

        public static explicit operator ClassofBeerSelf3(Newtonsoft.Json.Linq.JToken t)
        {
            return new ClassofBeerSelf3((string)t["href"]);
        }
    }
}
