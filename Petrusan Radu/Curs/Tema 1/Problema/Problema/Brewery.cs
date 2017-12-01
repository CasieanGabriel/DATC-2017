using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problema
{
    public class Brewery
    {
        protected Brewery()
        {
        }

        public int Id { get; protected set; }
        public string Name { get; set; }
    }
}
