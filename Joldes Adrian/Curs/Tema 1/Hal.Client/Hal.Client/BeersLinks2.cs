using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class BeerLinks2
    {

        private BeerSelf self;
        private Style style;
        private BreweryBeer brewery;

        public BeerSelf Self
        {
            get { return self; }
            set { self = value; }
        }


        public Style Style
        {
            get { return style; }
            set { style = value; }
        }

        public BreweryBeer Brewery
        {
            get { return brewery; }
            set { brewery = value; }
        }



         public BeerLinks2 (BeerSelf self, Style style, BreweryBeer brewery)
        {
            this.self = self;
            this.style = style;
            this.brewery = brewery;
        }

         public static explicit operator BeerLinks2(JToken token)
         {
             return new BeerLinks2((BeerSelf)token["self"], (Style)token["style"], (BreweryBeer)token["brewery"]);
         }
    }
}
