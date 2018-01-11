using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPI
{

    public class ClassOfLink
    {
        public ClassOfLink(ClassofBeerSelf3 self, ClassOfBeers beers)
        {
            this.self = self;
            this.beers = beers;
        }

        public ClassofBeerSelf3 self { get; set; }
        public ClassOfBeers beers { get; set; }

        public static explicit operator ClassOfLink(JToken t)
        {
            return new ClassOfLink((ClassofBeerSelf3)t["self"], (ClassOfBeers)t["beers"]);
        }
    }

}
