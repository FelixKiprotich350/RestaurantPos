namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoicesMaster", "SystemUser", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AddColumn("dbo.InvoicesMaster", "CustomerAccNo", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            DropColumn("dbo.InvoicesMaster", "ReceivedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InvoicesMaster", "ReceivedBy", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            DropColumn("dbo.InvoicesMaster", "CustomerAccNo");
            DropColumn("dbo.InvoicesMaster", "SystemUser");
        }
    }
}
