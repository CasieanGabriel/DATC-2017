using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;


namespace Tema1
{
    class Program
    {
        public static readonly Uri BreweriesUri = new Uri("http://datc-rest.azurewebsites.net/breweries");
      
        private static void Main()
        {
            var breweries = GetBreweries();
            DisplayBreweries(breweries);
            var chosedBrewery = GetChosedBrewery(breweries);
            var beers = GetBeers(chosedBrewery);

            DisplayBeers(beers);
            var selectedBeer = GetChosedBeer(beers);

            DisplayBeerDetails(selectedBeer);

        }
           
            private static ClassBrewery GetBreweries()
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/hal+json");
                    var result = httpClient.GetStringAsync(BreweriesUri).Result;
                    return ReadToObject<ClassBrewery>(result);
                }
            }

            private static void DisplayBreweries(ClassBrewery breweries)
            {
                foreach (var brewery in breweries.Embedded.Breweries)
                {
                    Console.WriteLine(brewery.Id + " - " + brewery.Name);
                }
            }

            private static Brewery GetChosedBrewery(ClassBrewery breweries)
            {
                Console.WriteLine();
                Console.Write("Please select brewery: ");

                if (!int.TryParse(Console.ReadLine(), out int selectedBreweryId))
                {
                    Console.WriteLine("Input is not valid. Please choose again");
                    return GetChosedBrewery(breweries);
                }

            if (breweries.Embedded.Breweries.All(x => x.Id != selectedBreweryId))
            {
                Console.WriteLine("Input is not valid. Please choose again");
                return GetChosedBrewery(breweries);
            }

                var selectedBrewery = breweries.Embedded.Breweries.Single(x => x.Id == selectedBreweryId);

                return selectedBrewery;
            }

            private static ClassBeer GetBeers(Brewery selectedBrewery)
            {
  
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/hal+json");
                    var result = httpClient.GetStringAsync(new Uri(BreweriesUri, selectedBrewery.BeersLinks.Beers.Href)).Result;
                    return ReadToObject<ClassBeer>(result);
                }
            }

        private static T ReadToObject<T>(string json) where T : class
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                var obj = serializer.ReadObject(ms) as T;
                return obj;
            }
        }
        private static void DisplayBeers(ClassBeer beers)
            {
                Console.WriteLine();
                Console.WriteLine("Available beers: ");
                foreach (var beer in beers.Embedded.Beers)
                {
                    Console.WriteLine(beer.Id + " - " + beer.Name);
                }
            }

            private static Beer GetChosedBeer(ClassBeer beers)
            {
                Console.WriteLine();
                Console.Write("Please select beer: ");
                
                if (!int.TryParse(Console.ReadLine(), out int selectedBeerId))
                {
                    Console.WriteLine("Input is not valid. Please choose again");
                    return GetChosedBeer(beers);
                }

            if (beers.Embedded.Beers.All(x => x.Id != selectedBeerId))
            {
                Console.WriteLine("Input is not valid. Please choose again");
                return GetChosedBeer(beers);
            }
           

            var selectedBeer = beers.Embedded.Beers.Single(x => x.Id == selectedBeerId);

                return selectedBeer;
            }

            private static void DisplayBeerDetails(Beer beer)
            {
                Console.WriteLine();
                Console.WriteLine("Beer name: " + beer.Name);
                Console.WriteLine("Style: " + beer.Style);
            }
        }
}

