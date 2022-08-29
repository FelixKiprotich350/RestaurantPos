
namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M4 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.UserPermission");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserPermission",
                c => new
                    {
                        PermissionID = c.Int(nullable: false, identity: true),
                        UserGuid = c.String(nullable: false, maxLength: 100),
                        ParentPermissionGuid = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.PermissionID);
            
        }
    }
}
