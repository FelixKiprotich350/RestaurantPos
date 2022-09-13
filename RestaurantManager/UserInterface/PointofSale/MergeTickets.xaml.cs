using RestaurantManager.BusinessModels.OrderTicket;
using RestaurantManager.BusinessModels.WorkPeriod;
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
        readonly Random R = new Random();
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
                    LisTview_TicketsList.ItemsSource = db.OrderMaster.Where(p => p.OrderStatus == ErpShared.OrderTicketStatuses.Pending.ToString() && p.UserServing == ErpShared.CurrentUser.UserName).ToList();
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
                if (data.Count <= 1)
                {
                    MessageBox.Show("Select Two or More Tickets to Merge!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
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

        private void Button_SelectTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectTableForTicket s = new SelectTableForTicket();
                if ((bool)s.ShowDialog())
                {
                    Button_SelectTable.Content = s.SelectedTable;
                }

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void Button_SelectCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectCustomerName s = new SelectCustomerName();
                if ((bool)s.ShowDialog())
                {
                    Button_SelectCustomer.Content = s.SelectedCustomerName;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Merge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = ListView_SelectedTickets.Items.Cast<OrderMaster>().Where(x => x.IsSelected).ToList();
                if (data.Count <= 1)
                {
                    MessageBox.Show("You cannot Merge less than Two (2) Tickets!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                //check whether work period is open
                WorkPeriod wp = ErpShared.CurrentOpenWorkPeriod();
                if (wp == null)
                {
                    MessageBox.Show("No WorkPeriod Open!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                //remove item and add voided item
                using (var db = new PosDbContext())
                {
                    string ordno = ErpShared.CurrentDate().ToString("ddmmyy") + "-" + R.Next(0, 999).ToString();
                    string ordguid = Guid.NewGuid().ToString();
                    List<OrderItem> newitems = new List<OrderItem>();
                    foreach (var t in data)
                    {
                        OrderMaster x = db.OrderMaster.Where(a => a.OrderNo == t.OrderNo).First();
                        x.MergedChild = ordguid;
                        x.OrderStatus = ErpShared.OrderTicketStatuses.Merged.ToString();
                        db.OrderItem.Where(a => a.OrderID == x.OrderNo).ToList().ForEach(b => b.OrderID = ordno);
                    }
                    OrderMaster om = new OrderMaster
                    {
                        OrderGuid = ordguid,
                        OrderNo = ordno,
                        CustomerName = Button_SelectCustomer.Content.ToString(),
                        TicketTable = Button_SelectTable.Content.ToString(),
                        UserServing = ErpShared.CurrentUser.UserName,
                        OrderStatus = ErpShared.OrderTicketStatuses.Pending.ToString(),
                        OrderDate = ErpShared.CurrentDate(),
                        PaymentDate = ErpShared.CurrentDate(),
                        Workperiod = wp.WorkperiodName
                    };
                    db.OrderMaster.Add(om);
                    db.SaveChanges();
                }
                MessageBox.Show("Tickets Merged Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
