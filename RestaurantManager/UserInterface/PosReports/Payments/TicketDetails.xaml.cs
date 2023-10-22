using DatabaseModels.OrderTicket;
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

namespace RestaurantManager.UserInterface.PosReports.Payments
{
    /// <summary>
    /// Interaction logic for PaidTicketDetails.xaml
    /// </summary>
    public partial class TicketDetails : Window
    {
        public OrderMaster SelectedTicket = null;
        public TicketDetails( OrderMaster o)
        {
            InitializeComponent();
            SelectedTicket = o;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (SelectedTicket!=null)
            {
                GetTicketDetails(SelectedTicket);
            }
            else
            {
                Close();
            }
        }

        private void GetTicketDetails(OrderMaster om)
        {
            try
            {
                var db = new PosDbContext();
                var items = db.OrderItem.AsNoTracking().Where(k => k.OrderID == om.OrderNo).ToList();
                Textbox_TicketNumber.Text = om.OrderNo;
                Textbox_postedby.Text = om.UserServing;
                Textbox_Status.Text = om.OrderStatus;
                Textbox_Date.Text = om.OrderDate.ToString();
                Textbox_ItemsCount.Text = items.Count.ToString();
                Textbox_Workperiodd.Text = om.Workperiod.ToString();
                Datagrid_TicketItems.ItemsSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
