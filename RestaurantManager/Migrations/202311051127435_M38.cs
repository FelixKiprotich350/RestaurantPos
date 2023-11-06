namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M38 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetItem", "IsFoodMaterial", c => c.Boolean(nullable: false));
            CreateIndex("dbo.TicketPaymentMaster", "TransNo", unique: true);
            DropColumn("dbo.AssetItem", "typeofasset");
            DropColumn("dbo.AssetItem", "AssetItemCost");
            DropColumn("dbo.AssetItem", "InStockQuantity");
            DropColumn("dbo.AssetItem", "IsPrecount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AssetItem", "IsPrecount", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssetItem", "InStockQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.AssetItem", "AssetItemCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AssetItem", "typeofasset", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            DropIndex("dbo.TicketPaymentMaster", new[] { "TransNo" });
            DropColumn("dbo.AssetItem", "IsFoodMaterial");
        }
    }
}
