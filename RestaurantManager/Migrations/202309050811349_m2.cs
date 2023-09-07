namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuProductItem", "Department", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AddColumn("dbo.MenuProductItem", "BuyingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MenuProductItem", "SellingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.MenuProductItem", "HouseType");
            DropColumn("dbo.MenuProductItem", "ProductPrice");
            DropColumn("dbo.MenuProductItem", "RemainingQuantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MenuProductItem", "RemainingQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.MenuProductItem", "ProductPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MenuProductItem", "HouseType", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            DropColumn("dbo.MenuProductItem", "SellingPrice");
            DropColumn("dbo.MenuProductItem", "BuyingPrice");
            DropColumn("dbo.MenuProductItem", "Department");
        }
    }
}
