namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m15 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AssetItem", "AssetDepartment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AssetItem", "AssetDepartment", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
        }
    }
}
