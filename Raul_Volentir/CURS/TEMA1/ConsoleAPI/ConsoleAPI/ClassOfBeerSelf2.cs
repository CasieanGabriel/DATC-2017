using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPI
{
    public class ClassOfBeerSelf2
    {
        public ClassOfBeerSelf2(string href)
        {
            this.href = href;
        }

        public string href { get; set; }

        public static explicit operator ClassOfBeerSelf2(Newtonsoft.Json.Linq.JToken t)
        {
            return new ClassOfBeerSelf2((string)t["href"]);
        }
    }
}