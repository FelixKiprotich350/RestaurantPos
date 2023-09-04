using DatabaseModels.Security; 
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
    /// Interaction logic for SelectRightsForRole.xaml
    /// </summary>
    public partial class SelectRightsForRole : Window
    {
        readonly Permissions pm = new Permissions();
        public List<PermissionMaster> selectedrights = new List<PermissionMaster>();
        public SelectRightsForRole(List<PermissionMaster> selecteditems)
        {
            InitializeComponent();
            selectedrights = selecteditems;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                var allrights = pm.GetAllPermissions();
                foreach(var x in selectedrights)
                {
                    allrights.Find(a => a.PermissionCode == x.PermissionCode).IsSelected = true;
                }
                ListView_Rights.ItemsSource = allrights;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectedrights = ListView_Rights.Items.Cast<PermissionMaster>().Where(a=>a.IsSelected).ToList();
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }
    }
}
