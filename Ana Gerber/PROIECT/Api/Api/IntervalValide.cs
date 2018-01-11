using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Api
{
    public class IntervalValide
    {
        [JsonProperty("Temperatura_Min")]
        public int Temperatura_Min { get; set; }

        [JsonProperty("Temperatura_Max")]
        public int Temperatura_Max { get; set; }

        [JsonProperty("Umididate_Min")]
        public int Umididate_Min { get; set; }

        [JsonProperty("Umididate_Max")]
        public int Umididate_Max { get; set; }
    }
}
