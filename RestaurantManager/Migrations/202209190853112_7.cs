namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuProductItem", "RemainingQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.OrderMaster", "IsPrinted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderMaster", "IsPrinted");
            DropColumn("dbo.MenuProductItem", "RemainingQuantity");
        }
    }
}
