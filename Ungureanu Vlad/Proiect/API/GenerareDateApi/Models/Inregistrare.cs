using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerareDateApi.Models
{
    public class Inregistrare
    {
        public int Id { get; set; }
       public int idsenzor { get; set; }
        public int temperatura { get; set; }
        public int umiditate { get; set; }
       public int presiune { get; set; }
        public DateTime data { get; set; }
    }
}
