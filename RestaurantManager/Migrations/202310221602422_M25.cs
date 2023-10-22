namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M25 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StockFlowTransaction", "TransactionDate", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StockFlowTransaction", "TransactionDate", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
        }
    }
}
