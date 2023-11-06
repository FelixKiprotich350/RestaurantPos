namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M31 : DbMigration
    {
        public override void Up()
        {
            //CreateIndex("dbo.WorkPeriod", "WorkperiodName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkPeriod", new[] { "WorkperiodName" });
        }
    }
}
