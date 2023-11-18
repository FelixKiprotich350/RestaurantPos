namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M51 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserActivityLog",
                c => new
                    {
                        LogID = c.Int(nullable: false, identity: true),
                        Logtype = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        LogLevel = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Description = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Parameters = c.String(nullable: false, unicode: false),
                        SystemUser = c.String(nullable: false, unicode: false),
                        Timmestamp = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.LogID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserActivityLog");
        }
    }
}
