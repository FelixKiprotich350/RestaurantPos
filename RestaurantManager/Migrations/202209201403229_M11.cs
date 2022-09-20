namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderMaster", "IsKitchenServed", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderMaster", "IsInPreparation", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderMaster", "IsInPreparation");
            DropColumn("dbo.OrderMaster", "IsKitchenServed");
        }
    }
}
