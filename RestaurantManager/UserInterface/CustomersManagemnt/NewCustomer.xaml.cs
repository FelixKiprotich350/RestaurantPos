using RestaurantManager.BusinessModels.CustomersManagement;
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
                Customer c = new Customer
                {
                    CustomerGuid = Guid.NewGuid().ToString(),
                    CustomerName = Textbox_FullName.Text,
                    PhoneNumber = Textbox_PhoneNo.Text,
                    Gender = Combobox_Gender.SelectedItem.ToString(),
                    RegistrationDate = GlobalVariables.SharedVariables.CurrentDate(),
                    BirthDate = (DateTime)DatePicker_BirthDate.SelectedDate
                };
                using (var db = new PosDbContext())
                {
                    db.Customer.Add(c);
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
    }
}
