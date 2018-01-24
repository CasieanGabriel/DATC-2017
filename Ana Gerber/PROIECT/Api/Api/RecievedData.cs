using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Api
{
    public class RecievedData
    {
        [JsonProperty("Latitudine")]
        public double Latitudine { get; set; }

        [JsonProperty("Longitudine")]
        public double Longitudine { get; set; }

        [JsonProperty("Temperatura")]
        public double Temperatura { get; set; }

        [JsonProperty("Umiditate")]
        public double Umiditate { get; set; }

        [JsonProperty("Data")]
        public DateTime DataInregistrare { get; set; }

        [JsonProperty("NevoieIrigare")]
        public string NevoieIrigare { get; set; }
    }
}
