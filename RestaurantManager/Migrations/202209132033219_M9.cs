namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VoucherCard",
                c => new
                    {
                        VoucherGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        VoucherNumber = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        VoucherBatch = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        VoucherStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        VoucherAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VoucherCreationDate = c.DateTime(nullable: false, precision: 0),
                        VoucherUsageDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.VoucherGuid);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VoucherCard");
        }
    }
}
