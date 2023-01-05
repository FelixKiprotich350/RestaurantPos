namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M27 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiscountItem", "ProductName", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DiscountItem", "ProductName");
        }
    }
}
