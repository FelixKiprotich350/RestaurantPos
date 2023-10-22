namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M20 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.InvoicesMaster", "InvoicePaymentStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InvoicesMaster", "InvoicePaymentStatus", c => c.String(nullable: false, unicode: false));
        }
    }
}
