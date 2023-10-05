namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerAccount", "LastUpdatedBy", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            DropColumn("dbo.CustomerAccount", "PersonalAccNo");
            DropColumn("dbo.CustomerAccount", "UpdatedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomerAccount", "UpdatedBy", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AddColumn("dbo.CustomerAccount", "PersonalAccNo", c => c.String(nullable: false, maxLength: 200, storeType: "nvarchar"));
            DropColumn("dbo.CustomerAccount", "LastUpdatedBy");
        }
    }
}
