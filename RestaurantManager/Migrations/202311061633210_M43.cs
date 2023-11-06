namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M43 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItem", "PostDate", c => c.DateTime(nullable: true, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderItem", "PostDate");
        }
    }
}
