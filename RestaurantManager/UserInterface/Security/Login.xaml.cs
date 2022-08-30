using MySql.Data.MySqlClient;
using RestaurantManager.BusinessModels.Security;
using System;
using System.Collections.Generic;
using System.Linq;
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
using RestaurantManager.BusinessModels.Navigation; 

namespace RestaurantManager.UserInterface.Security
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        readonly PermissionMaster Pm = new PermissionMaster();
        public Login()
        {
            InitializeComponent();
            ErpShared.CurrentUser = null;
        }

        public void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow m = new MainWindow();
                if (PasswordBox_UserPin.Password != "" )
                {
                    PosUser user = null;
                    using (var db = new PosDbContext())
                    {
                        if (db.PosUser.Where(a => a.UserPIN.ToString() == PasswordBox_UserPin.Password.Trim()).Count() > 0)
                        {
                            user = db.PosUser.Where(a => a.UserPIN.ToString() == PasswordBox_UserPin.Password.Trim()).First();
                        }
                        else
                        {
                            return;
                        }
                    }
                    List<string> raw = new List<string>();
                    //raw permissions
                    raw = user.UserRights.Split(',').Where(a => a.Trim() != "").ToList();
                    //final permissions
                    if (raw.Count > 0)
                    {
                        user.User_Permissions_final = new List<PermissionMaster>();
                        foreach (var a in raw)
                        {
                            if (Pm.GetAllPermissions().Where(b => b.PermissionGuid == a).Count() > 0)
                            {
                                user.User_Permissions_final.Add(Pm.GetAllPermissions().Where(b => b.PermissionGuid == a).First());
                            }
                        }
                    }
                    ErpShared.CurrentUser = user;
                    //this.DialogResult = true;
                    m.Show();
                    this.Close();
                    
                }
                else
                {
                    MessageBox.Show(this, "Enter Username and Password!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                      InitializeDb();
                    PasswordBox_UserPin.Focus();
                }
                catch (System.Data.SqlClient.SqlException ex1)
                {
                    if (ex1.Message.ToLower().Contains("a network-related or instance-specific"))// error occured
                    {
                        if (MessageBox.Show(ex1.Message + "\n\nDo you want to configure the server now?", "Server Message Box", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            InitialServerConfiguration isc = new InitialServerConfiguration();
                            isc.ShowDialog();
                            MessageBox.Show("The application will shut down.\n\nKindly restart the application for the changes to apply.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                        App.Current.Shutdown();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                } 

            }
            catch (Exception ex)
            {
                string help = "\nKindly contact Developer for HELP!";
                MessageBox.Show(ex.Message + help, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeDb()
        {
            using (var a = new PosDbContext())
            {
                a.Database.Initialize(true);
                // a.ProductCategory.ToList();
            }
        }

    }
}
