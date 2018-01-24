using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1.Classes
{
    class Beers
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? BreweryId { get; set; }

        public string BreweryName { get; set; }

        public int? StyleId { get; set; }

        public string StyleName { get; set; }

        public string StyleLink { get; set; }

        public string SelfLink { get; set; }

        public string ReviewLink { get; set; }
    }
}
