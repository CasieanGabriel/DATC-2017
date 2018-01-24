namespace Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreasedPrecision : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ParkingLots", "TopLeftLat", c => c.Decimal(nullable: false, precision: 18, scale: 10));
            AlterColumn("dbo.ParkingLots", "TopLeftLng", c => c.Decimal(nullable: false, precision: 18, scale: 10));
            AlterColumn("dbo.ParkingLots", "TopRightLat", c => c.Decimal(nullable: false, precision: 18, scale: 10));
            AlterColumn("dbo.ParkingLots", "TopRightLng", c => c.Decimal(nullable: false, precision: 18, scale: 10));
            AlterColumn("dbo.ParkingLots", "BottomRightLat", c => c.Decimal(nullable: false, precision: 18, scale: 10));
            AlterColumn("dbo.ParkingLots", "BottomRightLng", c => c.Decimal(nullable: false, precision: 18, scale: 10));
            AlterColumn("dbo.ParkingLots", "BottomLeftLat", c => c.Decimal(nullable: false, precision: 18, scale: 10));
            AlterColumn("dbo.ParkingLots", "BottomLeftLng", c => c.Decimal(nullable: false, precision: 18, scale: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ParkingLots", "BottomLeftLng", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ParkingLots", "BottomLeftLat", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ParkingLots", "BottomRightLng", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ParkingLots", "BottomRightLat", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ParkingLots", "TopRightLng", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ParkingLots", "TopRightLat", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ParkingLots", "TopLeftLng", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ParkingLots", "TopLeftLat", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
