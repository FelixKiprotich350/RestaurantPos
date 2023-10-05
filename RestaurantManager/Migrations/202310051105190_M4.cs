namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "AccountNo", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            CreateIndex("dbo.Person", "AccountNo", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Person", new[] { "AccountNo" });
            AlterColumn("dbo.Person", "AccountNo", c => c.String(nullable: false, unicode: false));
        }
    }
}
