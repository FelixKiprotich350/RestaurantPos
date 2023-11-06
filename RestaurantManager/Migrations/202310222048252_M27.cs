namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M27 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockTakingMaster",
                c => new
                    {
                        ItemGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        STTNumber = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        Notes = c.String(nullable: false, unicode: false),
                        OpeningDate = c.DateTime(nullable: false, precision: 0),
                        ClosingDate = c.DateTime(nullable: false, precision: 0),
                        OpenedBy = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        ClosedBy = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.ItemGuid);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StockTakingMaster");
        }
    }
}
