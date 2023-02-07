namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m5 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ProductCategory", "CategoryName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProductCategory", new[] { "CategoryName" });
        }
    }
}
