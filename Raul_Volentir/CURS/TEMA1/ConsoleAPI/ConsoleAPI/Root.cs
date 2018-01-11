using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPI
{

    public class Root
    {
        public Root(int totalResults, int totalPages, int page, BeerSet _embedded)
        {
            TotalResults = totalResults;
            TotalPages = totalPages;
            Page = page;
            this._embedded = _embedded;
        }

        public int TotalResults { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public BeerSet _embedded { get; set; }

        public static explicit operator Root(Newtonsoft.Json.Linq.JObject t)
        {
            return new Root((int)t["TotalResults"], (int)t["TotalPages"], (int)t["Page"], (BeerSet)t["_embedded"]);
        }
    }   

}
