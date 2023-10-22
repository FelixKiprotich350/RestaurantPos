namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersonalAccount", "NationalID", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            CreateIndex("dbo.PersonalAccount", "NationalID", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.PersonalAccount", new[] { "NationalID" });
            DropColumn("dbo.PersonalAccount", "NationalID");
        }
    }
}
