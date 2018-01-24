using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DATC_API.Models
{
    public class ReservationRequest
    {
        public ReservationRequest()
        {

        }

        public ReservationRequest(string ParkingLot, string ParkingSpace)
        {
            this.ParkingLot = ParkingLot;
            this.ParkingSpace = ParkingSpace;
        }

        public string ParkingLot { get; set; }
        public string ParkingSpace { get; set; }
    }
}