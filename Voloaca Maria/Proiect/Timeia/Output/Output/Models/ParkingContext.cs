using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
namespace Output.Models
{
    public class ParkingContext : DbContext
    {
        public DbSet<ParkingLot> ParkingLots { get; set; }
    }
}
