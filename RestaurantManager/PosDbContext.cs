using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Data.Entity.ModelConfiguration.Conventions; 
using DatabaseModels.Inventory;
using DatabaseModels.Security;
using DatabaseModels.Payments;
using DatabaseModels.GeneralSettings;
using DatabaseModels.WorkPeriod;
using System.Data.Entity.Infrastructure;
using DatabaseModels.OrderTicket; 
using DatabaseModels.SystemLogs; 
using System.Diagnostics;
using DatabaseModels.Vouchers;
using DatabaseModels.CRM; 
using DatabaseModels.Accounts;
using DatabaseModels.Payroll;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManager
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class PosDbContext : DbContext
    {
        //server=localhost;port=3306;database=restpos;uid=root;password=toor
        //Server=LAPTOP-FELIX;Database=LaxcPosDb;User Id=sa;Password=1234;

       //public PosDbContext() : base("server=" + AppStaticvalues.DbServer + ";port=" + AppStaticvalues.DbPort + ";database=restpos;uid=" + AppStaticvalues.DbUser + ";password=" + AppStaticvalues.DbPassword + ";")
        public PosDbContext() : base(GlobalVariables.SharedVariables.GetproductionMysqlDbConnectionString())
        { 
            Database.SetInitializer<PosDbContext>(null);
            //Database.CreateIfNotExists();
            //Database.SetInitializer<PosDbContext>(new MyInitializer());
            //this.Database.CommandTimeout=10; 
            // Database.Initialize(false);  
        }
 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        /// <summary>
        /// override savechanges to log all dbchanges
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            int rowsaffected = 0; 
            int logscount = 0;
            try
            {
                var modifiedEntities = ChangeTracker.Entries()
              .Where(p => (p.State == EntityState.Added || p.State == EntityState.Modified || p.State == EntityState.Deleted) && p.GetType().Name != typeof(DBChangeLog).Name&& p.GetType().Name != typeof(StockFlowTransaction).Name).ToList();
                var now = DateTime.Now;
                string currentuser = GlobalVariables.SharedVariables.CurrentUser == null ? "N/A" : GlobalVariables.SharedVariables.CurrentUser.UserName;
               
                foreach (var change in modifiedEntities)
                {

                    var entityName = change.Entity.GetType().Name;
                    var primaryKey = GetPrimaryKeyValue(change);
                    if (entityName != typeof(DBChangeLog).Name)
                    {
                        if (change.State == EntityState.Added)
                        { 
                            //handle Added state separately
                            if (entityName!= typeof(UserActivityLog).Name)
                            {
                               
                                DBChangeLog log = new DBChangeLog()
                                {
                                    LogActionType = EntityState.Added.ToString(),
                                    EntityName = entityName,
                                    PrimaryKeyValue = primaryKey?.ToString(),
                                    PropertyName = entityName,
                                    OldValue = "Added",
                                    NewValue = "Added",
                                    Timestamp = now,
                                    SystemUser = currentuser
                                };
                                DBChangeLog.Add(log);
                                logscount++;
                            }
                           

                        }
                        else if (change.State == EntityState.Deleted)
                        {
                            // Handle Deleted state separately
                            DBChangeLog log = new DBChangeLog()
                            {
                                LogActionType = EntityState.Deleted.ToString(),
                                EntityName = entityName,
                                PrimaryKeyValue = primaryKey?.ToString(),
                                PropertyName = entityName,
                                OldValue = "Deleted",
                                NewValue = "Deleted",
                                Timestamp = now,
                                SystemUser = currentuser
                            };
                            DBChangeLog.Add(log);
                            logscount++;
                        }
                        else
                        {
                            foreach (var prop in change.OriginalValues.PropertyNames)
                            {
                                var originalValue = change.OriginalValues[prop]?.ToString() == "" ? "Unknown" : change.OriginalValues[prop]?.ToString();
                                var currentValue = change.CurrentValues[prop]?.ToString() == "" ? "Unknown" : change.CurrentValues[prop]?.ToString();
                                if (originalValue != currentValue)
                                {
                                    Console.WriteLine(originalValue);
                                    DBChangeLog log = new DBChangeLog()
                                    {
                                        LogActionType = EntityState.Modified.ToString(),
                                        EntityName = entityName,
                                        PrimaryKeyValue = primaryKey?.ToString(),
                                        PropertyName = prop,
                                        OldValue = originalValue,
                                        NewValue = currentValue,
                                        Timestamp = now,
                                        SystemUser = currentuser
                                    };
                                    DBChangeLog.Add(log);
                                    logscount++;
                                }
                            }
                        }
                    }
                }
                
                rowsaffected = base.SaveChanges();
                if (logscount<rowsaffected)
                {
                    return rowsaffected - logscount;
                }
                else
                {
                    return rowsaffected;
                }
            }
            catch(DbEntityValidationException ex1)
            {
                string mess1 = ex1.InnerException?.Message?.ToString();
                string mess2 = "";
                var valerror = ex1.EntityValidationErrors.ToList();
                foreach(var er1 in valerror)
                {
                    foreach (var er2 in er1.ValidationErrors)
                    {
                        string mess = er1.Entry.Entity.GetType().Name.ToString() + "--" + er2.ErrorMessage+"\n";
                        mess2 += mess;
                    }
                } 
                
                System.Windows.MessageBox.Show(mess1+"\n"+mess2, "Data Validation Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return 0;
            }
            catch (Exception ex)
            {
                string valerror = ex.InnerException?.Message?.ToString();
                string mess = ex.InnerException.Message?.ToString();
                if ((bool)ex.Message?.Contains("EntityValidationErrors"))
                {
                    mess = ex.InnerException?.InnerException?.Message?.ToString();
                }
                else if (ex.InnerException?.InnerException != null)
                {
                    mess = ex.InnerException?.InnerException?.Message?.ToString();
                }
                else
                {

                }
                System.Windows.MessageBox.Show(mess,"Data Validation Error",System.Windows.MessageBoxButton.OK,System.Windows.MessageBoxImage.Error);
                return 0;
            }

            

        }

        private object GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var entityType = entry.Entity.GetType();
            var primaryKey = entityType.GetProperties()
                .FirstOrDefault(p => p.CustomAttributes.Any(attr => attr.AttributeType == typeof(KeyAttribute)));

            return primaryKey?.GetValue(entry.Entity);
        }

        //inventory
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<MenuProductItem> MenuProductItem { get; set; }
        public DbSet<StockEntryItem> StockEntryItem { get; set; }
        public DbSet<KitchenAddItem> KitchenAddItem { get; set; }
        public DbSet<DiscountItem> DiscountItem { get; set; }
        public DbSet<StockFlowTransaction> StockFlowTransaction { get; set; }
        public DbSet<StockTakingMaster> StockTakingMaster { get; set; }
        public DbSet<AssetItem> AssetItem { get; set; }
        public DbSet<AssetItemFlow> AssetsItemFlow { get; set; }
        public DbSet<AssetGroup> AssetGroup { get; set; }
        //pos user & security
        public DbSet<PosUser> PosUser { get; set; }
        public DbSet<UserRole> UserRoles { get; set; } 
        public DbSet<DBChangeLog> DBChangeLog { get; set; }
        public DbSet<UserActivityLog> UserActivityLog { get; set; }
        //pos & orders
        public DbSet<OrderMaster> OrderMaster { get; set; } 
        public DbSet<OrderItem> OrderItem { get; set; } 
        public DbSet<OrderItemVoided> OrderItemVoided { get; set; } 
        public DbSet<OrderMasterVoided> OrderMasterVoided { get; set; } 
        public DbSet<WorkPeriod> WorkPeriod { get; set; }
        //Vouchers
        public DbSet<VoucherCard> VoucherCard { get; set; }
        public DbSet<DiscountVoucher> DiscountVoucher { get; set; } 
        //payments & accounts
        public DbSet<TicketPaymentMaster> TicketPaymentMaster { get; set; }
        public DbSet<TicketPaymentItem> TicketPaymentItem { get; set; } 
        public DbSet<InvoicesMaster> InvoicesMaster { get; set; } 
        public DbSet<InvoicableAccount> InvoicableAccount { get; set; }
        //settings
        public DbSet<ClientInfoDetails> ClientInfo { get; set; }
        public DbSet<TableEntity> TableEntity { get; set; }
        public DbSet<PosVariables> PosVariables { get; set; }
        public DbSet<MailingProfile> MailingProfile { get; set; }
        public DbSet<AssetUOM> AssetUOM { get; set; }

        //Employees
        public DbSet<CustomerAccount> CustomerAccount { get; set; }
        public DbSet<CustomerPointsAccount> CustomerPointsAccount { get; set; }
        public DbSet<PersonalAccount> PersonalAccount { get; set; } 
        public DbSet<EmployeeAccount> EmployeeAccount { get; set; }
        public object DataValidationErrors { get; private set; }
    }
    public class MyInitializer : IDatabaseInitializer<PosDbContext>
    { 
        public void InitializeDatabase(PosDbContext context)
        {
            //check for existance
            if (!context.Database.Exists())
            {
                if (System.Windows.MessageBox.Show("THE DATABASE DOES NOT EXIST. DO YOU WANT TO CREATE?", "Message Box",System.Windows.MessageBoxButton.YesNo,System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                {
                    context.Database.Create();
                }
            }
        }
    }
}
