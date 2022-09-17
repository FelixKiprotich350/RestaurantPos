namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerAccount", "TransactionType", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AddColumn("dbo.OrderMaster", "CustomerRefference", c => c.String(maxLength: 200, storeType: "nvarchar"));
            DropColumn("dbo.OrderMaster", "CustomerName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderMaster", "CustomerName", c => c.String(maxLength: 200, storeType: "nvarchar"));
            DropColumn("dbo.OrderMaster", "CustomerRefference");
            DropColumn("dbo.CustomerAccount", "TransactionType");
        }
    }
}
