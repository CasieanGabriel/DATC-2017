using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GinBellWorker.
    Model
{
    public class DateDePrelucrat
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Valoare")]
        public int Valoare { get; set; }
    }
}
