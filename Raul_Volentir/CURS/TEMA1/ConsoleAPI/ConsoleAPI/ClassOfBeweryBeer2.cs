using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPI
{
    public class ClassOfBeweryBeer2
    {
        public class ClassOfBreweryBeer2
        {
            public ClassOfBreweryBeer2(string href)
            {
                this.href = href;
            }

            public string href { get; set; }

            public static explicit operator ClassOfBreweryBeer2(Newtonsoft.Json.Linq.JToken t)
            {
                return new ClassOfBreweryBeer2((string)t["href"]);
            }
        }
    }
}
