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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for KitchenDisplay.xaml
    /// </summary>
    public partial class KitchenDisplay : Page
    {
        readonly List<KitchenTicket> Ticket_items = new List<KitchenTicket>();

        public KitchenDisplay()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SyncItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_PrepareClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = sender as Button;
                var db = new PosDbContext();
                if (b.Content.ToString() == "Prepare")
                {
                    var order = db.OrderMaster.FirstOrDefault(x => x.OrderNo == b.Tag.ToString());
                    if (order != null)
                    {
                        order.IsInPreparation = true;
                        db.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Ticket", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else if (b.Content.ToString() == "Complete")
                {
                    var order = db.OrderMaster.FirstOrDefault(x => x.OrderNo == b.Tag.ToString());
                    if (order != null)
                    {
                        order.IsKitchenServed = true;
                        db.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Ticket", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Action!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                SyncItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SyncItems()
        {
            try
            {
                Ticket_items.Clear();
                var db = new PosDbContext();
                List<OrderMaster> master = new List<OrderMaster>();
                master = db.OrderMaster.Where(k => k.IsKitchenServed == false).ToList();
                foreach (var x in master)
                {
                    KitchenTicket kt = new KitchenTicket();
                    var oi = db.OrderItem.Where(k => k.OrderID == x.OrderNo && k.IsItemVoided == false).ToList();
                    kt.Order = x;
                    kt.Orderitems = oi;
                    if (x.IsInPreparation)
                    {
                        kt.ButtonText = "Complete";
                        kt.PreparingBG = (Brush)FindResource("KD_Color2");
                        kt.ButtonStyle = (Style)FindResource("Button_Accept");
                    }
                    else
                    {
                        kt.ButtonText = "Prepare";
                        kt.PreparingBG = (Brush)FindResource("KD_Color1"); 
                        kt.ButtonStyle = (Style)FindResource("Button_Cancel");
                    }
                    Ticket_items.Add(kt);
                }
                IC_PendingTickets.ItemsSource = null;
                IC_PendingTickets.ItemsSource = Ticket_items;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public class KitchenTicket
        {
             
            public Style ButtonStyle { get; set; }
            public string ButtonText { get; set; }
            public Brush PreparingBG { get; set; }
            public OrderMaster Order { get; set; }
            public List<OrderItem> Orderitems { get; set; }
        }

        private void ListView_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                ListView l = sender as ListView;
                l.SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
