using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using T1.Classes;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Net;
using System.IO;

namespace T1
{
    class Program
    {
        public static string MAINURL = "http://datc-rest.azurewebsites.net/";
        static void Main(string[] args)
        {
            int option = 0;
            JObject mainBreweryData;
            mainBreweryData = Function.GetJSONFromURL(MAINURL, "breweries");
            do
            {
                Function.ShowMainMenu();
                option = Convert.ToInt32(Console.ReadLine());
                Function.FirstOptionSelected(option, mainBreweryData);
            } while (option != 0);
        }

       
    }
}