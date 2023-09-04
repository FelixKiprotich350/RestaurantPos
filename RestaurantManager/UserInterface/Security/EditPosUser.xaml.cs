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
    /// Interaction logic for EditPosUser.xaml
    /// </summary>
    public partial class EditPosUser : Window
    {
        public string ReturningAction = "";
        PosUser pu = null;
        public EditPosUser(PosUser user)
        {
            InitializeComponent();
            pu = user;
              
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            { 
                LoadRoles();
                Textbox_Currentrole.Text = pu.UserRole;
                if (pu.UserWorkingStatus == "Active")
                {
                    Button_Delete.Visibility = Visibility.Visible;
                    Button_Restore.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Button_Delete.Visibility = Visibility.Collapsed;
                    Button_Restore.Visibility = Visibility.Visible;
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
                var x = Enum.GetNames(typeof(GlobalVariables.PosEnums.UserAccountsRoles)).Cast<string>().ToList();
                x.ForEach(k => ComboBox_Roles.Items.Add(k));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReturningAction = "Delete";
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                this.DialogResult = false;
            }
        }

        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ComboBox_Roles.SelectedItem == null)
                {
                    MessageBox.Show("No role has been selected for Update!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                   
                    DialogResult = false;

                }
                else
                {
                    ReturningAction = "Update";
                    this.DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                this.DialogResult = false;
            }
        }

        private void Button_Restore_Click(object sender, RoutedEventArgs e)
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
