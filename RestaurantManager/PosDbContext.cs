﻿using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Data.Entity.ModelConfiguration.Conventions;
using RestaurantManager.BusinessModels;
using RestaurantManager.BusinessModels.Menu;
using RestaurantManager.BusinessModels.Security;
using RestaurantManager.BusinessModels.Payments;
using RestaurantManager.BusinessModels.GeneralSettings;
using RestaurantManager.BusinessModels.WorkPeriod;
using System.Data.Entity.Infrastructure;
using RestaurantManager.BusinessModels.OrderTicket;
using System.Diagnostics;
using RestaurantManager.BusinessModels.Vouchers;
using RestaurantManager.BusinessModels.CustomersManagement;

namespace RestaurantManager
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    class PosDbContext : DbContext
    {
        //server=localhost;port=3306;database=restpos;uid=root;password=toor
        //Server=LAPTOP-FELIX;Database=LaxcPosDb;User Id=sa;Password=1234;
        public PosDbContext() : base("server=localhost;port=3306;database=restpos;uid=root;password=toor;")
        {
            //this.Database.CommandTimeout=10; 
            // this.Database.CreateIfNotExists(); 
            Database.Log = s => Debug.WriteLine(s);
            //Database.Log = s => Trace.WriteLine(s);
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
        private object GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }
        //menu products
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<MenuProductItem> MenuProductItem { get; set; }
        //pos user & security
        public DbSet<PosUser> PosUser { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        //pos & orders
        public DbSet<OrderMaster> OrderMaster { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<OrderItemVoided> OrderItemVoided { get; set; }
        public DbSet<WorkPeriod> WorkPeriod { get; set; }
        //Vouchers
        public DbSet<VoucherCard> VoucherCard { get; set; }
        //payments
        public DbSet<TicketPaymentMaster> TicketPaymentMaster { get; set; }
        public DbSet<TicketPaymentItem> TicketPaymentItem { get; set; }
        //settings
        public DbSet<ClientInfoDetails> ClientInfo { get; set; }
        public DbSet<TableEntity> TableEntity { get; set; }
        public DbSet<ChangeLog> ChangeLogs { get; set; }
        //customers
        public DbSet<Customer> Customer { get; set; }
    }
}
