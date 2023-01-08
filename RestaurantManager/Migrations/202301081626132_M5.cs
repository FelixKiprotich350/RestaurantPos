namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestModel",
                c => new
                    {
                        testkey = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.testkey);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TestModel");
        }
    }
}
