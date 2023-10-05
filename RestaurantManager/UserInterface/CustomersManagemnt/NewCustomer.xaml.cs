using DatabaseModels.CRM;
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

namespace RestaurantManager.UserInterface.CustomersManagemnt
{
    /// <summary>
    /// Interaction logic for NewCustomer.xaml
    /// </summary>
    public partial class NewCustomer : Window
    {
        public NewCustomer()
        {
            InitializeComponent();
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string accno = "ACC-"+GlobalVariables.SharedVariables.GeneratePersonAccNo();
                PersonalAccount c = new PersonalAccount
                {
                    PersonGuid = Guid.NewGuid().ToString(),
                    FullName = Textbox_FullName.Text,
                    AccountNo =accno,
                    InvoiceLimit = 0,
                    AccountStatus = GlobalVariables.PosEnums.PersonAccountStatus.Active.ToString(),
                    PhoneNumber = Textbox_PhoneNo.Text,
                    Gender = Textbox_PhoneNo.Text.Trim(),
                    Email = Combobox_Gender.SelectedItem.ToString(),
                    RegistrationDate = GlobalVariables.SharedVariables.CurrentDate(),
                    BirthDate = (DateTime)DatePicker_BirthDate.SelectedDate,
                    UpdateDate = GlobalVariables.SharedVariables.CurrentDate()
                };
                using (var db = new PosDbContext())
                {
                    db.PersonalAccount.Add(c);
                    db.SaveChanges();
                }
                MessageBox.Show("Successfully Saved!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }


        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Checkbox_DefaultBirthDate_Checked(object sender, RoutedEventArgs e)
        {
            DatePicker_BirthDate.SelectedDate = DateTime.MinValue;
        }

        private void Checkbox_DefaultBirthDate_Unchecked(object sender, RoutedEventArgs e)
        {
            DatePicker_BirthDate.SelectedDate = null;
        }
    }
}
