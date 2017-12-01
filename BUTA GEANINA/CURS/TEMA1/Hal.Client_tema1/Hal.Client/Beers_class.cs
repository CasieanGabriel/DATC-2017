using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Hal.Client
{

    public class Beers
    {
        public Beers(int id, string name, int breweryId, string breweryName, BeersLinks _links)
        {
            Id = id;
            Name = name;
            BreweryId = breweryId;
            BreweryName = breweryName;
            this._links = _links;
        }
        public Beers(string name)
        {
            Name = name;           
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int BreweryId { get; set; }
        public string BreweryName { get; set; }
        public BeersLinks _links { get; set; }

        public static explicit operator Beers(JObject v)
        {
            return new Beers((int)v["Id"], (string)v["Name"], (int)v["BreweryId"], (string)v["BreweryName"], (BeersLinks)v["_links"]);
        }
    }
    
    public class SelfBeers
    {
        public SelfBeers(string href)
        {
            this.href = href;
        }

        public string href { get; set; }

        public static explicit operator SelfBeers(JToken t)
        {
            return new SelfBeers((string)t["href"]);
        }
    }


    public class BeersBrewerys
    {
        public BeersBrewerys(string href)
        {
            this.href = href;
        }

        public string href { get; set; }

        public static explicit operator BeersBrewerys(JToken t)
        {
            return new BeersBrewerys((string)t["href"]);
        }
    }

    public class BeersLinks
    {
        public BeersLinks(SelfBeers self, BeersBrewerys brewery)
        {
            this.self = self;
            this.brewery = brewery;
        }

        public SelfBeers self { get; set; }
        public BeersBrewerys brewery { get; set; }

        public static explicit operator BeersLinks(JToken t)
        {
            return new BeersLinks((SelfBeers)t["self"],  (BeersBrewerys)t["brewery"]);
        }
    }

    public class EmbeddedBeers
    {
        public EmbeddedBeers(List<Beers> beer)
        {
            this.beer = beer;
        }

        public List<Beers> beer { get; set; }

        public static explicit operator EmbeddedBeers(JToken t)
        {
            List<Beers> BeersList = t["beer"].ToObject<List<Beers>>();
            return new EmbeddedBeers(BeersList);
        }
    }

    public class RootObjectBeers
    {
        public RootObjectBeers(int totalResults, int totalPages, int page, EmbeddedBeers _embedded)
        {
            TotalResults = totalResults;
            TotalPages = totalPages;
            Page = page;
            this._embedded = _embedded;
        }

        public int TotalResults { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public EmbeddedBeers _embedded { get; set; }

        public static explicit operator RootObjectBeers(JObject t)
        {
            return new RootObjectBeers((int)t["TotalResults"], (int)t["TotalPages"], (int)t["Page"], (EmbeddedBeers)t["_embedded"]);
        }
    }

}

