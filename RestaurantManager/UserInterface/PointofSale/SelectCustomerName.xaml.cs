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

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for SelectCustomerName.xaml
    /// </summary>
    public partial class SelectCustomerName : Window
    {
        public string SelectedCustomerName = "Customer";
        public SelectCustomerName()
        {
            InitializeComponent(); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                 
                using (var db = new PosDbContext())
                {
                    var data = db.Customer.ToList();
                    Listview_Customers.ItemsSource = data;
                } 

            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            SelectedCustomerName = "";
            this.Close();
        }

        private void Button_Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (Textbox_CustomerName.Text.Trim() != "")
                //{
                //    SelectedCustomerName = Textbox_CustomerName.Text;
                //    this.DialogResult = true;
                //}
                //else
                //{
                //    Close();
                //}
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
        }

    }
}
