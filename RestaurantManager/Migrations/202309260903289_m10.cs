namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItem", "BuyingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.OrderItem", "SellingPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderItem", "SellingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.OrderItem", "BuyingPrice");
        }
    }
}
