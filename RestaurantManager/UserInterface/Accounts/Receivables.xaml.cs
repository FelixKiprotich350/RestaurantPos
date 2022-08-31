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

namespace RestaurantManager.UserInterface.Accounts
{
    /// <summary>
    /// Interaction logic for Receivables.xaml
    /// </summary>
    public partial class Receivables : Page
    {
        public Receivables()
        {
            InitializeComponent();
        }

        private void Button_Cash_Click(object sender, RoutedEventArgs e)
        {
            Label_SelectedAccountHeader.Content = "Cash Payments";
        }

        private void Button_Voucher_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Cards_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Mpesa_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
