using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCiteste
{
    class Senzor
    {
        public Senzor(int idsenzor, string latitudine, string longitudine)
        {
            this.idsenzor = idsenzor;
            this.latitudine = latitudine;
            this.longitudine = longitudine;
        }

        public int idsenzor { get; set; }
        public string latitudine { get; set; }
        public string longitudine { get; set; }
    }
}
