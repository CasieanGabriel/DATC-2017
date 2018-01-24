using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class Embedded
    {

        private List<Brewery2> brewery;

        public List<Brewery2> Brewery
        {
            get { return brewery; }
            set { brewery = value; }
        }

        public Embedded(List<Brewery2> brewery)
        {
            this.brewery = brewery;
        }


        public static explicit operator Embedded(JToken token)
        {
            List<Brewery2> breweryList = token["brewery"].ToObject<List<Brewery2>>();
            return new Embedded(breweryList);
        }
    }

}
