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
using System.Text.RegularExpressions;

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

        public void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
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
                            MessageBox.Show(this, "The User PIN is Wrong!!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            PasswordBox_UserPin.Password = "";
                            return;
                        }
                        if (user.UserWorkingStatus.ToLower() != "active")
                        {
                            MessageBox.Show(this, "The User Status is not Active.\nAsk the administrator to enable your activenes!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            PasswordBox_UserPin.Password = "";
                            user = null;
                            return;
                        }
                        if (user.UserIsDeleted)
                        {
                            MessageBox.Show(this, "The UserPIN has been deleted. Enter Different PIN!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            PasswordBox_UserPin.Password = "";
                            user = null;
                            return;
                        }
                    }
                    MainWindow m = new MainWindow();
                    List<string> raw = new List<string>();
                    //raw permissions
                    raw = user.UserRights.Split(',').Where(a => a.Trim() != "").ToList();
                    //final permissions
                    if (user.UserRole == "Admin")
                    {
                        user.User_Permissions_final = new List<PermissionMaster>();
                        user.User_Permissions_final.AddRange(Pm.GetAllPermissions());
                    }
                    else if (raw.Count > 0)
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
                    using (var db = new PosDbContext())
                    {
                        if (db.ClientInfo.Count() > 0)
                        {
                            ErpShared.ClientInfo = db.ClientInfo.First();
                        }
                    }
                    ErpShared.CurrentUser = user;
                    m.Show();
                    Close();
                    
                }
                else
                {
                    MessageBox.Show(this, "Enter User PIN!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void InitializeDb()
        {
            using (var a = new PosDbContext())
            {
                a.Database.Initialize(true);
                // a.ProductCategory.ToList();
            }
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
       
        #region Number Pad

        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 1;
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 3;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 4;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 5;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 6;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_7_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 7;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_8_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 8;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_9_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 9;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_0_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        private void PasswordBox_UserPin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {

                Regex regex = new Regex("[0-9]+");
                if (!regex.IsMatch(e.Text))
                {
                    e.Handled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
         
    }
}
