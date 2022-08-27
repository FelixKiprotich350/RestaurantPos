namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kk1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ProductCategory");
            AlterColumn("dbo.ProductCategory", "CategoryGuid", c => c.String(nullable: false, maxLength: 100));
            AddPrimaryKey("dbo.ProductCategory", "CategoryGuid");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ProductCategory");
            AlterColumn("dbo.ProductCategory", "CategoryGuid", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.ProductCategory", "CategoryGuid");
        }
    }
}
