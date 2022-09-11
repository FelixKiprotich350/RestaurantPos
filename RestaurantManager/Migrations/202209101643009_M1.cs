namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientInfoDetails",
                c => new
                    {
                        ClientGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ClientTitle = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        PhysicalAddress = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Email = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Phone = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ClientKRAPIN = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ReceiptNote1 = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ReceiptNote2 = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ThankYouNote = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.ClientGuid)
                .Index(t => t.Email);
            
            CreateTable(
                "dbo.MenuProductItem",
                c => new
                    {
                        ProductGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ProductName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        CategoryGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        AvailabilityStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ProductGuid);
            
            CreateTable(
                "dbo.OrderItem",
                c => new
                    {
                        ItemRowGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ParentProductItemGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OrderID = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ItemName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ServiceType = c.String(nullable: false, maxLength: 20, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.ItemRowGuid);
            
            CreateTable(
                "dbo.OrderMaster",
                c => new
                    {
                        OrderGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OrderNo = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OrderStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        CustomerName = c.String(maxLength: 200, storeType: "nvarchar"),
                        TicketTable = c.String(maxLength: 100, storeType: "nvarchar"),
                        UserServing = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OrderDate = c.DateTime(nullable: false, precision: 0),
                        PaymentDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.OrderGuid)
                .Index(t => t.OrderNo);
            
            CreateTable(
                "dbo.PosUser",
                c => new
                    {
                        UserGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UserPIN = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UserFullName = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        UserRole = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UserWorkingStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UserIsDeleted = c.Boolean(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false, precision: 0),
                        LastLoginDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.UserGuid)
                .Index(t => t.UserPIN)
                .Index(t => t.UserName)
                .Index(t => t.UserRole);
            
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        CategoryGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        CategoryName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        CreationDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.CategoryGuid);
            
            CreateTable(
                "dbo.TableEntity",
                c => new
                    {
                        TableGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        TableName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        TableStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        IsDeleted = c.Boolean(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.TableGuid)
                .Index(t => t.IsDeleted);
            
            CreateTable(
                "dbo.TicketPaymentItem",
                c => new
                    {
                        PaymentGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ParentOrderNo = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ParentPaymasterGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Method = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        PaymentDate = c.DateTime(nullable: false, precision: 0),
                        PrimaryRefference = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        SecondaryRefference = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        ReceivingUsername = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.PaymentGuid);
            
            CreateTable(
                "dbo.TicketPaymentMaster",
                c => new
                    {
                        PaymentMasterGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        TicketNo = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        TotalAmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmountCharged = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TicketBalanceReturned = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentDate = c.DateTime(nullable: false, precision: 0),
                        PosUser = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.PaymentMasterGuid);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        RoleGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        RoleName = c.String(unicode: false),
                        RoleDescription = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        RoleStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        RolePermissions = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        RoleIsDeleted = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        RegistrationDate = c.DateTime(nullable: false, precision: 0),
                        LastUpdateDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.RoleGuid);
            
            CreateTable(
                "dbo.WorkPeriod",
                c => new
                    {
                        WorkPeriodGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        WorkperiodName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        WorkperiodDescription = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        WorkperiodStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Openedby = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ClosedBy = c.String(maxLength: 100, storeType: "nvarchar"),
                        OpeningDate = c.DateTime(nullable: false, precision: 0),
                        ClosingDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.WorkPeriodGuid)
                .Index(t => t.WorkperiodName);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkPeriod", new[] { "WorkperiodName" });
            DropIndex("dbo.TableEntity", new[] { "IsDeleted" });
            DropIndex("dbo.PosUser", new[] { "UserRole" });
            DropIndex("dbo.PosUser", new[] { "UserName" });
            DropIndex("dbo.PosUser", new[] { "UserPIN" });
            DropIndex("dbo.OrderMaster", new[] { "OrderNo" });
            DropIndex("dbo.ClientInfoDetails", new[] { "Email" });
            DropTable("dbo.WorkPeriod");
            DropTable("dbo.UserRole");
            DropTable("dbo.TicketPaymentMaster");
            DropTable("dbo.TicketPaymentItem");
            DropTable("dbo.TableEntity");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.PosUser");
            DropTable("dbo.OrderMaster");
            DropTable("dbo.OrderItem");
            DropTable("dbo.MenuProductItem");
            DropTable("dbo.ClientInfoDetails");
        }
    }
}
