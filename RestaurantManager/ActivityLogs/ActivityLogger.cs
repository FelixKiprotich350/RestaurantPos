using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseModels.SystemLogs;
using RestaurantManager.GlobalVariables;

namespace RestaurantManager.ActivityLogs
{
    public static class ActivityLogger
    {
        public static string CurrentParameters = "";
        public static void LogDBAction(string type,string Description, string parameters)
        {
            try
            {
                var log = new UserActivityLog();
                log.Logtype = type;
                log.SystemUser = SharedVariables.CurrentUser != null ? SharedVariables.CurrentUser.UserName : "N/A";
                log.Timmestamp = SharedVariables.CurrentDate(); 
                log.LogLevel = "1";
                log.Description =  Description;
                log.Parameters =parameters;
                using (var db = new PosDbContext())
                { 
                    db.UserActivityLog.Add(log);
                    db.SaveChanges();
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Message Box",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
        public static void LogFileAction(string type,string Description, string parameters)
        {
            try
            {
                var log = new UserActivityLog();
                log.Logtype = type;
                log.SystemUser = SharedVariables.CurrentUser != null ? SharedVariables.CurrentUser.UserName : "N/A";
                log.Timmestamp = SharedVariables.CurrentDate(); 
                log.LogLevel = "1";
                log.Description =  Description;
                log.Parameters =parameters;
                using (var db = new PosDbContext())
                { 
                    db.UserActivityLog.Add(log);
                    //db.SaveChanges();
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Message Box",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }
}
