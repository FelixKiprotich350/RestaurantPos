namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientInfo",
                c => new
                    {
                        ClientGuid = c.String(nullable: false, maxLength: 100),
                        ClientTitle = c.String(nullable: false, maxLength: 100),
                        PhysicalAddress = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(nullable: false, maxLength: 100),
                        ClientKRAPIN = c.String(nullable: false, maxLength: 100),
                        ReceiptNote1 = c.String(nullable: false, maxLength: 100),
                        ReceiptNote2 = c.String(nullable: false, maxLength: 100),
                        ThankYouNote = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ClientGuid)
                .Index(t => t.Email);
            
            CreateTable(
                "dbo.TableEntity",
                c => new
                    {
                        TableGuid = c.String(nullable: false, maxLength: 100),
                        TableName = c.String(nullable: false, maxLength: 100),
                        TableStatus = c.String(nullable: false, maxLength: 100),
                        IsDeleted = c.String(nullable: false, maxLength: 100),
                        RegistrationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TableGuid)
                .Index(t => t.IsDeleted);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TableEntity", new[] { "IsDeleted" });
            DropIndex("dbo.ClientInfo", new[] { "Email" });
            DropTable("dbo.TableEntity");
            DropTable("dbo.ClientInfo");
        }
    }
}
