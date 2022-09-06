namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.UserRole", new[] { "RoleName" });
            AlterColumn("dbo.UserRole", "RoleName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserRole", "RoleName", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.UserRole", "RoleName");
        }
    }
}
