namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetItem",
                c => new
                    {
                        AssetItemGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        AssetName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        AssetDepartment = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        AssetDescription = c.String(nullable: false, unicode: false),
                        AssetGroupGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UOM = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        typeofasset = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        AssetItemCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InStockQuantity = c.Int(nullable: false),
                        IsPrecount = c.Boolean(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false, precision: 0),
                        LastUpdateDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.AssetItemGuid);
            
            CreateTable(
                "dbo.AssetItemFlow",
                c => new
                    {
                        AssetFlowGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        AssetItemGuid = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        AssetName = c.String(nullable: false, unicode: false),
                        FlowDirection = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OutTransactionCode = c.String(nullable: false, unicode: false),
                        InTransactionCode = c.String(nullable: false, unicode: false),
                        TransactionDate = c.DateTime(nullable: false, precision: 0),
                        IsCancelled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AssetFlowGuid);
            
            CreateTable(
                "dbo.MeasureUnit",
                c => new
                    {
                        UnitGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UnitName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UnitShortcode = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        RegistrationDate = c.DateTime(nullable: false, precision: 0),
                        LastUpdateDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.UnitGuid)
                .Index(t => t.UnitName, unique: true);
            
            AlterColumn("dbo.StockFlowTransaction", "FlowDirection", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropIndex("dbo.MeasureUnit", new[] { "UnitName" });
            AlterColumn("dbo.StockFlowTransaction", "FlowDirection", c => c.String(maxLength: 100, storeType: "nvarchar"));
            DropTable("dbo.MeasureUnit");
            DropTable("dbo.AssetItemFlow");
            DropTable("dbo.AssetItem");
        }
    }
}
