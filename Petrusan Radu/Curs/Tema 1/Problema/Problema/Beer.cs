using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problema
{
    public class Beer
    {
        protected Beer()
        {
        }

        public Beer(string name)
        {
            Name = name;
        }

        public int Id { get; protected set; }
        public string Name { get; set; }
        public BeerStyle Style { get; set; }
        public Brewery Brewery { get; set; }
    }
}
