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
    /// Interaction logic for ManageSystemUsers.xaml
    /// </summary>
    public partial class ManageSystemUsers : Page
    {
        readonly Random Rand = new Random();
        public ManageSystemUsers()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshUsers();
                LoadRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void RefreshUsers()
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    Datagrid_UsersList.ItemsSource = db.PosUser.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadRoles()
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    ComboBox_Roles.ItemsSource = db.UserRoles.ToList();
                }
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
                list = Listview_SelectedRolerights.Items.Cast<PermissionMaster>().ToList();
                SelectRightsForRole select = new SelectRightsForRole(list);
                if ((bool)select.ShowDialog())
                {
                    Listview_SelectedRolerights.ItemsSource = select.selectedrights;
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
                    MessageBox.Show("You must select atleast one Right!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                string rights = "";
                foreach (var x in Listview_SelectedRolerights.Items.Cast<PermissionMaster>())
                {
                    rights += x.PermissionGuid + ",";
                }
                using (var db = new PosDbContext())
                {
                    PosUser user = new PosUser
                    {
                        UserGuid = Guid.NewGuid().ToString(),
                        UserPIN = Rand.Next(1000, 9999),
                        UserFullName = Textbox_UserFullName.Text,
                        UserRole = ((UserRole)ComboBox_Roles.SelectedItem).RoleGuid,
                        RegistrationDate = ErpShared.CurrentDate(),
                        LastLoginDate = ErpShared.CurrentDate(),
                        UserRights = rights,
                        UserWorkingStatus="Active",
                        UserIsDeleted = false
                    };
                    db.PosUser.Add(user);
                    int x = db.SaveChanges();
                    if (x != 1)
                    {
                        MessageBox.Show("Failed to save the new User!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    MessageBox.Show("Successfully Saved.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshUsers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
