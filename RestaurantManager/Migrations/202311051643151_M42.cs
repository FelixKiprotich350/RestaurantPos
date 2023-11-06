namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M42 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.InvoicePaymentItem");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.InvoicePaymentItem",
                c => new
                    {
                        PaymentGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        InvoiceNo = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Method = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        PaymentWorkperiod = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        PaymentDate = c.DateTime(nullable: false, precision: 0),
                        ReceivingUsername = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.PaymentGuid);
            
        }
    }
}
