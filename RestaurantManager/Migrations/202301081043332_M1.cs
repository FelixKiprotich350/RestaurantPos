namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItem", "GiftItemGuid", c => c.String(nullable: false, unicode: false));
            AlterColumn("dbo.PosUser", "UserRights", c => c.String(nullable: false, unicode: false));
            DropColumn("dbo.OrderItem", "ParentItem");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderItem", "ParentItem", c => c.String(nullable: false, unicode: false));
            AlterColumn("dbo.PosUser", "UserRights", c => c.String(nullable: false, maxLength: 1000, storeType: "nvarchar"));
            DropColumn("dbo.OrderItem", "GiftItemGuid");
        }
    }
}
