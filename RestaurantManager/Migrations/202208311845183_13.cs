namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PosUser", "UserName", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.TicketPaymentItem", "PaymentDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.TicketPaymentItem", "ReceivingUsername", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.PosUser", "UserName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PosUser", new[] { "UserName" });
            DropColumn("dbo.TicketPaymentItem", "ReceivingUsername");
            DropColumn("dbo.TicketPaymentItem", "PaymentDate");
            DropColumn("dbo.PosUser", "UserName");
        }
    }
}
