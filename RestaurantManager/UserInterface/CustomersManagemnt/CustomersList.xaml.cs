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

namespace RestaurantManager.UserInterface.CustomersManagemnt
{
    /// <summary>
    /// Interaction logic for CustomersList.xaml
    /// </summary>
    public partial class CustomersList : Page
    {
        public CustomersList()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCustomers();
        }
        private void LoadCustomers()
        {
            try
            { 
                using (var db = new PosDbContext())
                {
                    var data = db.Customer.ToList();
                    Datagrid_CustomersList.ItemsSource = data;
                } 
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_NewCustomer_Click(object sender, RoutedEventArgs e)
        {
            NewCustomer cust = new NewCustomer();
            cust.ShowDialog();

        }

       
    }
}
