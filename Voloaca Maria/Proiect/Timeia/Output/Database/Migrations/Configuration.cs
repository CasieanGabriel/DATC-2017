namespace Database.Migrations
{
    using Output.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Output.Models.ParkingContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Output.Models.ParkingContext context)
        {
            context.ParkingLots.AddOrUpdate(new ParkingLot() { BottomLeftLat = 45.750548m, BottomLeftLng = 21.225330m, BottomRightLat= 45.750566m, BottomRightLng = 21.225352m, TopRightLat= 45.750594m, TopRightLng = 21.225300m, TopLeftLat= 45.750577m, TopLeftLng = 21.225277m, Status = true });
            context.ParkingLots.AddOrUpdate(new ParkingLot() { BottomLeftLat = 45.750510m, BottomLeftLng = 21.225330m, BottomRightLat = 45.750528m, BottomRightLng = 21.225357m, TopRightLat = 45.750558m, TopRightLng = 21.225306m, TopLeftLat = 45.750541m, TopLeftLng = 21.225279m, Status = false });
            context.ParkingLots.AddOrUpdate(new ParkingLot() { BottomLeftLat = 45.750468m, BottomLeftLng = 21.225336m, BottomRightLat = 45.750489m, BottomRightLng = 21.225362m, TopRightLat = 45.750516m, TopRightLng = 21.225316m, TopLeftLat = 45.750499m, TopLeftLng = 21.225288m, Status = false });
          //  context.ParkingLots.AddOrUpdate(new ParkingLot() { BottomLeftLat = 45.750334m, BottomLeftLng = 21.225409m, BottomRightLat = 45.750345m, BottomRightLng = 21.225418m, TopRightLat = 45.750362m, TopRightLng = 21.225381m, TopLeftLat = 45.750349m, TopLeftLng = 21.225366m, Status = false });
        }
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }

