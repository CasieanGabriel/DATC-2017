using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KappaWebApp.Models
{
    public class SensorData
    {
        public double Lat {get; set;}
        public double Lng { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public DateTime Data { get; set; }
    }

}