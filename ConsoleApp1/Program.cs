 using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    class SellRightContext : DbContext
    {

        //Add your Dbsets here  
        public DbSet<MyEntity> Products
        {
            get;
            set;
        }
        public SellRightContext() : base("server=127.0.0.1;user=root;password=toor;port=3306;database=restpos") { }
    }
    public class MyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gggg { get; set; }


    }
}
