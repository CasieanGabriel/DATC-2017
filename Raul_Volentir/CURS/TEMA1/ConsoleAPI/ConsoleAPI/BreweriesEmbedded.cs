using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPI
{
    class BreweriesEmbedded
    {

        public BreweriesEmbedded(List<ClassOfBreweries> brewery)
        {
            this.brewery = brewery;
        }

        public List<ClassOfBreweries> brewery { get; set; }

        public static explicit operator BreweriesEmbedded(Newtonsoft.Json.Linq.JToken t)
        {

            List<ClassOfBreweries> breweryList = t["brewery"].ToObject<List<ClassOfBreweries>>();
            return new BreweriesEmbedded(breweryList);
        }

    }
}
