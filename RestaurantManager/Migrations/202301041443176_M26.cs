namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M26 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItem", "IsGiftItem", c => c.Boolean(nullable: false));
            DropColumn("dbo.OrderItem", "IsDiscountItem");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderItem", "IsDiscountItem", c => c.Boolean(nullable: false));
            DropColumn("dbo.OrderItem", "IsGiftItem");
        }
    }
}
