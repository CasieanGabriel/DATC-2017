using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class RootObjectBeers
    {
        private int totalResults;
        private int totalPages;
        private int page;
        private BeerEmbedded embedded;

        public RootObjectBeers(int totalResults, int totalPages, int page, BeerEmbedded embedded)
        {
            this.totalResults = totalResults;
            this.totalPages = totalPages;
            Page = page;
            this.embedded = embedded;
        }

        public int TotalResults
        {
            get { return totalResults; }
            set { totalResults = value; }
        }

        public int TotalPages
        {
            get { return totalPages; }
            set { totalPages = value; }
        }

        public int Page
        {
            get { return page; }
            set { page = value; }
        }

        public BeerEmbedded Embedded
        {
            get { return embedded; }
            set { embedded = value; }
        }

        public static explicit operator RootObjectBeers(JObject token)
        {
            return new RootObjectBeers((int)token["totalResults"], (int)token["totalPages"], (int)token["page"], (BeerEmbedded)token["embedded"]);
        }

    }
}
