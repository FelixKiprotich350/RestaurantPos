namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketPaymentItem", "ParentTransNo", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            DropColumn("dbo.TicketPaymentItem", "ParentPaymasterGuid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TicketPaymentItem", "ParentPaymasterGuid", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            DropColumn("dbo.TicketPaymentItem", "ParentTransNo");
        }
    }
}
