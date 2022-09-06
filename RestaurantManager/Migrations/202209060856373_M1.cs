namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PosUser", "UserRights");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PosUser", "UserRights", c => c.String(nullable: false, maxLength: 500));
        }
    }
}
