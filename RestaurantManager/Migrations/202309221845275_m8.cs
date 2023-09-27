namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockFlowTransaction",
                c => new
                    {
                        TransactionID = c.Int(nullable: false, identity: true),
                        ProductGuid = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        ProductName = c.String(nullable: false, unicode: false),
                        FlowDirection = c.String(maxLength: 100, storeType: "nvarchar"),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OutTransactionCode = c.String(nullable: false, unicode: false),
                        InTransactionCode = c.String(nullable: false, unicode: false),
                        TransactionDate = c.DateTime(nullable: false, precision: 0),
                        IsCancelled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StockFlowTransaction");
        }
    }
}
