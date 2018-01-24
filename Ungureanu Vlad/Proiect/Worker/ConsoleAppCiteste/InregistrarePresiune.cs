using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCiteste
{
    public class InregistrarePresiune
    {

        private int idsenzor;
        private int culoare;
        private string latA;
        private string lngA;
        private string latB;
        private string lngB;
        private string latC;
        private string lngC;
        private string latD;
        private string lngD;
        private CuloarePresiune culoareEnum;

        public InregistrarePresiune(int idsenzor, int culoare, string latA, string lngA, string latB, string lngB, string latC, string lngC, string latD, string lngD)
        {
            this.Idsenzor = idsenzor;
            this.Culoare = culoare;
            this.LatA = latA;
            this.LngA = lngA;
            this.LatB = latB;
            this.LngB = lngB;
            this.LatC = latC;
            this.LngC = lngC;
            this.LatD = latD;
            this.LngD = lngD;
        }

        public InregistrarePresiune(int idsenzor, int culoare, string latA, string lngA, string latB, string lngB, string latC, string lngC, string latD, string lngD, CuloarePresiune culoareEnum) : this(idsenzor, culoare, latA, lngA, latB, lngB, latC, lngC, latD, lngD)
        {
            this.culoareEnum = culoareEnum;
        }

        public InregistrarePresiune()
        {
        }

        public CuloarePresiune CuloareEnum { get => culoareEnum; set => culoareEnum = value; }
        public int Idsenzor { get => idsenzor; set => idsenzor = value; }
        public int Culoare { get => culoare; set => culoare = value; }
        public string LatA { get => latA; set => latA = value; }
        public string LngA { get => lngA; set => lngA = value; }
        public string LatB { get => latB; set => latB = value; }
        public string LngB { get => lngB; set => lngB = value; }
        public string LatC { get => latC; set => latC = value; }
        public string LngC { get => lngC; set => lngC = value; }
        public string LatD { get => latD; set => latD = value; }
        public string LngD { get => lngD; set => lngD = value; }
    }
}
