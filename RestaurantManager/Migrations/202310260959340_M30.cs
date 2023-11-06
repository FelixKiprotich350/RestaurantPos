namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M30 : DbMigration
    {
        public override void Up()
        {
           // DropIndex("dbo.WorkPeriod", new[] { "WorkperiodName" });
            CreateIndex("dbo.WorkPeriod", "WorkperiodName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkPeriod", new[] { "WorkperiodName" });
            CreateIndex("dbo.WorkPeriod", "WorkperiodName");
        }
    }
}
