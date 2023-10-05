namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M2 : DbMigration
    {
        public override void Up()
        {
            //DropIndex("dbo.Person", new[] { "AccountNo" });
            AlterColumn("dbo.Person", "PhoneNumber", c => c.String(nullable: false, unicode: false));
            AlterColumn("dbo.Person", "AccountNo", c => c.String(nullable: false, unicode: false));
            AlterColumn("dbo.Person", "InvoiceLimit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Person", "InvoiceLimit", c => c.Int(nullable: false));
            AlterColumn("dbo.Person", "AccountNo", c => c.String(nullable: false, maxLength: 200, storeType: "nvarchar"));
            AlterColumn("dbo.Person", "PhoneNumber", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            CreateIndex("dbo.Person", "AccountNo", unique: true);
        }
    }
}
