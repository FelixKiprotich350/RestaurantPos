namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketPaymentMaster", "TransNo", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketPaymentMaster", "TransNo");
        }
    }
}
