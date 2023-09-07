namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuProductItem", "RemainingQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MenuProductItem", "IsPrecount", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuProductItem", "IsPrecount");
            DropColumn("dbo.MenuProductItem", "RemainingQuantity");
        }
    }
}
