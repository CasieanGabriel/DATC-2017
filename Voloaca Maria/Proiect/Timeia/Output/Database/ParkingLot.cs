using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Output.Models
{
    public class ParkingLot
    {
        [Key]
        public int LotId { get; set; }
        public decimal TopLeftLat { get; set; }
        public decimal TopLeftLng { get; set; }
        public decimal TopRightLat { get; set; }
        public decimal TopRightLng { get; set; }
        public decimal BottomRightLat { get; set; }
        public decimal BottomRightLng { get; set; }
        public decimal BottomLeftLat { get; set; }
        public decimal BottomLeftLng { get; set; }

        public bool Status { get; set; }
    }
}
