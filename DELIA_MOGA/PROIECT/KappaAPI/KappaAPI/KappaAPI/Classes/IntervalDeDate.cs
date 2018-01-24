using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KappaAPI.Classes
{
    public class IntervalDeDate
    {
        [JsonProperty("TemperatureMin")]
        public int TemperatureMin { get; set; }

        [JsonProperty("TemperatureMax")]
        public int TemperatureMax { get; set; }

        [JsonProperty("HumidityMin")]
        public int HumidityMin { get; set; }

        [JsonProperty("HumidityMax")]
        public int HumidityMax { get; set; }
    }
}
