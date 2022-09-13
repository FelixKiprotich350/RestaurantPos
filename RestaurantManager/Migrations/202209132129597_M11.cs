namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customer", "CustomerEmail", c => c.String(maxLength: 200, storeType: "nvarchar"));
            DropColumn("dbo.Customer", "CustomerNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customer", "CustomerNumber", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AlterColumn("dbo.Customer", "CustomerEmail", c => c.String(nullable: false, maxLength: 200, storeType: "nvarchar"));
        }
    }
}
