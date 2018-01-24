using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class Links
    {
        private Self self;
        private List<Brewery> brewery;

        public Links(Self self, List<Brewery> brewery)
        {
            this.self = self;
            this.brewery = brewery;
        }

        public Self Self
        {
            get { return self; }
            set { self = value; }
        }

        public List<Brewery> Brewery
        {
            get { return brewery; }
            set { brewery = value; }
        }
        public static explicit operator Links(JToken token)
        {
            List<Brewery> breweries = token["brewery"].ToObject<List<Brewery>>();
            return new Links((Self)token["href"],breweries);
        }
    }
}
