namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M5 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.PosUser", "UserRole");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PosUser", new[] { "UserRole" });
        }
    }
}
