using DatabaseModels.Security;
using RestaurantManager.ApplicationFiles; 
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
                    if (db.PosUser.FirstOrDefault(k => k.UserName == Textbox_Username.Text.Trim().ToString()) != null)
                    {
                        MessageBox.Show("The Username already Exists. Try another Name!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (db.PosUser.FirstOrDefault(k => k.PhoneNumber == Textbox_Username.Text.Trim().ToString()) != null)
                    {
                        MessageBox.Show("The Phone Number already Exists. Try another Phone!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (!int.TryParse(Textbox_DefaultPin.Text.Trim(),out int pin))
                    {
                        MessageBox.Show("The Pin is not Allowed. Try another Pin!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    PosUser user = new PosUser
                    {
                        UserGuid = Guid.NewGuid().ToString(),
                        UserPIN = pin,
                        IsDefaultpin = true,
                        UserName = Textbox_Username.Text.Trim(),
                        PhoneNumber = Textbox_PhoneNumber.Text.Trim(),
                        UserFullName = Textbox_UserFullName.Text,
                        UserRole = ComboBox_Roles.SelectedItem.ToString(),
                        RegistrationDate = GlobalVariables.SharedVariables.CurrentDate(),
                        LastLoginDate = GlobalVariables.SharedVariables.CurrentDate(),
                        UserWorkingStatus = "Active",
                        UserIsDeleted = false,
                        IsBackendUser = false,
                        IsPosUser=true,
                        UserRights = ","
                        
                    };
                    db.PosUser.Add(user);
                    int x = db.SaveChanges();
                    if (x != 1)
                    {
                        MessageBox.Show("Failed to save the new User!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Successfully Saved The default PIN is " + user.UserPIN + "\nLogin and change your PIN now!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
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
