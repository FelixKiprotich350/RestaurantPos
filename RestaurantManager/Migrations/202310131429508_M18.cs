namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M18 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OrderItem", "IsItemVoided");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderItem", "IsItemVoided", c => c.Boolean(nullable: false));
        }
    }
}
