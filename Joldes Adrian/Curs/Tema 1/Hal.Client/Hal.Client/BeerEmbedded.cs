using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class BeerEmbedded
    {
        private List<Beer2> beer;

        public BeerEmbedded(List<Beer2> beer)
        {
            this.beer = beer;
        }


        public List<Beer2> Beer
        {
            get { return beer; }
            set { beer = value; }
        }

        public static explicit operator BeerEmbedded(JToken token)
        {
            List<Beer2> Beer2List = token["beer"].ToObject<List<Beer2>>();
            return new BeerEmbedded(Beer2List);
        }
    }
}
