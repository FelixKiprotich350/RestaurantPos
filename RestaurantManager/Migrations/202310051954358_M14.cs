namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M14 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.InvoicePaymentItem", "TransNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InvoicePaymentItem", "TransNo", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
        }
    }
}
