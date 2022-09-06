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

namespace RestaurantManager.UserInterface.Security
{

    /// <summary>
    /// Interaction logic for EditUserRole.xaml
    /// </summary>
    public partial class EditUserRole : Window
    {
        public string ReturningAction = "";
        public UserRole SelectedRole = null;
        readonly PermissionMaster pm = new PermissionMaster();
        public List<PermissionMaster> selectedrights = new List<PermissionMaster>();
        public EditUserRole(object Role)
        {
            InitializeComponent();
            SelectedRole = (UserRole)Role;
            if (SelectedRole != null)
            {
                if (SelectedRole.RoleStatus == "Active")
                {
                    Button_DeleteteRole.Visibility = Visibility.Visible; 
                }
                else
                {
                    Button_RestoreRole.Visibility = Visibility.Visible;
                } 
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var allrights = pm.GetAllPermissions();
                List<string> raw = SelectedRole.RolePermissions.Split(',').Where(z => z != "").ToList();
                foreach (var x in raw)
                {
                    selectedrights.Add(allrights.Find(a => a.PermissionGuid == x));
                }
                foreach (var x in selectedrights)
                {
                    allrights.Find(a => a.PermissionGuid == x.PermissionGuid).IsSelected = true;
                }
                ListView_Rights.ItemsSource = allrights;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_DeleteteRole_Click(object sender, RoutedEventArgs e)
        {
            ReturningAction = "Delete"; 
            this.DialogResult = true;
        }

        private void Button_UpdateRights_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectedrights = ListView_Rights.Items.Cast<PermissionMaster>().Where(a => a.IsSelected).ToList();
                ReturningAction = "Update";
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                this.DialogResult = false;
            }
        }

        private void Button_RestoreRole_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                ReturningAction = "Restore";
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                this.DialogResult = false;
            }
        }
    }
}
