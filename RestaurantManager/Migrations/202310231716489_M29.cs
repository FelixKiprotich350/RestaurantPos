namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M29 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockFlowTransaction", "STTNumber", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockFlowTransaction", "STTNumber");
        }
    }
}
