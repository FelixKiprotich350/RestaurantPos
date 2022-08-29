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
                if (Textbox_Username.Text != "" )
                {
                    PosUser user = null;
                    using (var db = new PosDbContext())
                    {
                        if (db.PosUser.Where(a => a.UserPIN.ToString() == Textbox_Username.Text.Trim()).Count() > 0)
                        {
                            user = db.PosUser.Where(a => a.UserPIN.ToString() == Textbox_Username.Text.Trim()).First();
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
