using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPI
{
    class ClassOfRoot
    {
        public ClassOfRoot(LinkOfBewery _links, BreweriesEmbedded _embedded)
        {
            this._links = _links;
            this._embedded = _embedded;
        }

        public LinkOfBewery _links { get; set; }
        public BreweriesEmbedded _embedded { get; set; }

        public static explicit operator ClassOfRoot(Newtonsoft.Json.Linq.JToken t)
        {
            return new ClassOfRoot((LinkOfBewery)t["_links"], (BreweriesEmbedded)t["_embedded"]);
        }
    }
}
