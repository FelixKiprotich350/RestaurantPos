namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M28 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientInfoDetails", "ReceiptNote3", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AddColumn("dbo.ClientInfoDetails", "TaxPercentage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.ClientInfoDetails", "ThankYouNote");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClientInfoDetails", "ThankYouNote", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            DropColumn("dbo.ClientInfoDetails", "TaxPercentage");
            DropColumn("dbo.ClientInfoDetails", "ReceiptNote3");
        }
    }
}
