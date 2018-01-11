using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPI
{

    public class ClassOfBreweries
    {
        public ClassOfBreweries(int id, string name, ClassOfLink _links)
        {
            Id = id;
            Name = name;
            this._links = _links;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ClassOfLink _links { get; set; }

        public static explicit operator ClassOfBreweries(Newtonsoft.Json.Linq.JToken t)
        {
            return new ClassOfBreweries((int)t["Id"], (string)t["Name"], (ClassOfLink)t["_links"]);
        }
    }


}
