namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        CustomerName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        CustomerNumber = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        PhoneNumber = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        CustomerEmail = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Gender = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        RegistrationDate = c.DateTime(nullable: false, precision: 0),
                        BirthDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.CustomerGuid);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Customer");
        }
    }
}
