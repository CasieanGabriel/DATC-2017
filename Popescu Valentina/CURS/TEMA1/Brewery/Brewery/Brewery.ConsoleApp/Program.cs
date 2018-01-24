using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Brewery.ConsoleApp
{
    public class Program
    {
        public static readonly Uri BaseUri = new Uri("http://datc-rest.azurewebsites.net/breweries");

        private static void Main()
        {
            
            var breweries = GetBreweries();

            PrintBreweries(breweries);

            var selectedBrewery = GetSelectedBrewery(breweries);

            var beers = GetBeers(selectedBrewery);

            
            if (beers.Embedded == null || beers.Embedded.Beers == null || beers.Embedded.Beers.Length == 0)
            {
                Console.WriteLine("Beraria nu contine nicio bere!");
                Console.ReadLine();
                return;
            }

           
            PrintBeers(beers);

            var selectedBeer = GetSelectedBeer(beers);

            PrintBeerDetails(selectedBeer);

            
        }

        private static BreweryRootobject GetBreweries()
        {
          
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/hal+json");

                var result = httpClient.GetStringAsync(BaseUri).Result;

                return ReadToObject<BreweryRootobject>(result);
            }
        }

        private static void PrintBreweries(BreweryRootobject breweries)
        {
            foreach (var brewery in breweries.Embedded.Breweries)
            {
                Console.WriteLine(brewery.Id + " - " + brewery.Name);
            }
        }

        private static Brewery GetSelectedBrewery(BreweryRootobject breweries)
        {
            Console.WriteLine();
            Console.Write("Selecteaza o berarie: ");

            if (!int.TryParse(Console.ReadLine(), out int selectedBreweryId))
            {
                Console.WriteLine("Incearca din nou:");
                return GetSelectedBrewery(breweries);
            }

            if (breweries.Embedded.Breweries.All(x => x.Id != selectedBreweryId))
            {
                Console.WriteLine("Incearca din nou:");
                return GetSelectedBrewery(breweries);
            }

            var selectedBrewery = breweries.Embedded.Breweries.Single(x => x.Id == selectedBreweryId);

            return selectedBrewery;
        }

        private static BeerRootobject GetBeers(Brewery selectedBrewery)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/hal+json");

                var result = httpClient.GetStringAsync(new Uri(BaseUri, selectedBrewery.BeersLinks.Beers.Href)).Result;

                return ReadToObject<BeerRootobject>(result);
            }
        }

        private static void PrintBeers(BeerRootobject beers)
        {
            Console.WriteLine();
            Console.WriteLine("Beri disponibile: ");

            foreach (var beer in beers.Embedded.Beers)
            {
                Console.WriteLine(beer.Id + " - " + beer.Name);
            }
        }

        private static Beer GetSelectedBeer(BeerRootobject beers)
        {
            Console.WriteLine();
            Console.Write("Selecteaza o bere: ");

            if (!int.TryParse(Console.ReadLine(), out int selectedBeerId))
            {
                Console.WriteLine("Incearca din nou:");
                return GetSelectedBeer(beers);
            }

            if (beers.Embedded.Beers.All(x => x.Id != selectedBeerId))
            {
                Console.WriteLine("Incearca din nou:");
                return GetSelectedBeer(beers);
            }

            var selectedBeer = beers.Embedded.Beers.Single(x => x.Id == selectedBeerId);

            return selectedBeer;
        }

        private static void PrintBeerDetails(Beer beer)
        {
            Console.WriteLine();
            Console.WriteLine("Name: " + beer.Name);
            Console.WriteLine("Style: " + beer.Style);
        }

        /*
        private static bool GetShouldCreateNewBeer()
        {
            Console.WriteLine();
            Console.WriteLine("Doriti sa adaugati o bere?");
            Console.WriteLine("Da \t Nu");

            var input = Console.ReadLine();
            if (input != "Da" && input != "Nu")
            {
                Console.WriteLine("Incercati din nou!");
                return GetShouldCreateNewBeer();
            }

            return input == "Da";
        }


        private static Beer GetNewBeer()
        {
            Console.WriteLine();
            Console.Write("Numele noii beri: ");
            var name = Console.ReadLine();

            return new Beer
            {
                Name = name
            };
        }
        */
        private static T ReadToObject<T>(string json) where T : class
        {
            
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                var obj = serializer.ReadObject(ms) as T;
                return obj;
            }
        }
    }
}
