namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M36 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockFlowTransaction", "PrimaryRefference", c => c.String(nullable: false, unicode: false));
            AddColumn("dbo.StockFlowTransaction", "SecondaryRefference", c => c.String(nullable: false, unicode: false));
            DropColumn("dbo.StockFlowTransaction", "OutTransactionCode");
            DropColumn("dbo.StockFlowTransaction", "InTransactionCode");
            DropColumn("dbo.StockFlowTransaction", "STTNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StockFlowTransaction", "STTNumber", c => c.String(nullable: false, unicode: false));
            AddColumn("dbo.StockFlowTransaction", "InTransactionCode", c => c.String(nullable: false, unicode: false));
            AddColumn("dbo.StockFlowTransaction", "OutTransactionCode", c => c.String(nullable: false, unicode: false));
            DropColumn("dbo.StockFlowTransaction", "SecondaryRefference");
            DropColumn("dbo.StockFlowTransaction", "PrimaryRefference");
        }
    }
}
