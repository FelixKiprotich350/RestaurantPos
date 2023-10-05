namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M6 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CustomerAccount", new[] { "CustAccountNo" });
            AddColumn("dbo.CustomerAccount", "PersonAccNo", c => c.String(nullable: false, maxLength: 200, storeType: "nvarchar"));
            AddColumn("dbo.CustomerAccount", "InvoiceLimit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.CustomerAccount", "PersonAccNo", unique: true);
            DropColumn("dbo.CustomerAccount", "CustAccountNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomerAccount", "CustAccountNo", c => c.String(nullable: false, maxLength: 200, storeType: "nvarchar"));
            DropIndex("dbo.CustomerAccount", new[] { "PersonAccNo" });
            DropColumn("dbo.CustomerAccount", "InvoiceLimit");
            DropColumn("dbo.CustomerAccount", "PersonAccNo");
            CreateIndex("dbo.CustomerAccount", "CustAccountNo", unique: true);
        }
    }
}
