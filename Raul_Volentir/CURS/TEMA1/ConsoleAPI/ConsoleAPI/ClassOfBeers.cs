using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPI
{
    public class ClassOfBeers
    {
        public ClassOfBeers(string href)
        {
            this.href = href;
        }

        public string href { get; set; }

        public static explicit operator ClassOfBeers(Newtonsoft.Json.Linq.JToken t)
        {
            return new ClassOfBeers((string)t["href"]);
        }
    }
}
