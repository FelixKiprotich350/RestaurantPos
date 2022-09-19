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
    /// Interaction logic for TrackTicketMaster.xaml
    /// </summary>
    public partial class TrackTicketMaster : Window
    {
        readonly OrderMaster Ticket = null;
        public TrackTicketMaster(OrderMaster o)
        {
            InitializeComponent();
            Ticket = o;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                using (var db = new PosDbContext())
                {
                    var tick = db.OrderMaster.Where(o => o.OrderNo == Ticket.OrderNo).First();
                    this.Title = "Track Ticket - " + tick.OrderNo;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
