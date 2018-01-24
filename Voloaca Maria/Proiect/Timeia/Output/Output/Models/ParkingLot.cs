using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Output.Models
{
    public class ParkingLot
    {
        public int LotId { get; set; }
        public string Coordinates { get; set; }
        public bool Status { get; set; }
    }
}
