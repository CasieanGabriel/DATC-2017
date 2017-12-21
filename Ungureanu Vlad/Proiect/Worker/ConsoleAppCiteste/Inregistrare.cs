using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleAppCiteste
{
    public class Inregistrare
    {
        public int Id { get; set; }
        public int idsenzor { get; set; }
        public int temperatura { get; set; }
        public int umiditate { get; set; }
        public int presiune { get; set; }
        public DateTime data { get; set; }

        public InregistrareTemperatura[,] inregistrareTemperaturaActuala;
        
        public Inregistrare()
        {
            this.inregistrareTemperaturaActuala = new InregistrareTemperatura[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4;j++)
            {
                    inregistrareTemperaturaActuala[i, j] = new InregistrareTemperatura();
            }
        }

        public Inregistrare(int id, int idsenzor, int temperatura, int umiditate, int presiune, DateTime data, InregistrareTemperatura[,] inregistrareTemperaturaActuala)
        {
            Id = id;
            this.idsenzor = idsenzor;
            this.temperatura = temperatura;
            this.umiditate = umiditate;
            this.presiune = presiune;
            this.data = data;
            this.inregistrareTemperaturaActuala = new InregistrareTemperatura[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    inregistrareTemperaturaActuala[i, j] = new InregistrareTemperatura();
                }
            this.inregistrareTemperaturaActuala = inregistrareTemperaturaActuala;
        }
        public Inregistrare(int id, int idsenzor, int temperatura, int umiditate, int presiune, DateTime data)
        {
            Id = id;
            this.idsenzor = idsenzor;
            this.temperatura = temperatura;
            this.umiditate = umiditate;
            this.presiune = presiune;
            this.data = data;
            this.inregistrareTemperaturaActuala = new InregistrareTemperatura[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    inregistrareTemperaturaActuala[i, j] = new InregistrareTemperatura();
                }
        }
    }
}
