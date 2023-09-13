namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m6 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PosUser", new[] { "UserRole" });
            AddColumn("dbo.PosUser", "PhoneNumber", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PosUser", "PhoneNumber");
            CreateIndex("dbo.PosUser", "UserRole");
        }
    }
}
