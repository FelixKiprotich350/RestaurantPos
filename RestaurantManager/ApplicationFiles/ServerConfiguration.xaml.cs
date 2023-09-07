using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RestaurantManager.ApplicationFiles
{
    /// <summary>
    /// Interaction logic for ServerConfiguration.xaml
    /// </summary>
    public partial class ServerConfiguration : Window
    {
        public ServerConfiguration()
        {
            if (!IsAdministrator())
            {
                MessageBox.Show("Restart the application with Administrator Privileges!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                App.Current.Shutdown();
            }
            InitializeComponent();
        }
        private void Button_Test_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connection = "";
                connection += "Server=" + Textbox_ServerIP.Text.Trim() + ";";
                connection += "user=" + Textbox_ServerUserID.Text.Trim() + ";";
                connection += "password=" + Textbox_ServerPassword.Text.Trim() + ";";
                connection += "port=" + Textbox_ServerPort.Text.Trim() + ";";
                MySqlConnection con = new MySqlConnection(connection);
                con.Open();
                con.Close();
                con.Dispose();
                MessageBox.Show("DATABASE SERVER CONNECTED!", "DATABASE SERVER CONNECTION", MessageBoxButton.OK, MessageBoxImage.Information);
                string fileName = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                fileName += "\\config.txt";
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                connection += "Database=" + Textbox_ServerDatabase.Text.Trim() + ";";
                // Create a new file     
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file    
                    Byte[] title = new UTF8Encoding(true).GetBytes(connection);
                    fs.Write(title, 0, title.Length);
                }
                MessageBox.Show("SERVER CONNECTION SAVED!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);

                using (var db = new PosDbContext())
                {

                    if (!db.Database.Exists())
                    {
                        //db.Database.Delete();
                        db.Database.Create();
                    }
                    db.Database.Connection.Open();
                    db.Database.Connection.Close();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DATABASE CONNECTION FAILED", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally
            {
                App.Current.Shutdown();
            }
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        public static bool IsAdministrator()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
