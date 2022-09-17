using RestaurantManager.BusinessModels.GeneralSettings;
using RestaurantManager.BusinessModels.Security;
using RestaurantManager.BusinessModels.WorkPeriod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.GlobalVariables
{
    public static class SharedVariables
    {
     
        public static MainWindow Main_Window = null;
        public static string DbConnectionstring = "server=127.0.0.1;user=root;password=toor;port=3306;database=restpos";

        public static ClientInfoDetails ClientInfo = null;
        public static PosUser CurrentUser = null;
        public static string AdminRoleName = "Admin";


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
    }
}
