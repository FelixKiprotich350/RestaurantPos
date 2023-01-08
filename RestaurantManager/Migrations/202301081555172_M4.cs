namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M4 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.DiscountedItem");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DiscountedItem",
                c => new
                    {
                        ItemRowGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        BatchNumber = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        ProductItemGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.ItemRowGuid);
            
        }
    }
}
