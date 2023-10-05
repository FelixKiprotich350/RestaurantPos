namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M7 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Person", newName: "PersonalAccount");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.PersonalAccount", newName: "Person");
        }
    }
}
