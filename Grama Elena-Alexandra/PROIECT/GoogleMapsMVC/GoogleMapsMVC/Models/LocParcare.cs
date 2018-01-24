using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleMapsMVC.Models
{
    public class LocParcare
    {
        public int Id { get; set; }
        
        public enum Stare { Liber, Ocupat, Lucru }
    }
}