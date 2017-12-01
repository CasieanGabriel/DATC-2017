using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Problema
{
    class HttpUtils
    {
        public HttpRequestMessage CreateRequest(string url, HttpMethod method, List<string> headers)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = method
            };
            headers.ForEach(h => request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(h)));
            return request;
        }

        public JObject ReadData(string url)
        {
            var client = new HttpClient();
            var request = CreateRequest(url, HttpMethod.Get, new List<string>() { "application/hal+json" });
            var response = client.SendAsync(request).Result;

            var data = response.Content.ReadAsStringAsync().Result;
            JObject obj = null;
            try
            {
                obj = (JObject)JsonConvert.DeserializeObject(data);
            }
            catch
            {

            }

            Console.WriteLine(data);
            Console.ReadLine();

            return obj;
        }
    }
}
