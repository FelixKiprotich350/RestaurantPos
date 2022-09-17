namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PosVariables",
                c => new
                    {
                        RowGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        VarName = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        VarValue1 = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        VarValue2 = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.RowGuid);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PosVariables");
        }
    }
}
