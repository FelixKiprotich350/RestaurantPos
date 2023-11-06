namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M41 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TicketPaymentItem", "IsVoided");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TicketPaymentItem", "IsVoided", c => c.Boolean(nullable: false));
        }
    }
}
