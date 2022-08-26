using RestaurantManager.BusinessModels.Payments;
using RestaurantManager.BusinessModels.PointofSale;
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
    /// Interaction logic for CheckoutTicket.xaml
    /// </summary>
    public partial class CheckoutTicket : Page
    {
        public CheckoutTicket()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshTicketList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_RefreshTicketsList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshTicketList();
                MessageBox.Show("Done", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshTicketList()
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    LisTview_TicketsList.ItemsSource = db.OrderMaster.Where(p => p.OrderStatus != "Completed").ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_CheckoutTicket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TextBlock_TicketNo.Text == "")
                {
                    MessageBox.Show("Select a Ticket to CheckOut!", "Message Box", MessageBoxButton.OK);
                    return;
                }
                var db1 = new PosDbContext();
                if (db1.OrderMaster.Where(h => h.OrderNo == TextBlock_TicketNo.Text).Count() <= 0)
                {
                    MessageBox.Show("The Ticket Order does not Exist!", "Message Box", MessageBoxButton.OK);
                    return;
                }
                decimal total = 0;
                var a = Datagrid_TicketItems.Items.Cast<OrderItem>().ToList();
                foreach (OrderItem x in a)
                {
                    total += x.Price * x.Quantity;
                }
                if (total.ToString() != TextBox_TotalAmount.Text.ToString())
                {
                    MessageBox.Show("Select the Ticket and Try again!", "Message Box", MessageBoxButton.OK);
                    return;
                }
                List<PaymentMethod> pm = new List<PaymentMethod>();
                PaymentsUI T = new PaymentsUI(total, pm);
                if (T.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    pm = T.Payments;
                }
                TicketPaymentMaster tpm = new TicketPaymentMaster
                {
                    PaymentMasterGuid = Guid.NewGuid().ToString(),
                    TicketNo = TextBlock_TicketNo.Text,
                    PosUser = ErpShared.CurrentUser.Username.ToString(),
                    TotalAmountPaid = 0,
                    TotalAmountCharged = total,
                    TicketBalanceReturned = 0,
                    PaymentDate = ErpShared.CurrentDate()
                };
                using (var db=new PosDbContext())
                {
                    db.TicketPaymentMaster.Add(tpm);

                    db.SaveChanges(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void LisTview_TicketsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListView Lv = sender as ListView;
                if (Lv.SelectedItem == null)
                {
                    return;
                }
                OrderMaster om = (OrderMaster)Lv.SelectedItem;
                LoadTicketDetails(om);
                Lv.SelectedItem = null;
                Button_CheckoutTicket.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTicketDetails(OrderMaster order)
        {
            try
            {
                //get ticket items
                using (var db = new PosDbContext())
                {
                    var a = db.OrderItem.Where(o => o.OrderID == order.OrderNo).ToList();
                    Datagrid_TicketItems.ItemsSource = a;
                }
                //find total
                decimal total = 0;
                var b = Datagrid_TicketItems.Items.Cast<OrderItem>().ToList();
                foreach (OrderItem x in b)
                {
                    total += x.Price * x.Quantity;
                }
                TextBox_TotalAmount.Text = total.ToString(); 
                TextBlock_TicketNo.Text = order.OrderNo;
                TextBlock_TicketDate.Text = order.OrderDate.ToString();
                TextBlock_ItemsCount.Text = b.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_Discard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox_TotalAmount.Text = "0.00";
                TextBlock_TicketNo.Text = "";
                TextBlock_TicketDate.Text = "";
                TextBlock_ItemsCount.Text = "0";
                Datagrid_TicketItems.ItemsSource = null;
                Button_CheckoutTicket.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
 
    }
}
