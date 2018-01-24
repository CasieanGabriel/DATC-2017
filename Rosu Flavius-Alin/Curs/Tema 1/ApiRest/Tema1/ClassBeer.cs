using System.Runtime.Serialization;

namespace Tema1
{
    
    [DataContract]
    public class Beer
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "StyleName")]
        public string Style { get; set; }
    }

    [DataContract]
    public class ClassBeer
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

}
