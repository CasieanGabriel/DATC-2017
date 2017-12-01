using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerareV3
{
    class Inregistrare
    {

        public Inregistrare(int idsenzor, int temperatura, int umiditate,int presiune, DateTime data)
        {
            this.idsenzor = idsenzor;
            this.temperatura = temperatura;
            this.umiditate = umiditate;
            this.presiune = presiune;
            this.data = data;
        }

        public int idsenzor { get; set; }
        public int temperatura { get; set; }
        public int umiditate { get; set; }
        public int presiune { get; set; }
        public DateTime data { get; set; }

    }
}
