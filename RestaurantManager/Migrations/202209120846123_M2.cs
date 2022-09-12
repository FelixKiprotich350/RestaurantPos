namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangeLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntityName = c.String(unicode: false),
                        PropertyName = c.String(unicode: false),
                        PrimaryKeyValue = c.String(unicode: false),
                        OldValue = c.String(unicode: false),
                        NewValue = c.String(unicode: false),
                        DateChanged = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChangeLog");
        }
    }
}
