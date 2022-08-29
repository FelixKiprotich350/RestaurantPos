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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantManager.UserInterface.Security
{
    /// <summary>
    /// Interaction logic for Manage_Roles.xaml
    /// </summary>
    public partial class ManageRoles : Page
    {

        public ManageRoles()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void RefreshCategories()
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    Datagrid_RolesList.ItemsSource = db.UserRoles.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Listview_SelectedRolerights.Items.Count <= 0)
                {
                    MessageBox.Show("You must select atleast one Right!","Message Box",MessageBoxButton.OK,MessageBoxImage.Warning);
                    return;
                }
                string rights = "";
                foreach (var x in Listview_SelectedRolerights.Items.Cast<PermissionMaster>())
                {
                    rights += x.PermissionGuid + ",";
                }
                using (var db = new PosDbContext())
                {
                    UserRole ur = new UserRole
                    {
                        RoleGuid = Guid.NewGuid().ToString(),
                        RoleName = Textbox_RoleName.Text,
                        RoleDescription = Textbox_RoleDescription.Text,
                        RoleIsDeleted = "False",
                        RegistrationDate = ErpShared.CurrentDate(),
                        LastUpdateDate = ErpShared.CurrentDate(),
                        RolePermissions = rights
                    };
                    db.UserRoles.Add(ur);
                    int x = db.SaveChanges();
                    if (x != 1)
                    {
                        MessageBox.Show("Failed to save the new Role!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    MessageBox.Show("Successfully Saved", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        { 
            try
            {
                RefreshCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_SelectPermission_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                List<PermissionMaster> list = new List<PermissionMaster>();
                list =  Listview_SelectedRolerights.Items.Cast<PermissionMaster>().ToList();
                SelectRightsForRole select = new SelectRightsForRole(list);
               if((bool)select.ShowDialog())
                {
                    Listview_SelectedRolerights.ItemsSource = select.selectedrights;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

 