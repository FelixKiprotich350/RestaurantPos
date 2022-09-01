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
        public string CustomerName = "";
        public SelectCustomerName()
        {
            InitializeComponent();
            CustomerName = "";
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            CustomerName = "";
            this.Close();
        }

        private void Button_Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_CustomerName.Text.Trim() != "")
                {
                    CustomerName = Textbox_CustomerName.Text;
                }
                this.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
