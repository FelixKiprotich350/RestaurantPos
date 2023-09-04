using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager
{
    public static class AppStaticvalues
    {
        public static string GetMysqlDbConnectionString()
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
                return "server=127.0.0.1;user=felix;password=felix;port=3306;=database=podbv1";


            }
            catch
            {
                return "";
            }
        }

        public static string GetSqlServerDbConnectionString()
        {
            return "Server=DESKTOP-23HBUBN\\SQLEXPRESS;Database=posdbv1;User Id=sa;Password=felix";
        }
    }
}
