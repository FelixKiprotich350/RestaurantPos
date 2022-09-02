using RestaurantManager.BusinessModels.PointofSale;
using RestaurantManager.UserInterface.Security;
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
    /// Interaction logic for ManageTickets.xaml
    /// </summary>
    public partial class ManageTickets : Page
    {
        public ManageTickets()
        {
            InitializeComponent();
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

        private void Button_RefreshTicketsList_Click(object sender, RoutedEventArgs e)
        {
            RefreshTicketList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTicketList();
            Button_VoidTicket.IsEnabled = false;
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
                Button_VoidTicket.IsEnabled = true;
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
                    Datagrid_OrderItems.ItemsSource = a;
                }
                //find total
                decimal total = 0;
                var b = Datagrid_OrderItems.Items.Cast<OrderItem>().ToList();
                foreach (OrderItem x in b)
                {
                    total += x.Price * x.Quantity;
                }
                TextBlock_TicketNo.Text = order.OrderNo;
                TextBlock_TicketDate.Text = order.OrderDate.ToString();
                TextBlock_ItemsCount.Text = b.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_VoidTicket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TextBlock_TicketNo.Text.Trim() == "")
                {
                    MessageBox.Show("Select a Ticket To Manage!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                PromptAdminPin p = new PromptAdminPin();

                if ((bool)p.ShowDialog())
                {
                    using (var db = new PosDbContext())
                    {
                        db.OrderMaster.Where(o => o.OrderNo == TextBlock_TicketNo.Text.Trim()).First().OrderStatus="Cancelled";
                        db.SaveChanges();
                    }
                    MessageBox.Show("Completed.Ticket Void Success!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshTicketList();
                    Button_CancelEditing_Click(new object(), new RoutedEventArgs());
                }
                else
                {
                    MessageBox.Show("Admin Approval Rejected!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_CancelEditing_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button_VoidTicket.IsEnabled = false;
                
                TextBlock_TicketNo.Text = "";
                TextBlock_TicketDate.Text = "";
                TextBlock_ItemsCount.Text = "0";
                Datagrid_OrderItems.ItemsSource = null;
                RefreshTicketList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
          
        }
    }
}
