using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DATC_API.Models
{
    public class DateFinale
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Valoare")]
        public int Valoare { get; set; }
    }
}