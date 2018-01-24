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
        public InregistrareUmiditate[,] inregistrareUmiditateActuala;
        public InregistrarePresiune[,] inregistrarePresiuneActuala;

        public Inregistrare()
        {
            this.inregistrareTemperaturaActuala = new InregistrareTemperatura[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4;j++)
            {
                    inregistrareTemperaturaActuala[i, j] = new InregistrareTemperatura();
            }

            this.inregistrareUmiditateActuala = new InregistrareUmiditate[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    inregistrareUmiditateActuala[i, j] = new InregistrareUmiditate();
                }

            this.inregistrarePresiuneActuala = new InregistrarePresiune[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    inregistrarePresiuneActuala[i, j] = new InregistrarePresiune();
                }
        }

        public Inregistrare(int id, int idsenzor, int temperatura, int umiditate, int presiune, DateTime data, InregistrareTemperatura[,] inregistrareTemperaturaActuala, InregistrareUmiditate[,] inregistrareUmiditateActuala, InregistrarePresiune[,] inregistrarePresiuneActuala) :this()
        {
            Id = id;
            this.idsenzor = idsenzor;
            this.temperatura = temperatura;
            this.umiditate = umiditate;
            this.presiune = presiune;
            this.data = data;

            this.inregistrareTemperaturaActuala = inregistrareTemperaturaActuala;
            this.inregistrarePresiuneActuala = inregistrarePresiuneActuala;
            this.inregistrareUmiditateActuala = inregistrareUmiditateActuala;
        }
        public Inregistrare(int id, int idsenzor, int temperatura, int umiditate, int presiune, DateTime data) : this()
        {
            Id = id;
            this.idsenzor = idsenzor;
            this.temperatura = temperatura;
            this.umiditate = umiditate;
            this.presiune = presiune;
            this.data = data;            
        }
    }
}
