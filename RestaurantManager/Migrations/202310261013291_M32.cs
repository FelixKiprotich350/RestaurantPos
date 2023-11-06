namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M32 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.WorkPeriod", new[] { "WorkperiodName" });
        }
        
        public override void Down()
        {
            //CreateIndex("dbo.WorkPeriod", "WorkperiodName", unique: true);
        }
    }
}
