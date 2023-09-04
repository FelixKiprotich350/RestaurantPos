using DatabaseModels.Security;
using RestaurantManager.ApplicationFiles; 
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
       // readonly Random Rand = new Random();
        public ManageSystemUsers()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshUsers(); 
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
                if (dep is DataGridCell cell)
                {

                    if (Datagrid_UsersList.SelectedItem == null)
                    {
                        return;
                    }
                    PosUser o = (PosUser)Datagrid_UsersList.SelectedItem;
                    if (cell.Column.DisplayIndex.ToString() == "2")
                    {
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
                                //UserRole role = (UserRole)er.ComboBox_Roles.SelectedItem;
                                r.UserRole = er.ComboBox_Roles.Text;
                                db.SaveChanges();
                                MessageBox.Show("User Role Updated Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        RefreshUsers();
                    }
                    Datagrid_UsersList.SelectedItem = o;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_NewUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddNewUser anu = new AddNewUser();
                anu.ShowDialog();
                RefreshUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_UpdateRights_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tag=Label_Username.Tag;
                if (tag is PosUser user)
                {
                    var items = Listview_SelectedRolerights.Items.Cast<PermissionMaster>().ToList().Where(k => k.IsSelected).ToList();
                    using (var db = new PosDbContext())
                    {
                        string rights = "";
                        foreach (var x in items)
                        {
                            rights += x.PermissionCode + ",";
                        }
                        db.PosUser.Where(k => k.UserName == user.UserName).First().UserRights = rights;
                        db.SaveChanges();
                        MessageBox.Show("Success . Rights Updated!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("The user is Invalid!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Datagrid_UsersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Datagrid_UsersList.SelectedItem == null)
                {
                    return;
                }
                PosUser o = (PosUser)Datagrid_UsersList.SelectedItem;
                GetUserRights(o);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetUserRights(PosUser p)
        {
            try
            {
                var db = new PosDbContext();
                var user = db.PosUser.AsNoTracking().Where(k=>k.UserName==p.UserName).FirstOrDefault();
                if(user is null)
                {
                    MessageBox.Show("The user does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);

                    return;
                }
                List<PermissionMaster> pm = new List<PermissionMaster>();
                Permissions master = new Permissions();
                var items = master.GetAllPermissions();
                var rights = user.UserRights.Split(',').ToList();
                foreach (var x in rights)
                {
                    try
                    {
                        items.Where(k => k.PermissionCode == x).First().IsSelected = true;
                    }
                    catch
                    {

                    }
                }
                Label_Username.Tag = user;
                Listview_SelectedRolerights.ItemsSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
