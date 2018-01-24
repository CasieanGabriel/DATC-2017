using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class Beer2
    {

        private int id;
        private string name;
        private int breweryId;
        private int styleId;
        private string breweryName;
        private BeerLinks2 links;
        private string styleName;

        public Beer2(int id, string name, int breweryId, string breweryName, int styleId, string styleName, BeerLinks2 links)
        {
            this.id = id;
            this.name = name;
            this.breweryId = breweryId;
            this.breweryName = breweryName;
            this.styleId = styleId;
            this.styleName = styleName;
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

        public int BreweryId
        {
            get { return breweryId; }
            set { breweryId = value; }
        }

        public string BreweryName
        {
            get { return breweryName; }
            set { breweryName = value; }
        }

        public int StyleId
        {
            get { return styleId; }
            set { styleId = value; }
        }

        public string StyleName
        {
            get { return styleName; }
            set { styleName = value; }
        }

        public BeerLinks2 Links
        {
            get { return links; }
            set { links = value; }
        }

        public static explicit operator Beer2(JToken token)
        {
            return new Beer2((int)token["id"], (string)token["name"], (int)token["breweryId"], (string)token["breweryName"], (int)token["styleId"], (string)token["styleName"], (BeerLinks2)token["links"]);
        }
    }
}
