namespace Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ParkingLots",
                c => new
                    {
                        LotId = c.Int(nullable: false, identity: true),
                        TopLeftLat = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TopLeftLng = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TopRightLat = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TopRightLng = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BottomRightLat = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BottomRightLng = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BottomLeftLat = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BottomLeftLng = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LotId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ParkingLots");
        }
    }
}
