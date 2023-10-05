namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "PhoneNumber", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            CreateIndex("dbo.Person", "PhoneNumber", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Person", new[] { "PhoneNumber" });
            AlterColumn("dbo.Person", "PhoneNumber", c => c.String(nullable: false, unicode: false));
        }
    }
}
