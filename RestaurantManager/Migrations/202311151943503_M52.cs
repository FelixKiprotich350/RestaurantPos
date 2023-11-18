namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M52 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserActivityLog", "Parameters", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserActivityLog", "Parameters", c => c.String(nullable: false, unicode: false));
        }
    }
}
