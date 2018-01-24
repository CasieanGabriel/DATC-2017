using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problema
{
    public class BeerStyle
    {
        protected BeerStyle()
        {
        }

        public int Id { get; protected set; }
        public string Name { get; set; }
    }
}
