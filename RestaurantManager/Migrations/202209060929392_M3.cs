namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserRole", "RoleStatus", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserRole", "RoleStatus");
        }
    }
}
