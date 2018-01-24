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
    public class Log
    {
        private int idSenzor;
        private int valoareEronata;
        private int valoareCorecta;
        private string tipData;
        private string data;

        public Log(int idSenzor, int valoareEronata, int valoareCorecta, string tipData, string data)
        {
            this.idSenzor = idSenzor;
            this.valoareEronata = valoareEronata;
            this.valoareCorecta = valoareCorecta;
            this.tipData = tipData;
            this.data = data;
        }

        public int IdSenzor { get => idSenzor; set => idSenzor = value; }
        public int ValoareEronata { get => valoareEronata; set => valoareEronata = value; }
        public int ValoareCorecta { get => valoareCorecta; set => valoareCorecta = value; }
        public string TipData { get => tipData; set => tipData = value; }
        public string Data { get => data; set => data = value; }
    }
}