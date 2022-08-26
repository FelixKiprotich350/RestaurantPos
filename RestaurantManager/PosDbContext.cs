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

namespace RestaurantManager
{
    //[DbConfigurationType(typeof(MySqlEFConfiguration))] 
    class PosDbContext : DbContext
    {
        public PosDbContext() : base("Server=LAPTOP-FELIX;Database=myDataBase;User Id=sa;Password=1234;")
        {
            this.Database.CreateIfNotExists();
            //this.Database.Initialize(false);

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<MenuProductItem> MenuProductItem { get; set; }
        public DbSet<PosUser> PosUser { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }
        public DbSet<OrderMaster> OrderMaster { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
    }
     
}
