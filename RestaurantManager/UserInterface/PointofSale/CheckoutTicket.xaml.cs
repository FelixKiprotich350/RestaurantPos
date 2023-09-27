using DatabaseModels.CustomersManagement;
using DatabaseModels.OrderTicket;
using DatabaseModels.Payments;
using DatabaseModels.WorkPeriod;
using RestaurantManager.ApplicationFiles; 
using RestaurantManager.GlobalVariables;
using RestaurantManager.UserInterface.CustomersManagemnt;
using RestaurantManager.UserInterface.TicketPayments;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Drawing.Printing;
using System.IO;
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
using winformdrawing = System.Drawing;

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for CheckoutTicket.xaml
    /// </summary>
    public partial class CheckoutTicket : Page
    {
        readonly Random R = new Random();
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
                MessageBox.Show("Done. Refresh Complete", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    LisTview_TicketsList.ItemsSource = db.OrderMaster.AsNoTracking().Where(p =>  p.OrderStatus==PosEnums.OrderTicketStatuses.Pending.ToString()).ToList().OrderByDescending(m => m.OrderDate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private Customer GetCustomer(string custno)
        {
            try
            {
                if (custno == null)
                {
                    return null;
                }
                var db = new PosDbContext();
                Customer cust = db.Customer.Where(x => x.PhoneNumber == custno).FirstOrDefault();
                if (cust != null)
                {
                   return cust;
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

        public int GetLoyaltyPoints(int PurchaseAmount)
        {
            int points;
            try
            {
                //using (var db=new PosDbContext())
                //{

                //}
                points = PurchaseAmount / 100;
            }
            catch(Exception ex)
            {
                points = -1;
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return points;
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
                    var a = db.OrderItem.Where(o => o.OrderID == order.OrderNo && o.IsItemVoided == false).ToList();
                    Datagrid_TicketItems.ItemsSource = a;
                }
                //find total
                decimal total = 0;
                var b = Datagrid_TicketItems.Items.Cast<OrderItem>().Where(m => m.IsGiftItem == false).ToList();
                foreach (OrderItem x in b)
                {
                    total += (x.Price * x.Quantity) * ((100 - x.DiscPercent) / 100);
                }
                TextBox_TotalAmount.Text = total.ToString("N2");
                Textbox_TaxAmount.Text = (total * SharedVariables.ClientInfo().TaxPercentage / 100).ToString("N2");
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


        private void Button_CheckoutTicket_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                List<String> errors = new List<string>();
                R_balance = 0;
                R_totalpaid = 0;
                R_TotalCharged = 0;
                var db1 = new PosDbContext();
                DateTime Tdate = GlobalVariables.SharedVariables.CurrentDate();
                WorkPeriod wp = db1.WorkPeriod.Where(h => h.WorkperiodStatus == PosEnums.WorkPeriodStatuses.Open.ToString()).FirstOrDefault();
                OrderMaster om = db1.OrderMaster.Where(h => h.OrderNo == TextBlock_TicketNo.Text).FirstOrDefault();
                if (wp == null)
                {
                    MessageBox.Show("The work Period is Closed!", "Message Box", MessageBoxButton.OK);
                    return;
                }
                if (om == null)
                {
                    MessageBox.Show("The Ticket does not Exist. Select a ticket to Check it out!", "Message Box", MessageBoxButton.OK);
                    return;
                }
                decimal total = 0;
                var a = Datagrid_TicketItems.Items.Cast<OrderItem>().Where(m => m.IsGiftItem == false).ToList();
                foreach (OrderItem x in a)
                {
                    total += x.Price * x.Quantity * ((100 - x.DiscPercent) / 100);
                } 
                //get payments
                List<PaymentMethod> pm = new List<PaymentMethod>();
                PaymentsWindow pw = new PaymentsWindow
                {
                    AmountToPay = total
                    
                };
                if ((bool)pw.ShowDialog() == true)
                {
                    pm = pw.payments.ToList();
                    R_balance = pw.balance;
                    R_totalpaid = pw.AmountPaid;
                    R_TotalCharged = pw.AmountToPay;
                }
                if (pm.Count <= 0)
                {
                    MessageBox.Show("Payment Cancelled!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);

                    return;
                }
                TicketPaymentMaster tpm = new TicketPaymentMaster
                {
                    PaymentMasterGuid = Guid.NewGuid().ToString(),
                    TicketNo = TextBlock_TicketNo.Text,
                    TransNo = "C" + SharedVariables.CurrentDate().ToString("ddmmyy") + "-" + R.Next(0, 999).ToString(),
                    PosUser = SharedVariables.CurrentUser.UserName.ToString(),
                    TotalAmountPaid = pm.Sum(b => b.Amount),
                    TotalAmountCharged = total,
                    TicketBalanceReturned = pw.balance,
                    PaymentDate = Tdate,
                    WorkPeriod = wp.WorkperiodName
                };
                List<TicketPaymentItem> tpi = new List<TicketPaymentItem>();
                foreach (PaymentMethod p in pm)
                {
                    TicketPaymentItem t = new TicketPaymentItem
                    {
                        PaymentGuid = Guid.NewGuid().ToString(),
                        ParentOrderNo = TextBlock_TicketNo.Text,
                        ParentTransNo = tpm.TransNo,
                        PrimaryRefference = p.PrimaryRefference,
                        Method = p.PaymentMethodName,
                        AmountPaid = p.Amount,
                        PaymentDate = Tdate,
                        ReceivingUsername = SharedVariables.CurrentUser.UserName,
                        SecondaryRefference = p.SecondaryRefference
                    };
                    tpi.Add(t);

                }

                //customer service
                Customer cust = GetCustomer(om.CustomerRefference);
                CustomerPointsAccount ca = null;
                if (cust != null)
                {
                    ca = new CustomerPointsAccount
                    {
                        AccActionGuid = Guid.NewGuid().ToString(),
                        TransactionNo = SharedVariables.CurrentDate().ToString("yyMMddHHmmssffff"),
                        CustomerPhoneNo = cust.PhoneNumber,
                        ApprovedBy = SharedVariables.CurrentUser.UserName,
                        ActionDate = SharedVariables.CurrentDate(),
                        Credit = 0,
                        TransactionType = PosEnums.CustomerAccountActionType.PointsAward.ToString()
                    };
                    int points = GetLoyaltyPoints((int)total);
                    if (points > 0)
                    {
                        ca.Debit = points;
                    }
                    else
                    {

                        ca = null;
                    }
                }

                //saving data
                using (var db = new PosDbContext())
                {
                    using (DbContextTransaction tr = db.Database.BeginTransaction())
                    {
                        foreach (var item in tpi)
                        {
                            if (item.Method == PosEnums.TicketPaymentMethods.Voucher.ToString())
                            {
                                db.VoucherCard.Where(i => i.VoucherNumber == item.PrimaryRefference).First().VoucherStatus = GlobalVariables.PosEnums.VoucherStatuses.Redeemed.ToString();
                            }
                        }
                        db.SaveChanges();
                        var result = db.OrderMaster.FirstOrDefault(b => b.OrderNo == tpm.TicketNo);
                        if (result != null)
                        {
                            result.OrderStatus = GlobalVariables.PosEnums.OrderTicketStatuses.Completed.ToString();
                            result.PaymentDate = GlobalVariables.SharedVariables.CurrentDate();
                            int x = db.SaveChanges();
                            if (x != 1)
                            {
                                tr.Rollback(); 
                                errors.Add("Failed to update Order Status");
                            }
                        }
                        else
                        {
                            tr.Rollback(); 
                            errors.Add("The Order Ticket does not exist!");
                        }
                        var solditems = db.OrderItem.Where(b => b.OrderID == tpm.TicketNo);

                        foreach (var item_ in solditems)
                        {
                            db.StockFlowTransaction.Add(new DatabaseModels.Warehouse.StockFlowTransaction()
                            {
                                ProductGuid=item_.ProductItemGuid,
                                ProductName=item_.ItemName,
                                Quantity=item_.Quantity,
                                TransactionDate=GlobalVariables.SharedVariables.CurrentDate(),
                                FlowDirection="OUT",
                                InTransactionCode="N/A",
                                OutTransactionCode=result.OrderNo,
                                IsCancelled=false,
                            });
                            
                        }
                        int transactions = db.SaveChanges();
                        if (transactions != solditems.Count())
                        {
                            tr.Rollback();
                            errors.Add("The Order Ticket does not exist!");

                        }
                        db.TicketPaymentItem.AddRange(tpi);
                        db.SaveChanges();
                        db.TicketPaymentMaster.Add(tpm);
                        if (ca != null)
                        {
                            db.CustomerPointsAccount.Add(ca);
                        }
                        db.SaveChanges();
                        if (errors.Count<=0)
                        {
                            tr.Commit();
                            R_receiptno = tpm.TransNo;
                            PrintReceipt();
                            MessageBox.Show("Successfuly Saved", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                            Button_Discard_Click(new object(), new RoutedEventArgs());
                            RefreshTicketList();
                        }
                        else
                        {
                            MessageBox.Show("The checkout Process was not completed!\n"+"ERROR : "+errors[0], "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        string R_receiptno = "";
        decimal R_totalpaid = 0;
        decimal R_TotalCharged = 0;
        decimal R_balance = 0;
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
                PrintDocument document = new PrintDocument();
                document.PrintPage += new PrintPageEventHandler(this.ProvideContentforsalesreceipt);
                document.PrintController = new StandardPrintController();
                //document.PrinterSettings.PrintFileName = "Receipt.pdf";
                //document.PrinterSettings.PrintToFile = true;
                document.Print();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Failed to print Receipt", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ProvideContentforsalesreceipt(object sender, PrintPageEventArgs e)
        {
            int Center_X = 150;
            List<OrderItem> receiptitems = Datagrid_TicketItems.Items.Cast<OrderItem>().ToList();

            winformdrawing.Graphics graphics = e.Graphics;
            //begin receipt
            int topoffset = 10;
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
            graphics.DrawString(SharedVariables.ClientInfo().Phone, new winformdrawing.Font("Arial", 10f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 20;
            graphics.DrawString(SharedVariables.ClientInfo().Email, new winformdrawing.Font("Arial", 10f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 20;
            graphics.DrawString(SharedVariables.ClientInfo().ClientKRAPIN.ToUpper(), new winformdrawing.Font("Arial", 12f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 20;
            graphics.DrawString("Payment Receipt", new winformdrawing.Font("Palatino Linotype", 12f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), (float)Center_X, (float)topoffset, format);
            graphics.DrawString("____________", new winformdrawing.Font("Palatino Linotype", 15f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 15;
            graphics.DrawString("Receipt No:" + R_receiptno, new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Regular), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 10f, (float)topoffset);
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
                string[] textArray1 = new string[] { "                                                     ", ord_i.Quantity.ToString().Trim(), " *  ", ord_i.Price.ToString(), "   ", decimal.Parse(ord_i.Total.ToString()).ToString("N2") };
                graphics.DrawString(string.Concat(textArray1), new winformdrawing.Font("Arial", 8f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 0f, (float)topoffset);
                topoffset += 15;
            }
            graphics.DrawString("----------------------------------------------------------------", new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 10f, (float)topoffset);
            topoffset += 15;
            graphics.DrawString("TOTAL :", new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 50f, (float)topoffset);
            graphics.DrawString(R_TotalCharged.ToString("N2"), new winformdrawing.Font("Arial", 12f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 150f, (float)topoffset);
            topoffset += 20;
            graphics.DrawString("Amount Paid :", new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 50f, (float)topoffset);
            graphics.DrawString(R_totalpaid.ToString("N2"), new winformdrawing.Font("Arial", 12f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 150f, (float)topoffset);
            topoffset += 20;
            graphics.DrawString("Balance", new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 50f, (float)topoffset);
            graphics.DrawString(R_balance.ToString("N2"), new winformdrawing.Font("Arial", 12f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 150f, (float)topoffset);
            topoffset += 10;
            graphics.DrawString("----------------------------------------------------------------", new winformdrawing.Font("Arial", 10f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 10f, (float)topoffset);
            topoffset += 15;
            graphics.DrawString("Tax%        TaxAmt", new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Underline), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 70f, (float)topoffset);
            topoffset += 15;
            graphics.DrawString(SharedVariables.ClientInfo().TaxPercentage.ToString(), new winformdrawing.Font("Arial", 10f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 80f, (float)topoffset);
            graphics.DrawString((SharedVariables.ClientInfo().TaxPercentage / 100 * R_TotalCharged).ToString("N2"), new winformdrawing.Font("Arial", 10f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 135f, (float)topoffset);
            topoffset += 10;
            graphics.DrawString("----------------------------------------------------------------", new winformdrawing.Font("Arial", 10f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 10f, (float)topoffset);
            topoffset += 20;
            graphics.DrawString(SharedVariables.ClientInfo().ReceiptNote1, new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 20;
            graphics.DrawString(SharedVariables.ClientInfo().ReceiptNote1, new winformdrawing.Font("Arial", 10f, winformdrawing.FontStyle.Italic), new winformdrawing.SolidBrush(winformdrawing.Color.Black), (float)Center_X, (float)topoffset, format);
            topoffset += 15;
            graphics.DrawString(SharedVariables.ClientInfo().ReceiptNote1, new winformdrawing.Font("Arial", 8f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), (float)Center_X, (float)topoffset, format);
        }
    }
}
