using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPI
{

    public class BeerSet
    {
        public BeerSet(List<Beer> beer)
        {
            this.beer = beer;
        }

        public List<Beer> beer { get; set; }

        public static explicit operator BeerSet(Newtonsoft.Json.Linq.JToken t)
        {
            List<Beer> BeerList = t["beer"].ToObject<List<Beer>>();
            return new BeerSet(BeerList);
        }
    }


}