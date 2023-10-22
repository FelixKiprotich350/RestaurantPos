namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M17 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItemVoided",
                c => new
                    {
                        ItemRowGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ProductItemGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OrderID = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ItemName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        VoidReason = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        SystemUser = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        WorkPeriod = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        VoidTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.ItemRowGuid);
            
            CreateTable(
                "dbo.OrderMasterVoided",
                c => new
                    {
                        OrderGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OrderNo = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        CustomerRefference = c.String(maxLength: 200, storeType: "nvarchar"),
                        TicketTable = c.String(maxLength: 100, storeType: "nvarchar"),
                        SystemUser = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Workperiod = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OrderDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.OrderGuid)
                .Index(t => t.OrderNo);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.OrderMasterVoided", new[] { "OrderNo" });
            DropTable("dbo.OrderMasterVoided");
            DropTable("dbo.OrderItemVoided");
        }
    }
}
