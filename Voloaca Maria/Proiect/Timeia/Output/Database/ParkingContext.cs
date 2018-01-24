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
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParkingLot>().Property(x => x.BottomLeftLat).HasPrecision(18, 10);
            modelBuilder.Entity<ParkingLot>().Property(x => x.BottomLeftLng).HasPrecision(18, 10);
            modelBuilder.Entity<ParkingLot>().Property(x => x.BottomRightLat).HasPrecision(18, 10);
            modelBuilder.Entity<ParkingLot>().Property(x => x.BottomRightLng).HasPrecision(18, 10);
            modelBuilder.Entity<ParkingLot>().Property(x => x.TopRightLat).HasPrecision(18, 10);
            modelBuilder.Entity<ParkingLot>().Property(x => x.TopRightLng).HasPrecision(18, 10);
            modelBuilder.Entity<ParkingLot>().Property(x => x.TopLeftLat).HasPrecision(18, 10);
            modelBuilder.Entity<ParkingLot>().Property(x => x.TopLeftLng).HasPrecision(18, 10);

        }
    }
}
