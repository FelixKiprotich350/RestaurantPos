using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager
{
    public static class AppStaticvalues
    {
        public static string DbServer { get; set; }
        public static string DbUser { get; set; }
        public static string DbPort { get; set; }
        public static string DbPassword { get; set; }
        public static string GetDbConnectionString()
        {
            AppStaticvalues.DbServer = new ApplicationFiles.Base64().Base64Decode(Properties.Settings.Default.String1);
            AppStaticvalues.DbUser = new ApplicationFiles.Base64().Base64Decode(Properties.Settings.Default.String2);
            AppStaticvalues.DbPassword = new ApplicationFiles.Base64().Base64Decode(Properties.Settings.Default.String3);
            AppStaticvalues.DbPort = new ApplicationFiles.Base64().Base64Decode(Properties.Settings.Default.String4);
            return "server=" + DbServer + ";port=" + DbPort + ";database=restpos;uid=" + DbUser + ";password=" + DbPassword + ";";
        }
    }
}
