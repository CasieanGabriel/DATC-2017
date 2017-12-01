using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingDataGenerator
{
    public enum Stare
    {
        liber,
        ocupat
    }
    class LocParcare
    {
        private string id;
        private string stare;

        public string Id { get => id; set => id = value; }
        public string Stare { get => stare; set => stare = value; }
    }
}
