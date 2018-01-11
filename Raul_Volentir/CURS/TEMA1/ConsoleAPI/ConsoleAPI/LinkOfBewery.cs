using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPI
{
    public class LinkOfBewery
    {
        public LinkOfBewery(ClassOfBeerSelf2 self, List<ClassOfBeweryBeer2> brewery)
        {
            this.self = self;
            this.brewery = brewery;
        }

        public ClassOfBeerSelf2 self { get; set; }
        public List<ClassOfBeweryBeer2> brewery { get; set; }

        public static explicit operator LinkOfBewery(Newtonsoft.Json.Linq.JToken t)
        {
            List<ClassOfBeweryBeer2> breweryList = t["brewery"].ToObject<List<ClassOfBeweryBeer2>>();
            return new LinkOfBewery((ClassOfBeerSelf2)t["self"], breweryList);
        }
    }
}
