namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PosUser", "UserIsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PosUser", "UserIsDeleted", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
