namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OrderMaster", "IsKitchenServed");
            DropColumn("dbo.OrderMaster", "IsInPreparation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderMaster", "IsInPreparation", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderMaster", "IsKitchenServed", c => c.Boolean(nullable: false));
        }
    }
}
