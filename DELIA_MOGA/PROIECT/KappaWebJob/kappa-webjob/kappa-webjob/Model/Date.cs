using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace kappa_webjob.Model
{
    public class Date
    {
        [JsonProperty("Lat")]
        public double Lat { get; set; }

        [JsonProperty("Lng")]
        public double Lng { get; set; }

        [JsonProperty("Temperature")]
        public double Temperature { get; set; }

        [JsonProperty("Humidity")]
        public double Humidity { get; set; }

        [JsonProperty("Data")]
        public DateTime Data { get; set; }

        [JsonProperty("NeedIrigation")]
        public string NeedIrigation { get; set; }
    }
}
