namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M44 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrderItem", "PostDate", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderItem", "PostDate", c => c.DateTime(precision: 0));
        }
    }
}
