namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TableEntity", "TableName", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            CreateIndex("dbo.TableEntity", "TableName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.TableEntity", new[] { "TableName" });
            AlterColumn("dbo.TableEntity", "TableName", c => c.String(unicode: false));
        }
    }
}
