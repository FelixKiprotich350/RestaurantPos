namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M40 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("TicketPaymentItem");
            AlterColumn("TicketPaymentItem", "PaymentGuid", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("TicketPaymentItem", "PaymentGuid");
        }
        
        public override void Down()
        {
            DropPrimaryKey("TicketPaymentItem");
            AlterColumn("TicketPaymentItem", "PaymentGuid", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AddPrimaryKey("TicketPaymentItem", "PaymentGuid");
        }
    }
}
