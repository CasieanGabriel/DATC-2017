
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPI
{
    public class Beer
    {
        public Beer(int Id, string Name, int BreweryId, string BreweryName, int StyleId, string StyleName, LinkOfBeer _links)
        {
            this.Id = Id;
            this.Name = Name;
            this.BreweryId = BreweryId;
            this.BreweryName = BreweryName;
            this.StyleId = StyleId;
            this.StyleName = StyleName;
            this._links = _links;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int BreweryId { get; set; }
        public string BreweryName { get; set; }
        public int StyleId { get; set; }
        public string StyleName { get; set; }
        public LinkOfBeer _links { get; set; }

    }

    public class LinkOfBeer
    {
        public LinkOfBeer(ClassOfBeerSelf Self, ClassOfStyle Style, ClassOfBeweryBeer Brewery)
        {
            this.Self = Self;
            this.Style = Style;
            this.Brewery = Brewery;
        }

        public ClassOfBeerSelf Self { get; set; }
        public ClassOfStyle Style { get; set; }
        public ClassOfBeweryBeer Brewery { get; set; }

        public static explicit operator LinkOfBeer(Newtonsoft.Json.Linq.JToken t)
        {
            return new LinkOfBeer((ClassOfBeerSelf)t["self"], (ClassOfStyle)t["style"], (ClassOfBeweryBeer)t["brewery"]);
        }
    }
    public class Next
    {

        public string href { get; set; }
    }

    public class Page
    {
        public string href { get; set; }
        public bool templ { get; set; }
    }

}
