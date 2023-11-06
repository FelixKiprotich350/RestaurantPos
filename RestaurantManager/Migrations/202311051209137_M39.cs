namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M39 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketPaymentItem", "ParentSourceRef", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AddColumn("dbo.TicketPaymentItem", "MasterTransNo", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AddColumn("dbo.TicketPaymentItem", "PayForSource", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            DropColumn("dbo.TicketPaymentItem", "ParentOrderNo");
            DropColumn("dbo.TicketPaymentItem", "ParentTransNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TicketPaymentItem", "ParentTransNo", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AddColumn("dbo.TicketPaymentItem", "ParentOrderNo", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            DropColumn("dbo.TicketPaymentItem", "PayForSource");
            DropColumn("dbo.TicketPaymentItem", "MasterTransNo");
            DropColumn("dbo.TicketPaymentItem", "ParentSourceRef");
        }
    }
}
