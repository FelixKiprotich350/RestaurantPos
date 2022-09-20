using RestaurantManager.BusinessModels.OrderTicket;
using RestaurantManager.BusinessModels.WorkPeriod;
using RestaurantManager.GlobalVariables;
using RestaurantManager.UserInterface.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
                List<OrderMaster> items = new List<OrderMaster>();
                using (var db = new PosDbContext())
                {
                    items = db.OrderMaster.Where(p => p.OrderStatus == PosEnums.OrderTicketStatuses.Pending.ToString() && !p.IsPrinted).ToList();
                }
                if (SharedVariables.CurrentUser.UserRole == SharedVariables.AdminRoleName)
                {
                    LisTview_TicketsList.ItemsSource = items;
                }
                else
                {
                    LisTview_TicketsList.ItemsSource = items.Where(p => p.UserServing == SharedVariables.CurrentUser.UserName);
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
            ResetForm();
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
                TextBox_Table.Text = order.TicketTable;
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
                        db.OrderMaster.Where(o => o.OrderNo == TextBlock_TicketNo.Text.Trim()).First().OrderStatus = "Cancelled";
                        db.SaveChanges();
                    }
                    MessageBox.Show("Completed.Ticket Void Success!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshTicketList();
                    ResetForm();
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

        private void ResetForm()
        {
            try
            {
                Button_VoidTicket.IsEnabled = false;

                TextBlock_TicketNo.Text = "";
                TextBlock_TicketDate.Text = "";
                TextBlock_ItemsCount.Text = "";
                TextBox_Table.Text = "";
                Datagrid_OrderItems.ItemsSource = null;
                RefreshTicketList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_CancelEditing_Click(object sender, RoutedEventArgs e)
        {

            ResetForm();
        }

        private void Button_MoveTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectTableForTicket st = new SelectTableForTicket();
                if ((bool)st.ShowDialog())
                {
                    using (var db = new PosDbContext())
                    {
                        db.OrderMaster.Where(o => o.OrderNo == TextBlock_TicketNo.Text.Trim()).First().TicketTable = st.SelectedTable;
                        db.SaveChanges();
                    }
                    MessageBox.Show("Success. Table Updated!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetForm();
                    RefreshTicketList();
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

        private void Datagrid_OrderItems_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                // iteratively traverse the visual tree
                while ((dep != null) & !(dep is DataGridCell) & !(dep is DataGridColumnHeader))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }
                if (dep == null)
                {
                    return;
                }
                if (dep is DataGridCell)
                {
                    if (Datagrid_OrderItems.SelectedItem == null)
                    {
                        return;
                    }
                    OrderItem o = (OrderItem)Datagrid_OrderItems.SelectedItem;
                    EditTicketRemoveItem ei = new EditTicketRemoveItem(o.ItemName);
                    ei.ShowDialog();
                    if (!(bool)ei.DialogResult)
                    {
                        return;
                    }
                    if (ei.ReturningAction != "Delete")
                    {
                        return;
                    }

                    PromptAdminPin p = new PromptAdminPin();
                    if ((bool)p.ShowDialog())
                    {
                        WorkPeriod wp = GlobalVariables.SharedVariables.CurrentOpenWorkPeriod();
                        if (wp == null)
                        {
                            MessageBox.Show("No WorkPeriod Open!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        //check if this is the last item in the ticket
                        using (var db = new PosDbContext())
                        {
                            if (db.OrderItem.Where(a => a.OrderID == o.OrderID).Count() <= 1)
                            {
                                MessageBox.Show("This is the last Item in the Ticket.\nKindly consider Voiding the Ticket!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                        }
                        //remove item and add voided item
                        using (var db = new PosDbContext())
                        {
                            OrderItem x = db.OrderItem.Where(a => a.OrderID == TextBlock_TicketNo.Text.Trim() && a.ItemRowGuid == o.ItemRowGuid).First();
                            db.OrderItem.Remove(x);
                            OrderItemVoided ov = new OrderItemVoided
                            {
                                ItemRowGuid = x.ItemRowGuid,
                                ParentProductItemGuid = x.ParentProductItemGuid,
                                ItemName = x.ItemName,
                                OrderID = x.OrderID,
                                VoidTime = GlobalVariables.SharedVariables.CurrentDate(),
                                ApprovedBy = p.ApprovingAdmin,
                                VoidReason = ei.Textbox_Description.Text,
                                WorkPeriod = wp.WorkperiodName
                            };
                            db.OrderItemVoided.Add(ov);
                            db.SaveChanges();
                        }
                        MessageBox.Show("Item Voided Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        ResetForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_MergerTickets_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var t = LisTview_TicketsList.Items.Cast<OrderMaster>().ToList();
                MergeTickets merge = new MergeTickets(t);
                merge.ShowDialog();
                RefreshTicketList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TextBlock_TicketNo.Text.Trim() == "")
                {
                    MessageBox.Show("Select a Ticket To Print!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (MessageBox.Show("You are about to print a ticket.\n\nAre you sure ?", "Message Box", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    using (var db = new PosDbContext())
                    {
                        var order = db.OrderMaster.Where(o => o.OrderNo == TextBlock_TicketNo.Text.Trim()).FirstOrDefault();
                        if (order is null)
                        {
                            MessageBox.Show("The Ticket Number is Invalid!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        if (order.IsInPreparation && order.IsKitchenServed)
                        {
                            order.IsPrinted = true;
                            db.SaveChanges();
                        }
                        else
                        {
                            MessageBox.Show("The Ticket Items are being Served. Kindly wait for the Kitchen to Complete!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                    MessageBox.Show("Completed. Ticket Printed!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshTicketList();
                    Button_CancelEditing_Click(new object(), new RoutedEventArgs());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
