namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientInfoDetails",
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
                "dbo.MenuProductItem",
                c => new
                    {
                        ProductGuid = c.String(nullable: false, maxLength: 100),
                        ProductName = c.String(nullable: false, maxLength: 200),
                        CategoryGuid = c.String(nullable: false, maxLength: 100),
                        AvailabilityStatus = c.String(nullable: false, maxLength: 100),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ProductGuid);
            
            CreateTable(
                "dbo.OrderItem",
                c => new
                    {
                        ItemRowGuid = c.String(nullable: false, maxLength: 100),
                        ParentProductItemGuid = c.String(nullable: false, maxLength: 100),
                        OrderID = c.String(nullable: false, maxLength: 100),
                        ItemName = c.String(nullable: false, maxLength: 200),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ServiceType = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.ItemRowGuid);
            
            CreateTable(
                "dbo.OrderMaster",
                c => new
                    {
                        OrderGuid = c.String(nullable: false, maxLength: 100),
                        OrderNo = c.String(nullable: false, maxLength: 100),
                        OrderStatus = c.String(nullable: false, maxLength: 100),
                        CustomerName = c.String(maxLength: 200),
                        TicketTable = c.String(maxLength: 100),
                        UserServing = c.String(nullable: false, maxLength: 100),
                        OrderDate = c.DateTime(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OrderGuid)
                .Index(t => t.OrderNo);
            
            CreateTable(
                "dbo.PosUser",
                c => new
                    {
                        UserGuid = c.String(nullable: false, maxLength: 100),
                        UserPIN = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 100),
                        UserFullName = c.String(nullable: false, maxLength: 500),
                        UserRole = c.String(nullable: false, maxLength: 100),
                        UserWorkingStatus = c.String(nullable: false, maxLength: 100),
                        UserIsDeleted = c.Boolean(nullable: false),
                        UserRights = c.String(nullable: false, maxLength: 500),
                        RegistrationDate = c.DateTime(nullable: false),
                        LastLoginDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserGuid)
                .Index(t => t.UserPIN)
                .Index(t => t.UserName)
                .Index(t => t.UserRole);
            
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        CategoryGuid = c.String(nullable: false, maxLength: 100),
                        CategoryName = c.String(nullable: false, maxLength: 200),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryGuid);
            
            CreateTable(
                "dbo.TableEntity",
                c => new
                    {
                        TableGuid = c.String(nullable: false, maxLength: 100),
                        TableName = c.String(nullable: false, maxLength: 100),
                        TableStatus = c.String(nullable: false, maxLength: 100),
                        IsDeleted = c.Boolean(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TableGuid)
                .Index(t => t.IsDeleted);
            
            CreateTable(
                "dbo.TicketPaymentItem",
                c => new
                    {
                        PaymentGuid = c.String(nullable: false, maxLength: 100),
                        ParentOrderNo = c.String(nullable: false, maxLength: 100),
                        ParentPaymasterGuid = c.String(nullable: false, maxLength: 100),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Method = c.String(nullable: false, maxLength: 100),
                        PaymentDate = c.DateTime(nullable: false),
                        PrimaryRefference = c.String(nullable: false, maxLength: 100),
                        SecondaryRefference = c.String(nullable: false, maxLength: 500),
                        ReceivingUsername = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.PaymentGuid);
            
            CreateTable(
                "dbo.TicketPaymentMaster",
                c => new
                    {
                        PaymentMasterGuid = c.String(nullable: false, maxLength: 100),
                        TicketNo = c.String(nullable: false, maxLength: 100),
                        TotalAmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmountCharged = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TicketBalanceReturned = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentDate = c.DateTime(nullable: false),
                        PosUser = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.PaymentMasterGuid);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        RoleGuid = c.String(nullable: false, maxLength: 100),
                        RoleName = c.String(nullable: false, maxLength: 100),
                        RoleDescription = c.String(nullable: false, maxLength: 500),
                        RolePermissions = c.String(nullable: false, maxLength: 100),
                        RoleIsDeleted = c.String(nullable: false, maxLength: 100),
                        RegistrationDate = c.DateTime(nullable: false),
                        LastUpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RoleGuid)
                .Index(t => t.RoleName);
            
            CreateTable(
                "dbo.WorkPeriod",
                c => new
                    {
                        WorkPeriodGuid = c.String(nullable: false, maxLength: 100),
                        WorkperiodName = c.String(nullable: false, maxLength: 100),
                        WorkperiodDescription = c.String(nullable: false, maxLength: 500),
                        WorkperiodStatus = c.String(nullable: false, maxLength: 100),
                        Openedby = c.String(nullable: false, maxLength: 100),
                        ClosedBy = c.String(maxLength: 100),
                        OpeningDate = c.DateTime(nullable: false),
                        ClosingDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.WorkPeriodGuid)
                .Index(t => t.WorkperiodName);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkPeriod", new[] { "WorkperiodName" });
            DropIndex("dbo.UserRole", new[] { "RoleName" });
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
