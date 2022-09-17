namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerAccount", "TransactionNo", c => c.String(nullable: false, maxLength: 200, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerAccount", "TransactionNo");
        }
    }
}
