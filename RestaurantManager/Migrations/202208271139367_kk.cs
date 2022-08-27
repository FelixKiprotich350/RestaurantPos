namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kk : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.MenuProductItem");
            AlterColumn("dbo.MenuProductItem", "ProductGuid", c => c.String(nullable: false, maxLength: 100));
            AddPrimaryKey("dbo.MenuProductItem", "ProductGuid");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.MenuProductItem");
            AlterColumn("dbo.MenuProductItem", "ProductGuid", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.MenuProductItem", "ProductGuid");
        }
    }
}
