using RestaurantManager.ApplicationFiles;
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
    /// Interaction logic for EditPosUser.xaml
    /// </summary>
    public partial class AddNewUser : Window
    {
        readonly Random Rand = new Random();
        public AddNewUser()
        {
            InitializeComponent(); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            { 
                LoadRoles();
                
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
                        UserRole = ComboBox_Roles.SelectedItem.ToString(),
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
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
