using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerareDate
{
    public class Senzor
    {
    //    int temperatura, umiditate, Id;

        public Senzor (int idsenzor, int temperatura, int umiditate)
        {
            this.idsenzor = idsenzor;
            this.temperatura = temperatura;
            this.umiditate = umiditate;
        }

        public int idsenzor { get; set; } 
        public int temperatura { get; set; } 
        public int umiditate { get; set; }
    }
}
