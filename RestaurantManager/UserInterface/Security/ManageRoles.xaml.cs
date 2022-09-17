using RestaurantManager.BusinessModels.Security;
using RestaurantManager.UserInterface.PointofSale;
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
                RefreshRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void RefreshRoles()
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
                        RoleStatus = "Active",
                        RoleIsDeleted = "False",
                        RegistrationDate = GlobalVariables.SharedVariables.CurrentDate(),
                        LastUpdateDate = GlobalVariables.SharedVariables.CurrentDate(),
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
                RefreshRoles();
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

        private void Datagrid_RolesList_MouseUp(object sender, MouseButtonEventArgs e)
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
                    
                    if (Datagrid_RolesList.SelectedItem == null)
                    {
                        return;
                    }
                    UserRole o = (UserRole)Datagrid_RolesList.SelectedItem;
                    o.User_Permissions_final = new List<PermissionMaster>();
                    EditUserRole er = new EditUserRole(o);
                    er.ShowDialog();
                    if (er.ReturningAction == "Delete")
                    {
                        if (o.RoleName == GlobalVariables.SharedVariables.AdminRoleName)
                        {
                            MessageBox.Show("You can't Delete Admin Role!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        using (var db=new PosDbContext())
                        {
                            UserRole r = db.UserRoles.Where(a => a.RoleGuid == o.RoleGuid).First();
                            r.RoleStatus = "Deleted";
                            db.SaveChanges();
                            MessageBox.Show("Deleted Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                            
                        }
                    }
                    else if (er.ReturningAction == "Restore")
                    {

                        using (var db = new PosDbContext())
                        {
                            UserRole r = db.UserRoles.Where(a => a.RoleGuid == o.RoleGuid).First();
                            r.RoleStatus = "Active"; 
                            db.SaveChanges();
                            MessageBox.Show("Restored Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else if (er.ReturningAction == "Update")
                    {
                        if (o.RoleName == GlobalVariables.SharedVariables.AdminRoleName)
                        {
                            MessageBox.Show("You can't Update Admin Rights!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        using (var db = new PosDbContext())
                        {
                            UserRole r = db.UserRoles.Where(a => a.RoleGuid == o.RoleGuid).First();
                            string newrights = "";
                            foreach (PermissionMaster m in er.selectedrights)
                            {
                                newrights += m.PermissionGuid + ",";
                            }
                            r.RolePermissions = newrights;
                            db.SaveChanges();
                            MessageBox.Show("Role Updated Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    RefreshRoles();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
      
    }
}

 