namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VouchersBatch", "BulkSalesLimitAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.VouchersBatch", "StartDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.VouchersBatch", "EndDate", c => c.DateTime(nullable: false, precision: 0));
            DropColumn("dbo.VouchersBatch", "VouchersCount");
            DropColumn("dbo.VouchersBatch", "ExpiryDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VouchersBatch", "ExpiryDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.VouchersBatch", "VouchersCount", c => c.Int(nullable: false));
            DropColumn("dbo.VouchersBatch", "EndDate");
            DropColumn("dbo.VouchersBatch", "StartDate");
            DropColumn("dbo.VouchersBatch", "BulkSalesLimitAmount");
        }
    }
}
