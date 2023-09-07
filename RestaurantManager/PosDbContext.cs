using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Data.Entity.ModelConfiguration.Conventions; 
using DatabaseModels.Warehouse;
using DatabaseModels.Security;
using DatabaseModels.Payments;
using DatabaseModels.GeneralSettings;
using DatabaseModels.WorkPeriod;
using System.Data.Entity.Infrastructure;
using DatabaseModels.OrderTicket;
using System.Diagnostics;
using DatabaseModels.Vouchers;
using DatabaseModels.CustomersManagement;
using RestaurantManager.ActivityLogs; 

namespace RestaurantManager
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class PosDbContext : DbContext
    {
        //server=localhost;port=3306;database=restpos;uid=root;password=toor
        //Server=LAPTOP-FELIX;Database=LaxcPosDb;User Id=sa;Password=1234;

       //public PosDbContext() : base("server=" + AppStaticvalues.DbServer + ";port=" + AppStaticvalues.DbPort + ";database=restpos;uid=" + AppStaticvalues.DbUser + ";password=" + AppStaticvalues.DbPassword + ";")
        public PosDbContext() : base(GlobalVariables.SharedVariables.GetdevMysqlDbConnectionString())
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
        //public override int SaveChanges()
        //{
        //    var modifiedEntities = ChangeTracker.Entries()
        //        .Where(p => p.State == EntityState.Modified).ToList();
        //    var now = DateTime.UtcNow;

        //    foreach (var change in modifiedEntities)
        //    {
        //        var entityName = change.Entity.GetType().Name;
        //        var primaryKey = GetPrimaryKeyValue(change);

        //        foreach (var prop in change.OriginalValues.PropertyNames)
        //        {
        //            var originalValue = change.OriginalValues[prop].ToString();
        //            var currentValue = change.CurrentValues[prop].ToString();
        //            if (originalValue != currentValue)
        //            {
        //                ChangeLog log = new ChangeLog()
        //                {
        //                    EntityName = entityName,
        //                    PrimaryKeyValue = primaryKey.ToString(),
        //                    PropertyName = prop,
        //                    OldValue = originalValue,
        //                    NewValue = currentValue,
        //                    DateChanged = now
        //                };
        //                ChangeLogs.Add(log);
        //            }
        //        }
        //    }
        //    return base.SaveChanges();
        //}
 
        //Warehouse
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<MenuProductItem> MenuProductItem { get; set; }
        public DbSet<StockEntryItem> StockEntryItem { get; set; }
        public DbSet<KitchenAddItem> KitchenAddItem { get; set; }
        public DbSet<DiscountItem> DiscountItem { get; set; }
        //pos user & security
        public DbSet<PosUser> PosUser { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        //pos & orders
        public DbSet<OrderMaster> OrderMaster { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; } 
        public DbSet<WorkPeriod> WorkPeriod { get; set; }
        //Vouchers
        public DbSet<VoucherCard> VoucherCard { get; set; }
        public DbSet<DiscountVoucher> DiscountVoucher { get; set; } 
        //payments
        public DbSet<TicketPaymentMaster> TicketPaymentMaster { get; set; }
        public DbSet<TicketPaymentItem> TicketPaymentItem { get; set; }
        //settings
        public DbSet<ClientInfoDetails> ClientInfo { get; set; }
        public DbSet<TableEntity> TableEntity { get; set; }
        public DbSet<ChangeLog> ChangeLogs { get; set; }
        public DbSet<PosVariables> PosVariables { get; set; }
        public DbSet<MailingProfile> MailingProfile { get; set; }
        //customers
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerPointsAccount> CustomerPointsAccount { get; set; }
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
