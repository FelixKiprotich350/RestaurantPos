namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M8 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ClientInfo", newName: "ClientInfoDetails");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ClientInfoDetails", newName: "ClientInfo");
        }
    }
}
