namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderMaster", "MergedChild", c => c.String(maxLength: 100, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderMaster", "MergedChild");
        }
    }
}
