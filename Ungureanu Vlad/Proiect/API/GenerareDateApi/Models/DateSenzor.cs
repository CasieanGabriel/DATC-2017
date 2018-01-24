using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerareDateApi.Models
{
    public class DateSenzor
    {
        private int id;
        private int idsenzor;
        private string temperatura;
        private string umiditate;
        private string presiune;
        private string data;

        public DateSenzor(int id, int idsenzor, string temperatura, string umiditate, string presiune, string data)
        {
            this.Id = id;
            this.Idsenzor = idsenzor;
            this.Temperatura = temperatura;
            this.Umiditate = umiditate;
            this.Presiune = presiune;
            this.Data = data;
        }

        public int Id { get => id; set => id = value; }
        public int Idsenzor { get => idsenzor; set => idsenzor = value; }
        public string Temperatura { get => temperatura; set => temperatura = value; }
        public string Umiditate { get => umiditate; set => umiditate = value; }
        public string Presiune { get => presiune; set => presiune = value; }
        public string Data { get => data; set => data = value; }
    }
}
