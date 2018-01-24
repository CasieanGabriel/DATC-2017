using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GinBellWorker.Model
{
    class Date
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Valoare")]
        public double Valoare { get; set; }

        [JsonProperty("Data")]
        public DateTime Data { get; set; }
    }
}
