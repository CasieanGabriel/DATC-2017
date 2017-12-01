using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GenerareDate_v2
{
    public class LocParcare
    {
        public string Id { get; set; }
        public string StareLoc { get; set; }
        
    }

    public enum Stare { liber, ocupat,gresita }

    class Program
    {
        static HttpClient client = new HttpClient();

        static void ShowProduct(LocParcare product)
        {
            Console.WriteLine($"StareLoc: {product.StareLoc}/t Id: {product.Id}");
        }

        static async Task<Uri> CreateProductAsync(LocParcare product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Parcare", product);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<LocParcare> GetProductAsync(string path)
        {
            LocParcare product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<LocParcare>();
            }
            return product;
        }

        static async Task<LocParcare> UpdateProductAsync(LocParcare product)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/Parcare/{product.Id}", product);
            //response.EnsureSuccessStatusCode();
            if (response.StatusCode==HttpStatusCode.OK)
            {
                // Deserialize the updated product from the response body.
                product = await response.Content.ReadAsAsync<LocParcare>();
                return product;
            }
            else
            {
                return product;
            }
        }


        static void Main()
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://apiparcare20171127012928.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
               
                while (true)
                {
                    //random data
                    Random rnd = new Random();
                    List<LocParcare> l = new List<LocParcare>();
                    int nrRnd = 0;

                    for (int i = 0; i < 23; i++)
                    {
                        nrRnd = rnd.Next(0, 3);
                        LocParcare loc = new LocParcare();
                        loc.Id = (i + 1).ToString();
                        loc.StareLoc = ((Stare)nrRnd).ToString();
                        l.Add(loc);
                    }
                    //System.Threading.Thread.Sleep(3000);
                    // Create a new product
                    //LocParcare product = new LocParcare { StareLoc="ocupat" };

                    //var url = await CreateProductAsync(product);
                    //Console.WriteLine($"Created at {url}");

                    // Get the product
                    //product = await GetProductAsync(url.PathAndQuery);
                    //ShowProduct(product);

                    // Update the product
                    for (int i = 0; i < 23; i++)
                    {
                        Console.WriteLine("Updating loc...");
                        LocParcare product = new LocParcare();
                        //in lista id-ul este marit cu o valoare pentru a coincide cu id_ul din bd unde incepe de la 1
                        product.Id = l[i].Id;
                        product.StareLoc = l[i].StareLoc;
                        await UpdateProductAsync(product);
                    }
                    //a terminat de pus valorile
                    Console.WriteLine("a pus 23 de val");
                    System.Threading.Thread.Sleep(30000);
                }
                //// Get the updated product
                //product = await GetProductAsync(url.PathAndQuery);
                //ShowProduct(product);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

    }
}