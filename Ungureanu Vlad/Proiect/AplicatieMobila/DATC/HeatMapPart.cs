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
    public class HeatMapPart
    {
        private int id;
        private string latA;
        private string lngA;
        private string latB;
        private string lngB;
        private string latC;
        private string lngC;
        private string latD;
        private string lngD;
        private int culoare;
        private LatLng punctA;
        private LatLng punctB;
        private LatLng punctC;
        private LatLng punctD;

        public HeatMapPart(int id, string latA, string lngA, string latB, string lngB, string latC, string lngC, string latD, string lngD, int culoare)
        {
            this.id = id;
            this.latA = latA;
            this.lngA = lngA;
            this.latB = latB;
            this.lngB = lngB;
            this.latC = latC;
            this.lngC = lngC;
            this.latD = latD;
            this.lngD = lngD;
            this.culoare = culoare;
        }

        public int Id { get => id; set => id = value; }
        public string LatA { get => latA; set => latA = value; }
        public string LngA { get => lngA; set => lngA = value; }
        public string LatB { get => latB; set => latB = value; }
        public string LngB { get => lngB; set => lngB = value; }
        public string LatC { get => latC; set => latC = value; }
        public string LngC { get => lngC; set => lngC = value; }
        public string LatD { get => latD; set => latD = value; }
        public string LngD { get => lngD; set => lngD = value; }
        public int Culoare { get => culoare; set => culoare = value; }
        public LatLng PunctA { get => punctA; set => punctA = value; }
        public LatLng PunctB { get => punctB; set => punctB = value; }
        public LatLng PunctC { get => punctC; set => punctC = value; }
        public LatLng PunctD { get => punctD; set => punctD = value; }
    }
}