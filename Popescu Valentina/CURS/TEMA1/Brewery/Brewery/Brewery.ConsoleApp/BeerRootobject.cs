using System.Runtime.Serialization;

namespace Brewery.ConsoleApp
{
    [DataContract]
    public class BeerRootobject
    {
        [DataMember(Name = "_embedded")]
        public EmbeddedBeers Embedded { get; set; }
    }

    [DataContract]
    public class EmbeddedBeers
    {
        [DataMember(Name = "beer")]
        public Beer[] Beers { get; set; }
    }

    [DataContract]
    public class Beer
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }
        
        [DataMember(Name = "Style")]
        public string Style { get; set; }
    }

}
