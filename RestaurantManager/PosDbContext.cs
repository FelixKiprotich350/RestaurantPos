using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.BusinessModels.PointofSale;
using System.Data.Entity.ModelConfiguration.Conventions;
using RestaurantManager.BusinessModels;
using RestaurantManager.BusinessModels.Menu;
using RestaurantManager.BusinessModels.Security;
using RestaurantManager.BusinessModels.Payments;
using RestaurantManager.BusinessModels.GeneralSettings;

namespace RestaurantManager
{
    //[DbConfigurationType(typeof(MySqlEFConfiguration))] 
    class PosDbContext : DbContext
    {
        public PosDbContext() : base("Server=LAPTOP-FELIX;Database=myDataBase;User Id=sa;Password=1234;")
        { 
            //this.Database.CommandTimeout=10; 
            this.Database.CreateIfNotExists(); 
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
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
        //payments
        public DbSet<TicketPaymentMaster> TicketPaymentMaster { get; set; }
        public DbSet<TicketPaymentItem> TicketPaymentItem { get; set; }
        //settings
        public DbSet<ClientInfoDetails> ClientInfo { get; set; }
        public DbSet<TableEntity> TableEntity { get; set; }
    }
     
}
