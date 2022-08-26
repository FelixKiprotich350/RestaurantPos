namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1234567 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MenuProductItem", "CategoryGuid", "dbo.ProductCategory");
            DropIndex("dbo.MenuProductItem", new[] { "CategoryGuid" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.MenuProductItem", "CategoryGuid");
            AddForeignKey("dbo.MenuProductItem", "CategoryGuid", "dbo.ProductCategory", "CategoryGuid", cascadeDelete: true);
        }
    }
}
