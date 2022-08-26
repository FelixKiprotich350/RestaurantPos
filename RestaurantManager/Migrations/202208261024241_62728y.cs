namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _62728y : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.OrderItem");
            AddColumn("dbo.OrderItem", "ItemRowGuid", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.OrderItem", "ParentProductItemGuid", c => c.String(nullable: false, maxLength: 100));
            AddPrimaryKey("dbo.OrderItem", "ItemRowGuid");
            DropColumn("dbo.OrderItem", "ItemGuid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderItem", "ItemGuid", c => c.String(nullable: false, maxLength: 100));
            DropPrimaryKey("dbo.OrderItem");
            DropColumn("dbo.OrderItem", "ParentProductItemGuid");
            DropColumn("dbo.OrderItem", "ItemRowGuid");
            AddPrimaryKey("dbo.OrderItem", "ItemGuid");
        }
    }
}
