using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class Links2
    {
        private Self2 self;
        private Beers beers;

     
        internal Beers Beers
        {
            get { return beers; }
            set { beers = value; }
        }

        internal Self2 Self
        {
            get { return self; }
            set { self = value; }
        }
        public Links2(Self2 self, Beers beers)
        {
            this.self = self;
            this.beers = beers;
        }

        public static explicit operator Links2(JToken token)
        {
            return new Links2((Self2)token["self"], (Beers)token["beers"]);
        }
    }
}
