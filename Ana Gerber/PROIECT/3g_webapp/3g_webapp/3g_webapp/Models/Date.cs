using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 3G_WebApp.Models
{
    public class Date
    {
        [JsonProperty("Latitudine")]
        public double Latitude { get; set; }

        [JsonProperty("Longitudine")]
        public double Longitude { get; set; }

        [JsonProperty("Temperatura")]
        public double Temperature { get; set; }

        [JsonProperty("Umiditate")]
        public double Humidity { get; set; }

        [JsonProperty("Data")]
        public DateTime Data { get; set; }

        [JsonProperty("NevoieIrigare")]
        public string NeedIrigation { get; set; }
    }

}