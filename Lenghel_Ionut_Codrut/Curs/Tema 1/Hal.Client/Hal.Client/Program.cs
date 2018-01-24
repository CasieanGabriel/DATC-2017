using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

//HAL - Hypertext Application Language

namespace Hal.Client
{
	class Program
	{

        async public static void PostBeer()
        {

            Beer2 beer = new Beer2();
            Console.WriteLine("Id:");
            beer.Id = int.Parse(Console.ReadLine());
            Console.WriteLine("Name:");
            beer.Name = Console.ReadLine();
            Console.WriteLine("BreweryId:");
            beer.BreweryId = int.Parse(Console.ReadLine()); ;
            Console.WriteLine("BreweryName:");
            beer.BreweryName = Console.ReadLine();
            Console.WriteLine("StyleId:");
            beer.StyleId = int.Parse(Console.ReadLine()); ;
            Console.WriteLine("StyleName:");
            beer.StyleName = Console.ReadLine();

            var json = JsonConvert.SerializeObject(beer); //add user input to JASON file by serializing

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://datc-rest.azurewebsites.net/beers");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) // write to JSON file
            {
                streamWriter.Write(json);
                streamWriter.Flush();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        }

        public static void Main(string[] args)
	{
            Int32 opt;
            string apiEndpoint = "http://datc-rest.azurewebsites.net/breweries";
            List<Brewery2B> berarii = new List<Brewery2B>();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/hal+json");
			var response = client.GetAsync(apiEndpoint).Result;

			var data = response.Content.ReadAsStringAsync().Result;
            var obj = JsonConvert.DeserializeObject<dynamic>(data);
            RootObjectB rootB = JsonConvert.DeserializeObject<RootObjectB>(data);
      
            do
            {
                Console.WriteLine("\n1.Show brewery");
                Console.WriteLine("2.Select brewery and show the beers they have");
                Console.WriteLine("3.Use POST method to add a beer");
                Console.WriteLine("4.Exit");
                Console.WriteLine("Your option: ");

                opt = int.Parse(Console.ReadLine());
                switch (opt)
                {
                    case 1: // Show breweries

                        foreach (Brewery2B berarie in rootB._embedded.brewery)
                        {
                            Console.WriteLine("Brewery " + berarie.Id + "." + berarie.Name);
                        }
                        break;

                    case 2: // Select brewery and show the beers they have

                        RootObject beers = null;
                        dynamic obiect;

                        foreach (Brewery2B berarie in rootB._embedded.brewery)
                        {
                            Console.WriteLine("Brewery " + berarie.Id + "." + berarie.Name);
                        }

                        Console.WriteLine("Select brewery ID:  ");

                        int id = int.Parse(Console.ReadLine());

                        // Get only the first 34 characters from the endpoint API
                        string brewTypesLink = apiEndpoint.Substring(0,34) + "/styles" + rootB._embedded.brewery[id-1]._links.beers.href.Substring(10);
                        response = client.GetAsync(brewTypesLink).Result;
                        data = response.Content.ReadAsStringAsync().Result;

                        Console.WriteLine("\nYou selected: \"" + rootB._embedded.brewery[id - 1].Name + "\"\n");
                        beers = JsonConvert.DeserializeObject<RootObject>(data); 
                        foreach (Beer2 b in beers._embedded.beer)
                        {
                            Console.WriteLine(b.Id + ". " + b.Name);
                        }
                        break;

                     case 3: // call POST method to create a beer

                        PostBeer();
                        break;
                }               

            } while (opt != 4);

			Console.ReadLine();
		}

	}
}
