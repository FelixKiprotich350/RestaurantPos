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
        PermissionMaster Pm = new PermissionMaster();
        public Login()
        {
            InitializeComponent();
            ErpShared.CurrentUser = null;
        }

        public void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_Username.Text != "" && Password_Box.Password != "")
                {
                    PosUser user = null;
                    using (var db = new PosDbContext())
                    {
                        if (db.PosUser.Where(a => a.Username.ToString() == Textbox_Username.Text.Trim()).Count() > 0)
                        {
                            user = db.PosUser.Where(a => a.Username.ToString() == Textbox_Username.Text.Trim()).First();
                        }
                        else
                        {
                            return;
                        }
                    }
                    //raw permissions
                    using (var db = new PosDbContext())
                    {
                        user.User_Permissions_raw = db.UserPermission.Where(a => a.UserGuid == user.UserGuid).ToList();
                    }
                    //final permissions
                    if (user.User_Permissions_raw.Count > 0)
                    {
                        user.User_Permissions_final = new List<PermissionMaster>();
                        foreach (var a in user.User_Permissions_raw)
                        {
                            if (Pm.GetAllPermissions().Where(b => b.PermissionGuid == a.ParentPermissionGuid).Count() > 0)
                            {
                                user.User_Permissions_final.Add(Pm.GetAllPermissions().Where(b => b.PermissionGuid == a.ParentPermissionGuid).First());
                            }
                        }
                    }
                    ErpShared.CurrentUser = user;
                    this.DialogResult = true;
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
            this.DialogResult = false;
        }
    }
}
