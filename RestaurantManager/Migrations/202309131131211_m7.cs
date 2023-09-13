namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m7 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.PosUser", "PhoneNumber", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.PosUser", new[] { "PhoneNumber" });
        }
    }
}
