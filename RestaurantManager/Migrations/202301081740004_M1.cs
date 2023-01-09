namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M1 : DbMigration
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
                        ReceiptNote3 = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        TaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AcceptedCards = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.ClientGuid)
                .Index(t => t.Email);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        CustomerName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        PhoneNumber = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        CustomerEmail = c.String(maxLength: 200, storeType: "nvarchar"),
                        Gender = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        RegistrationDate = c.DateTime(nullable: false, precision: 0),
                        BirthDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.CustomerGuid);
            
            CreateTable(
                "dbo.CustomerAccount",
                c => new
                    {
                        AccActionGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        TransactionNo = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        CustomerPhoneNo = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Debit = c.Int(nullable: false),
                        Credit = c.Int(nullable: false),
                        ActionDate = c.DateTime(nullable: false, precision: 0),
                        ApprovedBy = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        TransactionType = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.AccActionGuid);
            
            CreateTable(
                "dbo.DiscountItem",
                c => new
                    {
                        DiscRowID = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ProductGuid = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        ProductName = c.String(nullable: false, unicode: false),
                        DiscType = c.String(maxLength: 100, storeType: "nvarchar"),
                        AttachedProduct = c.String(nullable: false, unicode: false),
                        DiscPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false, precision: 0),
                        EndDate = c.DateTime(nullable: false, precision: 0),
                        OfferDay = c.String(nullable: false, unicode: false),
                        IsRepetitive = c.Boolean(nullable: false),
                        DiscStatus = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.DiscRowID);
            
            CreateTable(
                "dbo.DiscountVoucher",
                c => new
                    {
                        BatchGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        BatchNumber = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        VoucherType = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        VoucherAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BulkSalesLimitAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        CreationDate = c.DateTime(nullable: false, precision: 0),
                        StartDate = c.DateTime(nullable: false, precision: 0),
                        EndDate = c.DateTime(nullable: false, precision: 0),
                        BatchDescription = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        IsActiveStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BatchGuid);
            
            CreateTable(
                "dbo.KitchenAddItem",
                c => new
                    {
                        ItemGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ProductGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ProductName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        WorkPeriod = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Quantity = c.Int(nullable: false),
                        InsertionDate = c.DateTime(nullable: false, precision: 0),
                        InsertionBy = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.ItemGuid);
            
            CreateTable(
                "dbo.MailingProfile",
                c => new
                    {
                        RowGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ProfileName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        DisplayName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        DestinationAddress = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        SenderAddress = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        MailingAddress = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        AppPassword = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.RowGuid);
            
            CreateTable(
                "dbo.MenuProductItem",
                c => new
                    {
                        ProductGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ProductName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        CategoryGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        AvailabilityStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        HouseType = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ProductPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PackagingCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RemainingQuantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductGuid);
            
            CreateTable(
                "dbo.OrderItem",
                c => new
                    {
                        ItemRowGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ProductItemGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OrderID = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ItemName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ServiceType = c.String(nullable: false, maxLength: 20, storeType: "nvarchar"),
                        IsGiftItem = c.Boolean(nullable: false),
                        GiftItemGuid = c.String(nullable: false, unicode: false),
                        ParentItemGuid = c.String(nullable: false, unicode: false),
                        DiscPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsItemVoided = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ItemRowGuid);
            
            CreateTable(
                "dbo.OrderMaster",
                c => new
                    {
                        OrderGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OrderNo = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OrderStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        CustomerRefference = c.String(maxLength: 200, storeType: "nvarchar"),
                        TicketTable = c.String(maxLength: 100, storeType: "nvarchar"),
                        UserServing = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        VoidReason = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        IsKitchenServed = c.Boolean(nullable: false),
                        IsInPreparation = c.Boolean(nullable: false),
                        MergedChild = c.String(maxLength: 100, storeType: "nvarchar"),
                        Workperiod = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OrderDate = c.DateTime(nullable: false, precision: 0),
                        PaymentDate = c.DateTime(nullable: false, precision: 0),
                        IsPrinted = c.Boolean(nullable: false),
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
                        UserFullName = c.String(nullable: false, unicode: false),
                        UserRole = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UserWorkingStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UserRights = c.String(nullable: false, unicode: false),
                        UserIsDeleted = c.Boolean(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false, precision: 0),
                        LastLoginDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.UserGuid)
                .Index(t => t.UserPIN)
                .Index(t => t.UserName)
                .Index(t => t.UserRole);
            
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
                "dbo.StockEntryItem",
                c => new
                    {
                        ItemGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ItemDescription = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        WorkPeriod = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UOM = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Quantity = c.Int(nullable: false),
                        InsertionDate = c.DateTime(nullable: false, precision: 0),
                        InsertionBy = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.ItemGuid);
            
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
                "dbo.TestModel",
                c => new
                    {
                        testkey = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.testkey);
            
            CreateTable(
                "dbo.TicketPaymentItem",
                c => new
                    {
                        PaymentGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ParentOrderNo = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ParentTransNo = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
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
                        TransNo = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        TicketNo = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        TotalAmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmountCharged = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TicketBalanceReturned = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentDate = c.DateTime(nullable: false, precision: 0),
                        PosUser = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        WorkPeriod = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
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
                "dbo.VoucherCard",
                c => new
                    {
                        VoucherGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        VoucherNumber = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        VoucherBatchNo = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        VoucherStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        VoucherType = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        VoucherAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreationDate = c.DateTime(nullable: false, precision: 0),
                        ExpiryDate = c.DateTime(nullable: false, precision: 0),
                        RedemptionDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.VoucherGuid);
            
            CreateTable(
                "dbo.WorkPeriod",
                c => new
                    {
                        WorkPeriodGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        WorkperiodName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        WorkperiodStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OpeningNote = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        Openedby = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OpeningDate = c.DateTime(nullable: false, precision: 0),
                        ClosedBy = c.String(maxLength: 100, storeType: "nvarchar"),
                        ClosingDate = c.DateTime(nullable: false, precision: 0),
                        ClosingNote = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
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
            DropTable("dbo.VoucherCard");
            DropTable("dbo.UserRole");
            DropTable("dbo.TicketPaymentMaster");
            DropTable("dbo.TicketPaymentItem");
            DropTable("dbo.TestModel");
            DropTable("dbo.TableEntity");
            DropTable("dbo.StockEntryItem");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.PosVariables");
            DropTable("dbo.PosUser");
            DropTable("dbo.OrderMaster");
            DropTable("dbo.OrderItem");
            DropTable("dbo.MenuProductItem");
            DropTable("dbo.MailingProfile");
            DropTable("dbo.KitchenAddItem");
            DropTable("dbo.DiscountVoucher");
            DropTable("dbo.DiscountItem");
            DropTable("dbo.CustomerAccount");
            DropTable("dbo.Customer");
            DropTable("dbo.ClientInfoDetails");
            DropTable("dbo.ChangeLog");
        }
    }
}
