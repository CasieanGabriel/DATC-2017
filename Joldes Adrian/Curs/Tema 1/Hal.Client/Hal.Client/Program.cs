using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace Hal.Client
{
    class Program
    {
        static void Main(string[] args)
        {
                int option;
            do
            {
                Console.Clear();
                showMainMenu();
                option= getOption();
                swithOnGivenOption(option);
            } while (option != 3);
          
        }

        private static void swithOnGivenOption(int option)
        {
            switch (option)
            {
                case 1:
                    navigate();
                    break;
                case 2:
                    addABeer();
                    break;
                case 3:
                    return;
            }
        }

        private static void addABeer()
        {
            var client = new HttpClient();
            Console.Clear();
            Console.WriteLine("Add beer ");
            Console.WriteLine("Beer name: ");
            string beerNameToAdd = Console.ReadLine();

            string beer = "{\"Name\":\"" + beerNameToAdd + "\"}";
            var postResponse = client.PostAsJsonAsync("http://datc-rest.azurewebsites.net/beers", beer);
        }

        private static void navigate()
        {
            string uri = "http://datc-rest.azurewebsites.net";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/hal+json");
            string entryPoint = uri + "/breweries/";
            var response = client.GetAsync(entryPoint).Result;

            var data = response.Content.ReadAsStringAsync().Result;
            var result = (JObject)JsonConvert.DeserializeObject(data);
            var breweriesInfo = (RootObject)result;

            int breweryNumber = breweriesInfo.Embedded.Brewery.Count();
            int breweryId = 0;
            var beerInfo = new RootObjectBeers(0, 0, 0, new BeerEmbedded(null));
            int beerId = 0;

            do
            {
                Console.Clear();
                Console.WriteLine("Informations about " + breweryNumber + "breweries");
                Console.WriteLine("Choose one to explore? (1, 2, 3... or " + breweryNumber + ")");
                Console.Write("Your option: ");
                breweryId = Int32.Parse(Console.ReadLine());

            } while (breweryId <= 0 || breweryId > breweryNumber);

            Console.WriteLine();
            Console.WriteLine("Brewery name: " + breweriesInfo.Embedded.Brewery[breweryId-1].Name);
            Console.Write("Press any key...");
            Console.ReadLine();


            do
            {
                Console.Clear();
                Console.WriteLine("========== List of beers from brewery with id " + breweryId + "===========");
                string apiBeers = uri + breweriesInfo.Embedded.Brewery[breweryId - 1].Links.Beers.Href;
                var responseApi = client.GetAsync(apiBeers).Result;
                var dataApi = responseApi.Content.ReadAsStringAsync().Result;
                var resultApi = (JObject)JsonConvert.DeserializeObject(dataApi);

                beerInfo = (RootObjectBeers)resultApi;

                Console.WriteLine("There are " + beerInfo.Embedded.Beer.Count()+ " available beers in the brewery " + breweriesInfo.Embedded.Brewery[breweryId-1].Name);
                Console.WriteLine("Choose one to explore ");
                Console.Write("Your choice: ");
                beerId = Int32.Parse(Console.ReadLine());
            } while (beerId <= 0 || beerId > beerInfo.Embedded.Beer.Count());

            Console.WriteLine();
            Console.WriteLine("Beer name: " + beerInfo.Embedded.Beer[beerId - 1].Name);
            Console.WriteLine("Beer style: " + beerInfo.Embedded.Beer[beerId - 1].StyleName);
            findBeersWithSameStyle(beerId, uri, client, beerInfo);
            Console.Write("Press any key...");
            Console.ReadLine();
        }

        private static void findBeersWithSameStyle(int beerId, string uri, HttpClient client, RootObjectBeers beerInfo)
        {

            string entryPoint = uri + beerInfo.Embedded.Beer[beerId - 1].Links.Style.Href;
            var response = client.GetAsync(entryPoint).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            var results = (JObject)JsonConvert.DeserializeObject(data);
            int totalNumberOfBeers = (int)results["totalResults"];

            if (totalNumberOfBeers == 0)
            {
                Console.WriteLine("No beer with this syle");
            }
            else if (totalNumberOfBeers >= 1)
            {
                Console.WriteLine(totalNumberOfBeers + "beer(s) with this style");
            }
        }

        private static int getOption()
        {
            int option = -1;
            bool readOk = true;
            do
            {
                try
                {
                    option = Int32.Parse(Console.ReadLine());

                    if (!(option > 0 && option <= 3))
                    {
                        readOk = false;
                    }
                    else
                    {
                        readOk = true;
                    }
                }
                catch
                {
                    Console.WriteLine("Wrong input !!! Enter again your option: ");
                    readOk = false;
                }

                if (!readOk)
                {
                    Console.WriteLine("Give a valid input: ");
                }

            } while (!readOk);
            return option;
        }

        private static void showMainMenu()
        {
            Console.WriteLine("=============================== Menu ============================");
            Console.WriteLine("1. Navigate ");
            Console.WriteLine("2. Add a beer ");
            Console.WriteLine("3. Exit ");
            Console.WriteLine("=================================================================");
            Console.WriteLine("Enter your option: ");
        }
    }
}
