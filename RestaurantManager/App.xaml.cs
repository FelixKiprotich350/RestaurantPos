using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows; 

namespace RestaurantManager
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
     
        private void Application_Startup(object sender, StartupEventArgs e)
        {
                App.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }
    }
}
