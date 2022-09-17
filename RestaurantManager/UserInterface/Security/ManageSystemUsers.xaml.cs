using RestaurantManager.BusinessModels.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
                    ComboBox_Roles.ItemsSource = db.UserRoles.Where(x => x.RoleStatus == "Active").ToList();
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
                
                using (var db = new PosDbContext())
                {
                    PosUser user = new PosUser
                    {
                        UserGuid = Guid.NewGuid().ToString(),
                        UserPIN = Rand.Next(1000, 9999),
                        UserName = Textbox_UserFullName.Text.Split(',')[0] + Rand.Next(100, 9999),
                        UserFullName = Textbox_UserFullName.Text,
                        UserRole = ((UserRole)ComboBox_Roles.SelectedItem).RoleName,
                        RegistrationDate = GlobalVariables.SharedVariables.CurrentDate(),
                        LastLoginDate = GlobalVariables.SharedVariables.CurrentDate(),
                        UserWorkingStatus = "Active",
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

        private void Datagrid_UsersList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DependencyObject dep = (DependencyObject)e.OriginalSource;

                // iteratively traverse the visual tree
                while ((dep != null) & !(dep is DataGridCell) & !(dep is DataGridColumnHeader))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }

                if (dep == null)
                {
                    return;
                }
                if (dep is DataGridCell)
                {

                    if (Datagrid_UsersList.SelectedItem == null)
                    {
                        return;
                    }
                    PosUser o = (PosUser)Datagrid_UsersList.SelectedItem;
                    o.User_Permissions_final = new List<PermissionMaster>();
                    EditPosUser er = new EditPosUser(o);
                    er.ShowDialog();
                    if (er.ReturningAction == "Delete")
                    {
                        if (o.UserName == GlobalVariables.SharedVariables.CurrentUser.UserName)
                        {
                            MessageBox.Show("You can't Delete Your own Account!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        using (var db = new PosDbContext())
                        {
                            PosUser r = db.PosUser.Where(a => a.UserName == o.UserName).First();
                            r.UserWorkingStatus = "Disabled";
                            db.SaveChanges();
                            MessageBox.Show("User Deleted Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                    } 
                    else if (er.ReturningAction == "Restore")
                    {
                        if (o.UserName == GlobalVariables.SharedVariables.CurrentUser.UserName)
                        {
                            MessageBox.Show("You can't Restore Your own Account!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        using (var db = new PosDbContext())
                        {
                            PosUser r = db.PosUser.Where(a => a.UserName == o.UserName).First();
                            r.UserWorkingStatus = "Active";
                            db.SaveChanges();
                            MessageBox.Show("User Restored Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                    } 
                    else if (er.ReturningAction == "Update")
                    {                         
                        using (var db = new PosDbContext())
                        {
                            PosUser r = db.PosUser.Where(a => a.UserName == o.UserName).First();
                            UserRole role = (UserRole)er.ComboBox_Roles.SelectedItem;
                            r.UserRole = role.RoleName;
                            db.SaveChanges();
                            MessageBox.Show("User Role Updated Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
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
