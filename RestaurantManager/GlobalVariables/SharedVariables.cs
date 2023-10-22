using RestaurantManager.ApplicationFiles; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.UserInterface;
using DatabaseModels.Security;
using DatabaseModels.WorkPeriod;
using DatabaseModels.GeneralSettings;
using System.IO;
using System.Reflection;

namespace RestaurantManager.GlobalVariables
{
    public static class SharedVariables
    {
     
        public static POSMainContainer POS_MainWindow = null;
        public static MainWindow Backend_MainWindow = null;
        public static string DbConnectionstring = "server=127.0.0.1;user=root;password=toor;port=3306;database=restpos";
         
        public static PosUser CurrentUser = null;
      //  public static string AdminRoleName = "Admin";
        public static string LogInCounter = Environment.MachineName; 
        public static string CurrentUserMode = ""; 

        /// <summary>
        /// Global Methods
        /// </summary>
        /// <returns></returns>
        public static DateTime CurrentDate()
        {
            return DateTime.Now;
        }

        public static WorkPeriod CurrentOpenWorkPeriod()
        {
            WorkPeriod w = null;
            try
            {
                using (var db = new PosDbContext())
                {
                    w = db.WorkPeriod.Where(x => x.WorkperiodStatus == "Open").First();
                }
                return w;
            }
            catch
            {
                return w;
            }
        } 
        public static int GenerateInvoiceNumber()
        {
            int w = 0;
            try
            {
                var db = new PosDbContext();
                w =  db.InvoicesMaster.Count() + 1;
                if (db.InvoicesMaster.AsNoTracking().FirstOrDefault(k=>k.InvoiceNo=="INV/S/"+w)!=null)
                {
                    w =db.InvoicesMaster.Count() + 2;
                }
                return w;
            }
            catch
            {
                return -1;
            }
        }
        public static int GeneratePersonAccNo()
        {
            int w = 0;
            try
            {
                var db = new PosDbContext();
                w = db.PersonalAccount.Count() + 1;
                if (db.PersonalAccount.AsNoTracking().FirstOrDefault(k => k.AccountNo == "ACC-" + w) != null)
                {
                    w = db.PersonalAccount.Count() + 2;
                }
                return w;
            }
            catch
            {
                return -1;
            }
        }
        public static ClientInfoDetails ClientInfo()
        {
            ClientInfoDetails w = null;
            try
            {
                using (var db = new PosDbContext())
                {
                    w = db.ClientInfo.AsNoTracking().FirstOrDefault();
                }
                return w;
            }
            catch
            {
                return w;
            }
        }


        /// <summary>
        /// database connectiion strings
        /// </summary>
        /// <returns></returns>
        public static string GetproductionMysqlDbConnectionString()
        {
            try
            {
                string fileName = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                fileName += "\\config.txt";
                if (File.Exists(fileName))
                {
                    List<string> lines = new List<string>();
                    using (StreamReader sr = File.OpenText(fileName))
                    {
                        string s = null;
                        while ((s = sr.ReadLine()) != null)
                        {
                            lines.Add(s);
                        }
                    }
                    return lines[0];
                }
                else
                {
                    return "";
                }


            }
            catch
            {
                return "";
            }
        }
        public static string GetdevMysqlDbConnectionString()
        {
            try
            {
                return "server=127.0.0.1;user=felix;password=felix;port=3306;database=posdbv3";


            }
            catch
            {
                return "";
            }
        }

        public static string GetSqlServerDbConnectionString()
        {
            return "Server=DESKTOP-23HBUBN\\SQLEXPRESS;Database=posdbv3;User Id=sa;Password=felix";
        }
    }
}
