namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeAccount",
                c => new
                    {
                        AccountGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        PersonAccNo = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        InvoiceLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MonthlySalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegDate = c.DateTime(nullable: false, precision: 0),
                        LastUpdateDate = c.DateTime(nullable: false, precision: 0),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        AccountStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.AccountGuid)
                .Index(t => t.PersonAccNo, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.EmployeeAccount", new[] { "PersonAccNo" });
            DropTable("dbo.EmployeeAccount");
        }
    }
}
