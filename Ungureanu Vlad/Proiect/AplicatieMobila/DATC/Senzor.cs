using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps.Model;

namespace DATC
{
    public class Senzor
    {
        private int id;
        private string idsenzor;
        private string latitudine;
        private string longitudine;
        private LatLng coordonate;
        private List<float> temperatura;
        private List<int> presiune;
        private List<int> umiditate;
        private List<DateTime> data;

        public Senzor(int id, string idsenzor, string latitudine, string longitudine)
        {
            this.id = id;
            this.idsenzor = idsenzor;
            this.latitudine = latitudine;
            this.longitudine = longitudine;
            // this.coordonate = new LatLng(double.Parse(latitudine), double.Parse(longitudine));
        }

        public int Id { get => id; set => id = value; }
        public string Idsenzor { get => idsenzor; set => idsenzor = value; }
        public string Latitudine { get => latitudine; set => latitudine = value; }
        public string Longitudine { get => longitudine; set => longitudine = value; }
        public LatLng Coordonate { get => coordonate; set => coordonate = value; }
        public List<float> Temperatura { get => temperatura; set => temperatura = value; }
        public List<int> Presiune { get => presiune; set => presiune = value; }
        public List<int> Umiditate { get => umiditate; set => umiditate = value; }
        public List<DateTime> Data { get => data; set => data = value; }
    }
}