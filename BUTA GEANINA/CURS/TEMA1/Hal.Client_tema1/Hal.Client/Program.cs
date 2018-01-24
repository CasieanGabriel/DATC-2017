using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.IO;

namespace Hal.Client
{
	class Program
	{
        static int option;

        static void Api_Post(string beer)
        {
            string breweries = "http://datc-rest.azurewebsites.net/breweries";

            var client = new HttpClient();
            var response = new HttpResponseMessage();

            if (beer != null)
            {
                client.PostAsync(breweries, new StringContent(beer));
                Console.WriteLine("The beer has been added.");
            }
            else
            {
                Console.WriteLine("The beer hasn't been added.");
            }
            
            // Console.WriteLine(data);
            // Console.ReadLine();

        }

        static void Api_GetBreweries(int Id)
        {
            string breweries = "http://datc-rest.azurewebsites.net/breweries";

            var client = new HttpClient();
            var response = client.GetAsync(breweries).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            var result = (JObject)JsonConvert.DeserializeObject(data);
            var brewery = (RootObjectBrewery)result;
            Console.WriteLine(brewery._embedded.breweries[Id - 1].Name);

        }
        static void Api_GetBeers(int Id)
        {
            string beers = "http://datc-rest.azurewebsites.net/beers";

            var client = new HttpClient();
            var response = client.GetAsync(beers).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            var result = (JObject)JsonConvert.DeserializeObject(data);
            var beer = (RootObjectBeers)result;
            Console.WriteLine(beer._embedded.beer[Id - 1].Name);
            do
            {
                Console.WriteLine("Number of " + beer._embedded.beer.Count() + " available at the brewery.");
            }
            while (Id < 0);

        }

		static void Main(string[] args)
		{
            string name;
            int id=0;


            do
            {
                Console.WriteLine("   Menu:\n");
                Console.WriteLine("1. View beers and breweries. \n");
                Console.WriteLine("2. Add a new beer. \n");
                Console.WriteLine("3. Exit. \n\n");
                Console.WriteLine("Please, enter your option: ");
                option = Int32.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Please, write the ID of which breweries you want to view: ");
                        id = Int32.Parse(Console.ReadLine());
                        Api_GetBreweries(id);
                        Api_GetBeers(id);
                        break;

                    case 2:
                        break;

                    case 3:
                        Console.WriteLine("What type of beer do you want to add? \n");
                        Console.WriteLine("Please, write the name of beer: ");
                        name = Console.ReadLine();
                        Beers beer = new Beers(name);
                        Api_Post(name);
                        break;

                    case 4:
                        break;

                    default:
                        Console.WriteLine("Invalid option! Please choose another. \n");
                        Console.ReadKey();
                        break;
                }
            }
            while (option!=4);



		}
	}
}
