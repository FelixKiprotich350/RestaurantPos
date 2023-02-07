using RestaurantManager.ApplicationFiles;
using RestaurantManager.BusinessModels.OrderTicket;
using RestaurantManager.BusinessModels.WorkPeriod;
using RestaurantManager.GlobalVariables;
using RestaurantManager.UserInterface.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
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
using winformdrawing = System.Drawing;

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
                    items = db.OrderMaster.Where(p => p.OrderStatus == PosEnums.OrderTicketStatuses.Pending.ToString()&& p.OrderStatus!=PosEnums.OrderTicketStatuses.Completed.ToString()).ToList();
                }
                if (SharedVariables.CurrentUser.UserRole == PosEnums.UserAccountsRoles.Admin.ToString())
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
                GrossTotal = 0;
                TicketNo = "";
                //get ticket items
                using (var db = new PosDbContext())
                {
                    var a = db.OrderItem.Where(o => o.OrderID == order.OrderNo && o.IsItemVoided == false).ToList();
                    Datagrid_OrderItems.ItemsSource = a;
                }
                //find total
                decimal total = 0;
                var b = Datagrid_OrderItems.Items.Cast<OrderItem>().Where(m=>m.IsGiftItem==false).ToList();
                foreach (OrderItem x in b)
                {
                    total += (x.Price * x.Quantity)*((100-x.DiscPercent)/100);
                }
                GrossTotal = total;
                TicketNo = order.OrderNo;
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
                        db.OrderMaster.Where(o => o.OrderNo == TextBlock_TicketNo.Text.Trim()).First().OrderStatus = PosEnums.OrderTicketStatuses.Voided.ToString();
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
                            x.IsItemVoided = true; 
                            //OrderItemVoided ov = new OrderItemVoided
                            //{
                            //    ItemRowGuid = x.ItemRowGuid,
                            //    ParentProductItemGuid = x.ParentProductItemGuid,
                            //    ItemName = x.ItemName,
                            //    OrderID = x.OrderID,
                            //    VoidTime = GlobalVariables.SharedVariables.CurrentDate(),
                            //    ApprovedBy = p.ApprovingAdmin,
                            //    VoidReason = ei.Textbox_Description.Text,
                            //    WorkPeriod = wp.WorkperiodName
                            //};
                            //db.OrderItemVoided.Add(ov);
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
                        PrintReceipt();
                        order.IsPrinted = true;
                        db.SaveChanges();
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

        public void PrintReceipt()
        {
            try
            {
                Debug.WriteLine(PrinterSettings.InstalledPrinters.Count);
                PrintDocument document = new PrintDocument();
                document.PrintPage += new PrintPageEventHandler(this.ProvideContentForTicket);
                document.PrintController = new StandardPrintController();
                //document.PrinterSettings.PrintFileName = "Ticket.pdf";
                //document.PrinterSettings.PrintToFile = true;
                document.Print();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Failed to print Receipt", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        string TicketNo = "";
        decimal GrossTotal = 0;
        public void ProvideContentForTicket(object sender, PrintPageEventArgs e)
        {
           
            int Center_X = 150; 
            List<OrderItem> receiptitems = Datagrid_OrderItems.Items.Cast<OrderItem>().ToList();

            decimal taxamount = SharedVariables.ClientInfo().TaxPercentage * GrossTotal / 100;
            winformdrawing.Graphics graphics = e.Graphics;
            //begin receipt
            int topoffset = 20;
            winformdrawing.StringFormat format1 = new winformdrawing.StringFormat
            {
                LineAlignment = winformdrawing.StringAlignment.Center,
                Alignment = winformdrawing.StringAlignment.Center
            };
            winformdrawing.StringFormat format = format1;
            graphics.DrawString(SharedVariables.ClientInfo().ClientTitle.ToUpper(), new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 20;
            graphics.DrawString(SharedVariables.ClientInfo().PhysicalAddress, new winformdrawing.Font("Arial", 10f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 20; 
            graphics.DrawString("Order Ticket", new winformdrawing.Font("Palatino Linotype", 15f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), (float)Center_X, (float)topoffset, format);
            graphics.DrawString("____________", new winformdrawing.Font("Palatino Linotype", 15f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 15;
            graphics.DrawString("TicketNo:" + this.TicketNo, new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Regular), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 10f, (float)topoffset);
            topoffset += 20;
            graphics.DrawString("Date:" + SharedVariables.CurrentDate().ToShortDateString(), new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Regular), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 10f, (float)topoffset);
            graphics.DrawString("Counter : " + SharedVariables.LogInCounter, new winformdrawing.Font("Arial", 10f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 120f, (float)topoffset);
            topoffset += 20;
            graphics.DrawString("Time : " + SharedVariables.CurrentDate().ToShortTimeString(), new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Regular), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 10f, (float)topoffset);
            graphics.DrawString("Served By: " + SharedVariables.CurrentUser.UserFullName, new winformdrawing.Font("Arial", 10f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 120f, (float)topoffset);
            topoffset += 10;
            graphics.DrawString("----------------------------------------------------------------", new winformdrawing.Font("Arial", 10f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 10f, (float)topoffset);
            topoffset += 10;
            graphics.DrawString("Item                              Qty Price    Total", new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 10f, (float)topoffset);
            graphics.DrawString("______________________________________", new winformdrawing.Font("Arial", 10f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 10f, (float)topoffset);
            topoffset += 20; 
            foreach (OrderItem ord_i in receiptitems)
            { 
                if (ord_i.ItemName.ToString().Length <= 0x1f)
                {
                    graphics.DrawString(ord_i.ItemName.ToString(), new winformdrawing.Font("Arial", 10f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 10f, (float)topoffset);
                }
                else
                {
                    Array array = ord_i.ItemName.ToString().ToCharArray(0, 30);
                    string s = "";
                    int index = 0;
                    while (true)
                    {
                        string text1;
                        if (index >= array.Length)
                        {
                            graphics.DrawString(s, new winformdrawing.Font("Arial", 10f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 10f, (float)topoffset);
                            break;
                        }
                        object obj1 = array.GetValue(index);
                        if (obj1 != null)
                        {
                            text1 = obj1.ToString();
                        }
                        else
                        {
                            object local1 = obj1;
                            text1 = null;
                        }
                        s += text1;
                        index++;
                    }
                }
                topoffset += 15;
                string[] textArray1 = new string[] { "                                                     ", ord_i.Quantity.ToString().Trim(), " *  ", ((int)ord_i.Price).ToString(), "   ", ((int)ord_i.Total).ToString() };
                graphics.DrawString(string.Concat(textArray1), new winformdrawing.Font("Arial", 8f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 0f, (float)topoffset);
                topoffset += 15; 
            }
            graphics.DrawString("----------------------------------------------------------------", new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 10f, (float)topoffset);
            topoffset += 15;
            graphics.DrawString("TOTAL :", new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 50f, (float)topoffset);
            graphics.DrawString(GrossTotal.ToString("N2"), new winformdrawing.Font("Arial", 12f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 150f, (float)topoffset);
            topoffset += 20;  
            graphics.DrawString("----------------------------------------------------------------", new winformdrawing.Font("Arial", 10f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 10f, (float)topoffset);
            topoffset += 20;
            graphics.DrawString(SharedVariables.ClientInfo().ReceiptNote1, new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), (float)Center_X, (float)topoffset, format);
            
        }  
    }
}
