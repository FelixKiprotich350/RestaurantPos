namespace RestaurantManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetGroup",
                c => new
                    {
                        GroupGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        GroupName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        CreationDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.GroupGuid)
                .Index(t => t.GroupName, unique: true);
            
            CreateTable(
                "dbo.AssetItem",
                c => new
                    {
                        AssetItemGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        AssetName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        AssetDescription = c.String(nullable: false, unicode: false),
                        AssetGroupGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UOM = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        typeofasset = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        AssetItemCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InStockQuantity = c.Int(nullable: false),
                        IsPrecount = c.Boolean(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false, precision: 0),
                        LastUpdateDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.AssetItemGuid);
            
            CreateTable(
                "dbo.AssetItemFlow",
                c => new
                    {
                        AssetFlowGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        AssetItemGuid = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        AssetName = c.String(nullable: false, unicode: false),
                        FlowDirection = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OutTransactionCode = c.String(nullable: false, unicode: false),
                        InTransactionCode = c.String(nullable: false, unicode: false),
                        TransactionDate = c.DateTime(nullable: false, precision: 0),
                        IsCancelled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AssetFlowGuid);
            
            CreateTable(
                "dbo.AssetUOM",
                c => new
                    {
                        UnitGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UnitName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UnitShortcode = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        RegistrationDate = c.DateTime(nullable: false, precision: 0),
                        LastUpdateDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.UnitGuid)
                .Index(t => t.UnitName, unique: true);
            
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
                "dbo.CustomerAccount",
                c => new
                    {
                        AccountGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        PersonalAccNo = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        CustAccountNo = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        TotalPoints = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegDate = c.DateTime(nullable: false, precision: 0),
                        LastUpdateDate = c.DateTime(nullable: false, precision: 0),
                        UpdatedBy = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        AccountStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.AccountGuid)
                .Index(t => t.CustAccountNo, unique: true);
            
            CreateTable(
                "dbo.CustomerPointsAccount",
                c => new
                    {
                        AccountGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        TransactionNo = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        CustomerAccNo = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Debit = c.Int(nullable: false),
                        Credit = c.Int(nullable: false),
                        ActionDate = c.DateTime(nullable: false, precision: 0),
                        ApprovedBy = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        TransactionType = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.AccountGuid);
            
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
                "dbo.InvoicesMaster",
                c => new
                    {
                        InvoiceGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        InvoiceNo = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        InvoiceAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvoiceStatus = c.String(nullable: false, unicode: false),
                        InvoicePaymentStatus = c.String(nullable: false, unicode: false),
                        InvoiceDate = c.DateTime(nullable: false, precision: 0),
                        LastPaymentDate = c.DateTime(nullable: false, precision: 0),
                        ExpectedPayDate = c.DateTime(nullable: false, precision: 0),
                        PrimaryRefference = c.String(nullable: false, unicode: false),
                        InvoiceSource = c.String(nullable: false, unicode: false),
                        CancelReason = c.String(nullable: false, unicode: false),
                        Notes = c.String(nullable: false, unicode: false),
                        ReceivedBy = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.InvoiceGuid)
                .Index(t => t.InvoiceNo, unique: true);
            
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
                        Department = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        BuyingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SellingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PackagingCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RemainingQuantity = c.Int(nullable: false),
                        IsPrecount = c.Boolean(nullable: false),
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
                        BuyingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BuyingPriceTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                        MergedChild = c.String(maxLength: 100, storeType: "nvarchar"),
                        Workperiod = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        OrderDate = c.DateTime(nullable: false, precision: 0),
                        PaymentDate = c.DateTime(nullable: false, precision: 0),
                        IsPrinted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderGuid)
                .Index(t => t.OrderNo);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        PersonGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        FullName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        PhoneNumber = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        AccountNo = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        InvoiceLimit = c.Int(nullable: false),
                        Email = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Gender = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        RegistrationDate = c.DateTime(nullable: false, precision: 0),
                        UpdateDate = c.DateTime(nullable: false, precision: 0),
                        BirthDate = c.DateTime(nullable: false, precision: 0),
                        AccountStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.PersonGuid)
                .Index(t => t.AccountNo, unique: true);
            
            CreateTable(
                "dbo.PosUser",
                c => new
                    {
                        UserGuid = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UserPIN = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UserFullName = c.String(nullable: false, unicode: false),
                        IsDefaultpin = c.Boolean(nullable: false),
                        IsPosUser = c.Boolean(nullable: false),
                        IsBackendUser = c.Boolean(nullable: false),
                        PhoneNumber = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UserRole = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UserWorkingStatus = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        UserRights = c.String(nullable: false, unicode: false),
                        UserIsDeleted = c.Boolean(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false, precision: 0),
                        LastLoginDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.UserGuid)
                .Index(t => t.UserPIN)
                .Index(t => t.UserName);
            
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
                        Department = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        CreationDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.CategoryGuid)
                .Index(t => t.CategoryName, unique: true);
            
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
                "dbo.StockFlowTransaction",
                c => new
                    {
                        TransactionID = c.Int(nullable: false, identity: true),
                        ProductGuid = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        ProductName = c.String(nullable: false, unicode: false),
                        FlowDirection = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OutTransactionCode = c.String(nullable: false, unicode: false),
                        InTransactionCode = c.String(nullable: false, unicode: false),
                        Description = c.String(nullable: false, unicode: false),
                        TransactionDate = c.DateTime(nullable: false, precision: 0),
                        IsCancelled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionID);
            
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
                .Index(t => t.TableName, unique: true)
                .Index(t => t.IsDeleted);
            
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
                        IsVoided = c.Boolean(nullable: false),
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
            DropIndex("dbo.TableEntity", new[] { "TableName" });
            DropIndex("dbo.ProductCategory", new[] { "CategoryName" });
            DropIndex("dbo.PosUser", new[] { "UserName" });
            DropIndex("dbo.PosUser", new[] { "UserPIN" });
            DropIndex("dbo.Person", new[] { "AccountNo" });
            DropIndex("dbo.OrderMaster", new[] { "OrderNo" });
            DropIndex("dbo.InvoicesMaster", new[] { "InvoiceNo" });
            DropIndex("dbo.CustomerAccount", new[] { "CustAccountNo" });
            DropIndex("dbo.ClientInfoDetails", new[] { "Email" });
            DropIndex("dbo.AssetUOM", new[] { "UnitName" });
            DropIndex("dbo.AssetGroup", new[] { "GroupName" });
            DropTable("dbo.WorkPeriod");
            DropTable("dbo.VoucherCard");
            DropTable("dbo.UserRole");
            DropTable("dbo.TicketPaymentMaster");
            DropTable("dbo.TicketPaymentItem");
            DropTable("dbo.TableEntity");
            DropTable("dbo.StockFlowTransaction");
            DropTable("dbo.StockEntryItem");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.PosVariables");
            DropTable("dbo.PosUser");
            DropTable("dbo.Person");
            DropTable("dbo.OrderMaster");
            DropTable("dbo.OrderItem");
            DropTable("dbo.MenuProductItem");
            DropTable("dbo.MailingProfile");
            DropTable("dbo.KitchenAddItem");
            DropTable("dbo.InvoicesMaster");
            DropTable("dbo.DiscountVoucher");
            DropTable("dbo.DiscountItem");
            DropTable("dbo.CustomerPointsAccount");
            DropTable("dbo.CustomerAccount");
            DropTable("dbo.ClientInfoDetails");
            DropTable("dbo.ChangeLog");
            DropTable("dbo.AssetUOM");
            DropTable("dbo.AssetItemFlow");
            DropTable("dbo.AssetItem");
            DropTable("dbo.AssetGroup");
        }
    }
}
