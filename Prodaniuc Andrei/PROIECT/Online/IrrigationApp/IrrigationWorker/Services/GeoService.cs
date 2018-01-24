using IrrigationWorker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IrrigationWorker.Services
{
    public class GeoService
    {
        private const string url = "http://api.geonames.org/findNearByWeatherJSON?lat={0}&lng={1}&username=aprodaniuc";

        public async Task<string> GetWeatherAsync(string lat, string lng)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(string.Format(url, lat, lng));
            var response = await client.GetAsync(client.BaseAddress);
            var JsonResult = response.Content.ReadAsStringAsync().Result;
            //var weather = JsonConvert.DeserializeObject<WeatherResult>(JsonResult);
            return JsonResult;
        }
    }
}
