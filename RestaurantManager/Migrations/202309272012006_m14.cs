namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m14 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "MeasureUnit", newName: "AssetUOM");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.AssetUOM", newName: "MeasureUnit");
        }
    }
}
