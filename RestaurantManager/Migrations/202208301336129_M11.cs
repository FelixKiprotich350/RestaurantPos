namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M11 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TableEntity", new[] { "IsDeleted" });
            AlterColumn("dbo.TableEntity", "IsDeleted", c => c.Boolean(nullable: false));
            CreateIndex("dbo.TableEntity", "IsDeleted");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TableEntity", new[] { "IsDeleted" });
            AlterColumn("dbo.TableEntity", "IsDeleted", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.TableEntity", "IsDeleted");
        }
    }
}
