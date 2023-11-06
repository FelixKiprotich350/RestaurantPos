namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M28 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockTakingMaster", "Status", c => c.String(nullable: false, maxLength: 200, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockTakingMaster", "Status");
        }
    }
}
