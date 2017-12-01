using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Brewery.ConsoleApp
{
    [DataContract]
    public class BreweryRootobject
    {
        [DataMember(Name = "_embedded")]
        public EmbeddedBreweries Embedded { get; set; }
    }
    
    [DataContract]
    public class EmbeddedBreweries
    {
        [DataMember(Name = "brewery")]
        public List<Brewery> Breweries { get; set; }
    }

    [DataContract]
    public class Brewery
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "_links")]
        public BreweryBeersLinks BeersLinks { get; set; }
    }

    [DataContract]
    public class BreweryBeersLinks
    {
        [DataMember(Name = "self")]
        public Link Self { get; set; }

        [DataMember(Name = "beers")]
        public Link Beers { get; set; }
    }

    [DataContract]
    public class Link
    {
        [DataMember(Name = "href")]
        public string Href { get; set; }
    }
}
