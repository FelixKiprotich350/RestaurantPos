namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerAccount",
                c => new
                    {
                        AccActionGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        CustomerPhoneNo = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Debit = c.Int(nullable: false),
                        Credit = c.Int(nullable: false),
                        ActionDate = c.DateTime(nullable: false, precision: 0),
                        ApprovedBy = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.AccActionGuid);
            
            DropTable("dbo.CustomerPointsAccount");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CustomerPointsAccount",
                c => new
                    {
                        AccountGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        CustomerPhoneNo = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Debit = c.Int(nullable: false),
                        Credit = c.Int(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.AccountGuid);
            
            DropTable("dbo.CustomerAccount");
        }
    }
}
