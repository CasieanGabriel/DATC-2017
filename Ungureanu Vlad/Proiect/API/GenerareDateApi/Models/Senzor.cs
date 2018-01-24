using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerareDateApi
{
    public class Senzor
    {
        private int id;
        private string idsenzor;
        private string latitudine;
        private string longitudine;

        public Senzor(int id, string idsenzor, string latitudine, string longitudine)
        {
            this.Id = id;
            this.Idsenzor = idsenzor;
            this.Latitudine = latitudine;
            this.Longitudine = longitudine;
        }

        public int Id { get => id; set => id = value; }
        public string Idsenzor { get => idsenzor; set => idsenzor = value; }
        public string Latitudine { get => latitudine; set => latitudine = value; }
        public string Longitudine { get => longitudine; set => longitudine = value; }
    }
}
