using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataGenerator
{
    class dataForProcess
    {
        int zona;
        int temperatura;
        int umiditate;
        DateTime data;

        public dataForProcess(int zona, int temperatura, int umiditate, DateTime data)
        {
            this.zona = zona;
            this.temperatura = temperatura;
            this.umiditate = umiditate;
            this.data = data;
        }

        public int Zona
        {
            get { return this.zona; }
        }

        public int Temperatura
        {
            get { return this.temperatura; }
        }

        public int Umiditate
        {
            get { return this.umiditate; }
        }

        public string Data
        {
            get { return this.data.ToString("dd/MM/yyyy HH:mm:ss"); }
        }
    }
}
