namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2222 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TicketPaymentItem",
                c => new
                    {
                        PaymentGuid = c.String(nullable: false, maxLength: 100),
                        ParentOrderNo = c.String(nullable: false, maxLength: 100),
                        ParentPaymasterGuid = c.String(nullable: false, maxLength: 100),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Method = c.String(nullable: false, maxLength: 100),
                        MethodRefference = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.PaymentGuid);
            
            CreateTable(
                "dbo.TicketPaymentMaster",
                c => new
                    {
                        PaymentMasterGuid = c.String(nullable: false, maxLength: 100),
                        TicketNo = c.String(nullable: false, maxLength: 100),
                        TotalAmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmountCharged = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TicketBalanceReturned = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentDate = c.DateTime(nullable: false),
                        PosUser = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.PaymentMasterGuid);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TicketPaymentMaster");
            DropTable("dbo.TicketPaymentItem");
        }
    }
}
