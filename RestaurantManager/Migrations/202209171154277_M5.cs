namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketPaymentMaster", "WorkPeriod", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketPaymentMaster", "WorkPeriod");
        }
    }
}
