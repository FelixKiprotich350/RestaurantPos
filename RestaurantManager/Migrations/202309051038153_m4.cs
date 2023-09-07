namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MenuProductItem", "RemainingQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MenuProductItem", "RemainingQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
