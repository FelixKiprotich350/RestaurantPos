namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M25 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DiscountItem",
                c => new
                    {
                        DiscRowID = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ProductGuid = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        DiscType = c.String(maxLength: 100, storeType: "nvarchar"),
                        AttachedProduct = c.String(nullable: false, unicode: false),
                        DiscPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false, precision: 0),
                        EndDate = c.DateTime(nullable: false, precision: 0),
                        OfferDay = c.String(nullable: false, unicode: false),
                        IsRepetitive = c.Boolean(nullable: false),
                        DiscStatus = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.DiscRowID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DiscountItem");
        }
    }
}
