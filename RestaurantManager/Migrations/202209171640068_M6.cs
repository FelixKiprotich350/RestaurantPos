namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MailingProfile",
                c => new
                    {
                        RowGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ProfileName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        DisplayName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        DestinationAddress = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        SenderAddress = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        MailingAddress = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        AppPassword = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.RowGuid);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MailingProfile");
        }
    }
}
