namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M26 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StockFlowTransaction", "TransactionDate", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StockFlowTransaction", "TransactionDate", c => c.String(nullable: false, unicode: false));
        }
    }
}
