using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Api
{
    public class DateSenzori
    {
        [JsonProperty("Zona")]
        public int Zona { get; set; }

        [JsonProperty("Data")]
        public DateTime Data { get; set; }

        [JsonProperty("Temperatura")]
        public double Temperatura { get; set; }

        [JsonProperty("Umiditate")]
        public double Umiditate { get; set; }

    }
}
