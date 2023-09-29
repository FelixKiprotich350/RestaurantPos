namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m13 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetGroup",
                c => new
                    {
                        GroupGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        GroupName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        CreationDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.GroupGuid)
                .Index(t => t.GroupName, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.AssetGroup", new[] { "GroupName" });
            DropTable("dbo.AssetGroup");
        }
    }
}
