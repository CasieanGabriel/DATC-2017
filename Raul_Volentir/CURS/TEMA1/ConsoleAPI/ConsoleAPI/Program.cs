using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ConsoleAPI
{
    class Program
    {
        static int Menu()
        {
            int opt;
            Console.WriteLine("\n***Menu***");
            Console.WriteLine("1.Navigate\n2.Add a beer\n0.Exit");
            opt = Int32.Parse(Console.ReadLine());
            return opt;

        }




        static void GETfunction()
        {

            using (var client = new HttpClient())
            {
                //var client = new HttpClient();
                client.BaseAddress = new Uri("http://datc-rest.azurewebsites.net");
                client.DefaultRequestHeaders.Add("accept", "application/hal+json");
                string entrypoint = client.BaseAddress + "/breweries/";
                HttpResponseMessage res = client.GetAsync(entrypoint).Result;
                var data = res.Content.ReadAsStringAsync().Result;
                var result = (JObject)JsonConvert.DeserializeObject(data);
                var infoBreweries = (ClassOfRoot)result;
                int countOfBrewery = infoBreweries._embedded.brewery.Count();
                Console.WriteLine("The total number of Breweries:\t" + countOfBrewery);
                for (int j = 0; j < countOfBrewery; j++)
                    Console.WriteLine("\t" + infoBreweries._embedded.brewery[j].Id + "\t " + infoBreweries._embedded.brewery[j].Name);
                Console.Write("\nPlease, type the id number of your wanted brewery: \t");
                int IdOfBrewey = Int32.Parse(Console.ReadLine());
                Console.WriteLine(
                    infoBreweries._embedded.brewery[IdOfBrewey - 1].Name);

                Console.Write("\n Menu of " + infoBreweries._embedded.brewery[IdOfBrewey - 1].Name);

                string Beers = client.BaseAddress + infoBreweries._embedded.brewery[IdOfBrewey - 1]._links.beers.href;
                var response2 = client.GetAsync(Beers).Result;
                var data2 = response2.Content.ReadAsStringAsync().Result;
                var result2 = (JObject)JsonConvert.DeserializeObject(data2);
                var infoOfBeer = (Root)result2;
                Console.WriteLine("\t***There are " + infoOfBeer._embedded.beer.Count() + " beers***");
                for (int i = 0; i < infoOfBeer._embedded.beer.Count(); i++)
                    Console.WriteLine("\t" + i + " " + infoOfBeer._embedded.beer[i].Name);
                Console.WriteLine("\nPlease, type the number of the beer that you want to check:\t");
                int IDBeer = Int32.Parse(Console.ReadLine());
                Console.WriteLine("\nName\t" + infoOfBeer._embedded.beer[IDBeer].Name
                    + "\nStyle ID\t" + infoOfBeer._embedded.beer[IDBeer].StyleId
                    + "\nStyle Name\t" + infoOfBeer._embedded.beer[IDBeer].StyleName);
            }
        }
        static void POSTfunction()
        {
            using (var client = new HttpClient())
            {

                Console.Write("Please add the name of the beer you want to add:\t");
                string NewBeer = Console.ReadLine();

                string beer = "{\"Name\":\"" + NewBeer + "\"}";
                var postResp = client.PostAsJsonAsync("http://datc-rest.azurewebsites.net/beers", beer);
            }
        }

        static void Main(string[] args)
        {
            int opt;
            while ((opt = Menu()) != 0)
            {
                switch (opt)
                {
                    case 1:
                        GETfunction();
                        break;
                    case 2:
                        POSTfunction();
                        break;

                }


            }

        }
    }
}
