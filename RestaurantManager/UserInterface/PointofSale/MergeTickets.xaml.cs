using RestaurantManager.BusinessModels.OrderTicket;
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
    /// Interaction logic for MergeTickets.xaml
    /// </summary>
    public partial class MergeTickets : Window
    {
        public MergeTickets()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTicketList();
        }
        private void RefreshTicketList()
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    LisTview_TicketsList.ItemsSource = db.OrderMaster.Where(p => p.OrderStatus == "Pending" && p.UserServing == ErpShared.CurrentUser.UserName).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = LisTview_TicketsList.Items.Cast<OrderMaster>().Where(x => x.IsSelected).ToList();
                ListView_SelectedTickets.ItemsSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListView_SelectedTickets_LostFocus(object sender, RoutedEventArgs e)
        {
            ListView_SelectedTickets.SelectedItem = null;
        }
    }
}
