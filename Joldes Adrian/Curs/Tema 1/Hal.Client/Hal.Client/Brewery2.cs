using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class Brewery2
    {
        private int id;
        private string name;
        private Links2 links;


        public Brewery2(int id, string name, Links2 links)
        {
            this.id = id;
            this.name = name;
            this.links = links;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        internal Links2 Links
        {
            get { return links; }
            set { links = value; }
        }

        public static explicit operator Brewery2(JToken token)
        {
            return new Brewery2((int)token["id"], (string)token["name"], (Links2)token["links"]);
        }
    }
}
