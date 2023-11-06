namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M35 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketPaymentItem", "AmountUsed", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TicketPaymentItem", "PaymentBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketPaymentItem", "PaymentBalance");
            DropColumn("dbo.TicketPaymentItem", "AmountUsed");
        }
    }
}
