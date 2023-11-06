namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M37 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockFlowTransaction", "StockFlowTrigger", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockFlowTransaction", "StockFlowTrigger");
        }
    }
}
