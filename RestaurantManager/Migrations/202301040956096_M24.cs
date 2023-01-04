namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItem", "IsDiscountItem", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderItem", "ParentItem", c => c.String(nullable: false, unicode: false));
            AddColumn("dbo.OrderItem", "DiscPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.OrderItem", "VoidReason");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderItem", "VoidReason", c => c.String(nullable: false, maxLength: 500, storeType: "nvarchar"));
            DropColumn("dbo.OrderItem", "DiscPercent");
            DropColumn("dbo.OrderItem", "ParentItem");
            DropColumn("dbo.OrderItem", "IsDiscountItem");
        }
    }
}
