namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItemVoided",
                c => new
                    {
                        ItemRowGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ParentProductItemGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OrderID = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ItemName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        VoidReason = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        ApprovedBy = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        WorkPeriod = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        VoidTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.ItemRowGuid);
            
            DropColumn("dbo.OrderItem", "IsItemVoied");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderItem", "IsItemVoied", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            DropTable("dbo.OrderItemVoided");
        }
    }
}
