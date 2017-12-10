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

namespace DATC
{
    public class DateSenzor
    {
        private int idsenzor;
        private string temperatura;
        private string umiditate;
        private string presiune;
        private string data;

        public DateSenzor(int idsenzor, string temperatura, string umiditate, string presiune, string data)
        {
            this.Idsenzor = idsenzor;
            this.Temperatura = temperatura;
            this.Umiditate = umiditate;
            this.Presiune = presiune;
            this.Data = data;
        }

        public int Idsenzor { get => idsenzor; set => idsenzor = value; }
        public string Temperatura { get => temperatura; set => temperatura = value; }
        public string Umiditate { get => umiditate; set => umiditate = value; }
        public string Presiune { get => presiune; set => presiune = value; }
        public string Data { get => data; set => data = value; }
    }
}