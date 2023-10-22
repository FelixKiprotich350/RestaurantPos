using DatabaseModels.CRM;
using DatabaseModels.OrderTicket;
using DatabaseModels.WorkPeriod;
using RestaurantManager.GlobalVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for MergeTickets.xaml
    /// </summary>
    public partial class MergeTickets : Window
    {
        readonly Random R = new Random();
        readonly List<OrderMaster> ticketsmaster = new List<OrderMaster>();
        List<OrderMaster> selectedtickets = new List<OrderMaster>();
        public MergeTickets(List<OrderMaster> t)
        {
            InitializeComponent();
            ticketsmaster = t;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LisTview_TicketsList.ItemsSource = ticketsmaster;
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
                selectedtickets = data;
                ListView_SelectedTickets.ItemsSource = selectedtickets;
                ticketsmaster.ToList().ForEach(x => x.IsSelected = false);
                LisTview_TicketsList.ItemsSource = null;
                LisTview_TicketsList.ItemsSource = ticketsmaster;
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
                    Button_SelectCustomer.Tag = s.SelectedCustomer;
                    Button_SelectCustomer.Content = s.SelectedCustomer.FullName;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 

        private CustomerAccount GetCustomer()
        {
            try
            {
                if (Button_SelectCustomer.Tag is null)
                {
                    return null;
                }

                if (Button_SelectCustomer.Tag.GetType() == typeof(CustomerAccount))
                {
                    if ((CustomerAccount)Button_SelectCustomer.Tag != null)
                    {
                        return (CustomerAccount)Button_SelectCustomer.Tag;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        private void Button_Merge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = ListView_SelectedTickets.Items.Cast<OrderMaster>().ToList();
                if (data.Count <= 1)
                {
                    MessageBox.Show("You cannot Merge less than Two (2) Tickets!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                //check whether work period is open
                WorkPeriod wp = GlobalVariables.SharedVariables.CurrentOpenWorkPeriod();
                if (wp == null)
                {
                    MessageBox.Show("No WorkPeriod Open!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                //remove item and add voided item
                using (var db = new PosDbContext())
                {
                    string ordno = SharedVariables.CurrentDate().ToString("ddmmyy") + "-" + R.Next(0, 999).ToString();
                    string ordguid = Guid.NewGuid().ToString();
                    List<OrderItem> newitems = new List<OrderItem>();
                    foreach (var t in data)
                    {
                        OrderMaster x = db.OrderMaster.Where(a => a.OrderNo == t.OrderNo).First();
                        x.MergedChild = ordguid;
                        x.OrderStatus = "Merged";
                        var y=db.OrderItem.Where(a => a.OrderID == x.OrderNo );
                        foreach (var m in y)
                        {
                            OrderItem item = new OrderItem()
                            {
                                ItemRowGuid = Guid.NewGuid().ToString(),
                                ItemName = m.ItemName,
                                OrderID = ordno, 
                                ProductItemGuid = m.ProductItemGuid,
                                Price = m.Price,
                                Quantity = m.Quantity, 
                                ServiceType = m.ServiceType,
                                Total = m.Total
                            };
                            newitems.Add(item);
                        }
                    } 
                    OrderMaster om = new OrderMaster
                    {
                        OrderGuid = ordguid,
                        IsPrinted = false,
                        OrderNo = ordno,
                        VoidReason = "None",
                        CustomerRefference = GetCustomer() != null ? GetCustomer().PersonAccNo : "None",
                        TicketTable = Button_SelectTable.Content.ToString(),
                        UserServing = GlobalVariables.SharedVariables.CurrentUser.UserName,
                        OrderStatus = PosEnums.OrderTicketStatuses.Pending.ToString(),
                        OrderDate = GlobalVariables.SharedVariables.CurrentDate(),
                        PaymentDate = GlobalVariables.SharedVariables.CurrentDate(),
                        Workperiod = wp.WorkperiodName
                    };

                    db.OrderMaster.Add(om);
                    db.OrderItem.AddRange(newitems);
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

        private void Button_Unselect_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                ListView_SelectedTickets.ItemsSource = null; 
                ticketsmaster.ToList().ForEach(x => x.IsSelected = false); 
                LisTview_TicketsList.ItemsSource = ticketsmaster; 
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
