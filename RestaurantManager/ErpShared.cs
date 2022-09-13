using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.BusinessModels.Security; 
using System.Windows;
using RestaurantManager.BusinessModels.GeneralSettings;
using RestaurantManager.BusinessModels.WorkPeriod;

namespace RestaurantManager
{
    public static class ErpShared
    {
        public static DateTime CurrentDate()
        {
            return DateTime.Now;
        }
        public static MainWindow Main_Window = null;
        public static ClientInfoDetails ClientInfo = null;
        public static PosUser CurrentUser = null;
        public static string DbConnectionstring = "server=127.0.0.1;user=root;password=toor;port=3306;database=restpos";
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
    }
}
