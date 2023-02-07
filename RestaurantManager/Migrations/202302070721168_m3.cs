namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductCategory", "Department", c => c.String(nullable: false, maxLength: 200, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductCategory", "Department");
        }
    }
}
