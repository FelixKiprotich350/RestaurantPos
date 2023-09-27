namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItem", "SellingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderItem", "SellingPrice");
        }
    }
}
