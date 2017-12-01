using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Hal.Client
{
    class Breweries_class
    {
    }


    public class Breweries
    {
        public Breweries(int id, string name, BreweriesLinks _links)
        {
            Id = id;
            Name = name;
            _links = _links;
        }

        public string href { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public BreweriesLinks _links { get; set; }

        public static explicit operator Breweries(JObject v)
        {
            return new Breweries((int)v["Id"], (string)v["Name"], (BreweriesLinks)v["_links"]);
        }
    }

    public class SelfBreweries
    {
        public SelfBreweries(string href)
        {
            this.href = href;
        }

        public string href { get; set; }

        public static explicit operator SelfBreweries(JToken t)
        {
            return new SelfBreweries((string)t["href"]);
        }
    }



    public class BreweriesBeers
    {
        public BreweriesBeers(string href)
        {
            this.href = href;
        }

        public string href { get; set; }

        public static explicit operator BreweriesBeers(JToken t)
        {
            return new BreweriesBeers((string)t["href"]);
        }
    }

    public class BreweriesLinks
    {
        public BreweriesLinks(SelfBreweries self, BreweriesBeers bere)
        {
            this.self = self;
            this.bere = bere;
        }

        public SelfBreweries self { get; set; }
        public BreweriesBeers bere { get; set; }

        public static explicit operator BreweriesLinks(JToken t)
        {
            return new BreweriesLinks((SelfBreweries)t["self"], (BreweriesBeers)t["beer"]);
        }
    }

    public class EmbeddedBreweries
    {
        public EmbeddedBreweries(List<Breweries> breweries)
        {
            this.breweries = breweries;
        }

        public List<Breweries> breweries { get; set; }

        public static explicit operator EmbeddedBreweries(JToken t)
        {
            List<Breweries> BreweriesList = t["brewery"].ToObject<List<Breweries>>();
            return new EmbeddedBreweries(BreweriesList);
        }
    }

    public class RootObjectBrewery
    {
        public RootObjectBrewery(BreweriesLinks _links, EmbeddedBreweries _embedded)
        {
            _links = _links;
            this._embedded = _embedded;
        }

        public BreweriesLinks _links { get; set; }
        public EmbeddedBreweries _embedded { get; set; }

        public static explicit operator RootObjectBrewery(JObject t)
        {
            return new RootObjectBrewery((BreweriesLinks)t["_links"], (EmbeddedBreweries)t["_embedded"]);
        }
    }

}

