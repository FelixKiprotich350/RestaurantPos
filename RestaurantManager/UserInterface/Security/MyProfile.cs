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
    /// Interaction logic for MyPin.xaml
    /// </summary>
    public partial class MyProfile : Page
    {
        public MyProfile()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Textbox_FullName.Text = ErpShared.CurrentUser.UserFullName;
                Textbox_username.Text = ErpShared.CurrentUser.UserName;
                Textbox_RegistrationDate.Text = ErpShared.CurrentUser.RegistrationDate.ToShortDateString();
                Textbox_Status.Text = ErpShared.CurrentUser.UserWorkingStatus;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        private void Button_ChangePin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Pass_OldPin.Password.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter OLD PIN !", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Pass_NewPin.Password.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter NEW PIN !", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Pass_ConfirmNewpin.Password.Trim() == string.Empty)
                {
                    MessageBox.Show("Confirm NEW PIN !", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!int.TryParse(Pass_OldPin.Password, out int oldpin))
                {
                    MessageBox.Show("Incorrect OLD PIN!!!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (oldpin!=ErpShared.CurrentUser.UserPIN)
                {
                    MessageBox.Show("Incorrect OLD PIN!!!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Pass_NewPin.Password != Pass_ConfirmNewpin.Password)
                {
                    MessageBox.Show("The new PIN does not match!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!int.TryParse(Pass_NewPin.Password, out int newpin))
                {
                    MessageBox.Show("PIN must be Numbers Only!. Try another pin!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (oldpin==newpin)
                {
                    MessageBox.Show("The New PIN must not be the same as the old Pin", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                using (var db=new PosDbContext())
                {
                    {
                        PosUser p = db.PosUser.Where(x => x.UserName == ErpShared.CurrentUser.UserName).First();
                        p.UserPIN = newpin;
                        db.SaveChanges();
                        MessageBox.Show("You have successfully Changed your PIN TO " + newpin.ToString()+"\nThe application will shut down Immediately.\nLogin using the new PIN!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        App.Current.Shutdown();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
             
            }
        }

       
    }
}
