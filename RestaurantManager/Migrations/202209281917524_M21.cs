namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M21 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PosUser", "UserFullName", c => c.String(nullable: false, unicode: false));
            AlterColumn("dbo.PosUser", "UserRights", c => c.String(nullable: false, maxLength: 1000, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PosUser", "UserRights", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AlterColumn("dbo.PosUser", "UserFullName", c => c.String(nullable: false, maxLength: 500, storeType: "nvarchar"));
        }
    }
}
