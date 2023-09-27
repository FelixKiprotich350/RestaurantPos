namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItem", "BuyingPriceTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderItem", "BuyingPriceTotal");
        }
    }
}
