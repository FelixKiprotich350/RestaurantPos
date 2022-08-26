namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuProductItem",
                c => new
                    {
                        ProductGuid = c.String(nullable: false, maxLength: 20),
                        ProductName = c.String(nullable: false, maxLength: 100),
                        CategoryCode = c.String(nullable: false, maxLength: 20),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ProductGuid);
            
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        CategoryGuid = c.String(nullable: false, maxLength: 20),
                        CategoryName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.CategoryGuid);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductCategory");
            DropTable("dbo.MenuProductItem");
        }
    }
}
