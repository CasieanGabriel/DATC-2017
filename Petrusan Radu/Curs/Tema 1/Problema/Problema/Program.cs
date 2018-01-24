using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace Problema
{
    class Program
    {
        static void Main(string[] args)
        {
            Loop();
        }

        static void Loop()
        {
            var httpUtils = new HttpUtils();
            var endPoint = ConfigurationSettings.AppSettings.Get("ApiAddress");

            var meniu = new Meniu();
            meniu.AfiseazaOptiuni();
            var optiune = meniu.CitesteOptiune();

            while (optiune != 3)
            {
                meniu.AfiseazaOptiuni();

                switch (optiune)
                {
                    case 1:
                        httpUtils.ReadData(endPoint + "breweries");
                        break;

                    case 2:
                        AdaugaBere();
                        break;

                    default:
                        break;
                }

                optiune = meniu.CitesteOptiune();                
            }
            return;
        }

        

        static void AdaugaBere()
        {

        }
    }
}
