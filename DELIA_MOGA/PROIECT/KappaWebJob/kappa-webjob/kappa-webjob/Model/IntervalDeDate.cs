using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace kappa_webjob.Model
{
    public class IntervalDeDate
    {
        [JsonProperty("TemperatureMin")]
        public double TemperatureMin { get; set; }

        [JsonProperty("TemperatureMax")]
        public double TemperatureMax { get; set; }

        [JsonProperty("HumidityMin")]
        public double HumidityMin { get; set; }

        [JsonProperty("HumidityMax")]
        public double HumidityMax { get; set; }
    }
}
