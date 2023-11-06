using DatabaseModels.CRM;
using DatabaseModels.OrderTicket;
using DatabaseModels.Payments;
using DatabaseModels.WorkPeriod;
using RestaurantManager.ApplicationFiles; 
using RestaurantManager.GlobalVariables;
using RestaurantManager.UserInterface.CustomersManagemnt;
using RestaurantManager.UserInterface.Security;
using RestaurantManager.UserInterface.Payments;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using winformdrawing = System.Drawing;
using DatabaseModels.Accounts;

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for CheckoutTicket.xaml
    /// </summary>
    public partial class CheckoutInvoice : Page
    { 
        public decimal AmountToPay = 0;
        public decimal AmountPaid = 0; 
        public decimal Current_balance = 0; 
        public ObservableCollection<TicketPaymentItem> payments = new ObservableCollection<TicketPaymentItem>();
        public CheckoutInvoice()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshInvoiceList();
                LoadBankCards();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
         
        /// <summary>
        /// tickets region
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        #region
        private void Button_RefreshTicketsList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshInvoiceList();
                MessageBox.Show("Done. Refresh Complete", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshInvoiceList()
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    LisTview_InvoiceList.ItemsSource = db.InvoicesMaster.AsNoTracking().Where(p =>  p.InvoiceStatus==PosEnums.InvoiceStatuses.Approved.ToString()).OrderBy(l=>l.InvoiceNo).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
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
                InvoicesMaster om = (InvoicesMaster)Lv.SelectedItem;
                LoadInvoiceDetails(om);
                Lv.SelectedItem = null; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadInvoiceDetails(InvoicesMaster order)
        {
            try
            {
                AmountPaid = 0;
                AmountToPay = 0; 
                Current_balance = 0;
                //get ticket items
                var db = new PosDbContext();
                decimal total = order.InvoiceAmount;
                TextBox_TotalAmount.Text = total.ToString("N2");
                AmountToPay = total;
                 TextBlock_TicketNo.Text = order.InvoiceNo;
                TextBlock_TicketDate.Text = order.InvoiceDate.ToString();
                GetPartialPayments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

       
        #endregion

        /// <summary>
        /// Number buttons region
        /// </summary>
        #region
        private void LoadBankCards()
        {
            try
            {
                var list = SharedVariables.ClientInfo().AcceptedCards.Split(',').Where(x => x != "").ToList();
                list.ForEach(k => k.Trim());
                Combobos_BanksList.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(3);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(4);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(5);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(6);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_7_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(7);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_8_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_9_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(9);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_0_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion


        /// <summary>
        /// Payments
        /// </summary>
        /// <param name="num"></param>
       #region
        private void NummberClicked(int num)
        {

            try
            {
                if (Label_SelectedModeofpayment.Content.ToString() == "")
                {
                    MessageBox.Show("Select Payment Method!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } 
                else
                {
                    Textbox_AmountToAdd.Text += num;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Textbox_AmountToAdd.Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //payment methods
        private void Btn_Cash_Click(object sender, RoutedEventArgs e)
        {
            PaymentMethodSelected(PosEnums.TicketPaymentMethods.Cash.ToString());
        }

        private void Btn_Mpesa_Click(object sender, RoutedEventArgs e)
        {
            PaymentMethodSelected(PosEnums.TicketPaymentMethods.Mpesa.ToString());
        }

        private void Btn_Cards_Click(object sender, RoutedEventArgs e)
        {
            PaymentMethodSelected(PosEnums.TicketPaymentMethods.Card.ToString());
        }
         
        private void Button_SelectInvoiceAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                InvoiceSalesBill inv = new InvoiceSalesBill();
               
                if (!(bool)inv.ShowDialog())
                {
                    return;
                }
                CustomerAccount billacc = (CustomerAccount)inv.Textbox_SelectedCustomerPhone.Tag;
                Textbox_SelectedAccount.Text = billacc.PersonAccNo + " - " + billacc.FullName;
                Textbox_SelectedAccount.Tag = billacc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_ResetInvoiceGroup_Click(object sender, RoutedEventArgs e)
        {
            Textbox_SelectedAccount.Text = "";
            Textbox_SelectedAccount.Tag = null;
        }

        private void Button_ResetPayparameters_Click(object sender, RoutedEventArgs e)
        {
            Textbox_SelectedAccount.Text = "";
            Label_SelectedModeofpayment.Content = "";
            Textbox_AmountToAdd.Text = "";
            Textbox_SelectedAccount.Tag = null;
        }

        private void PaymentMethodSelected(string method)
        {
            try
            {
                Label_SelectedModeofpayment.Content = method;
                if (method == PosEnums.TicketPaymentMethods.Cash.ToString())
                {
                     
                    Stackpanel_CardGroup.Visibility = Visibility.Collapsed;
                    Stackpanel_InvoiceGroup.Visibility = Visibility.Collapsed;

                }
                else if (method == PosEnums.TicketPaymentMethods.Card.ToString())
                { 
                    Stackpanel_CardGroup.Visibility = Visibility.Visible;
                    Stackpanel_InvoiceGroup.Visibility = Visibility.Collapsed;
                }
                else if (method == PosEnums.TicketPaymentMethods.Mpesa.ToString())
                { 
                    Stackpanel_CardGroup.Visibility = Visibility.Collapsed;
                    Stackpanel_InvoiceGroup.Visibility = Visibility.Collapsed;
                }
                else if (method == PosEnums.TicketPaymentMethods.Voucher.ToString())
                { 
                    Stackpanel_CardGroup.Visibility = Visibility.Collapsed;
                    Stackpanel_InvoiceGroup.Visibility = Visibility.Collapsed;
                    Label_SelectedModeofpayment.Content = "Enter Voucher Number";
                }
                else if (method == PosEnums.TicketPaymentMethods.Invoice.ToString())
                { 
                    Stackpanel_CardGroup.Visibility = Visibility.Collapsed;
                    Stackpanel_InvoiceGroup.Visibility = Visibility.Visible; 
                }
                else
                { 
                    Label_SelectedModeofpayment.Content = "Payment Not Selected";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void Button_AddPayment_Click1(object sender, RoutedEventArgs e)
        {
            //try
            //{
                 
            //    if (!int.TryParse(Textbox_AmountToAdd.Text.Trim(), out int amount))
            //    {
            //        MessageBox.Show("Enter Valid Amount!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
            //        return;
            //    }
            //    if (Label_SelectedModeofpayment.Content.ToString() == "" | Label_SelectedModeofpayment.Content.ToString() == "Discount")
            //    {
            //        MessageBox.Show("Select Payment Method!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
            //        return;
            //    }
            //    GetPartialPayments();
            //    var db = new PosDbContext(); 
            //    TicketPaymentItem pp = new TicketPaymentItem
            //    { 
            //        ParentSourceRef = TextBlock_TicketNo.Text,
            //        PaymentDate = SharedVariables.CurrentDate(),
            //        AmountPaid = amount, 
            //        PaymentBalance=0,
            //        PrimaryRefference="",
            //        AmountUsed=amount,
            //        SecondaryRefference = "None",
            //        PayForSource =PosEnums.PaymentForSources.InvoicePay.ToString(),
            //        Method = Label_SelectedModeofpayment.Content.ToString(),
            //        Workperiod = SharedVariables.CurrentOpenWorkPeriod().WorkperiodName,
            //        MasterTransNo = TextBlock_TicketNo.Text,
            //        ReceivingUsername = SharedVariables.CurrentUser.UserName,
            //    };
            //    if (pp.Method == PosEnums.TicketPaymentMethods.Card.ToString())
            //    {
            //        if (Combobos_BanksList.Text.Trim() == "")
            //        {
            //            MessageBox.Show("Select CardName!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
            //            return;
            //        } 
            //    } 
            //    db.TicketPaymentItem.Add(pp);
            //    db.SaveChanges();
            //    MessageBox.Show("Payment Saved!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information); 
            //    Label_SelectedModeofpayment.Content = "";
            //    Textbox_AmountToAdd.Text = "";
            //    GetPartialPayments();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void Button_AddPayment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string pmethod = Label_SelectedModeofpayment.Content.ToString();
                if (!int.TryParse(Textbox_AmountToAdd.Text.Trim(), out int amount))
                {
                    MessageBox.Show("Enter Valid Amount!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (pmethod == "")
                {
                    MessageBox.Show("Select Payment Method!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                GetPartialPayments();
                //validate payments
                //var requiredamount = AmountToPay - AmountPaid;
                decimal New_balance = amount + Current_balance;
                decimal AmountUsednow = 0;
                decimal Amountpaidnow = amount;

                if (Current_balance >= 0)
                {
                    MessageBox.Show("The payment is Enough Already. Do not add any Payment!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (pmethod == PosEnums.TicketPaymentMethods.Cash.ToString())
                {

                    Amountpaidnow = amount;
                    if (New_balance > 0)
                    {

                        if (New_balance > 1000)
                        {
                            MessageBox.Show("The Cash Balance Exceeds 1000!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        if (New_balance > CalculateCashtotal() + amount)
                        {
                            MessageBox.Show("The Cash balance is more than the cash paid!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        AmountUsednow = amount - New_balance;
                    }
                    else
                    {
                        AmountUsednow = amount;
                    }

                }
                else if (pmethod == PosEnums.TicketPaymentMethods.Mpesa.ToString() | pmethod == PosEnums.TicketPaymentMethods.Card.ToString() )
                {

                    if (New_balance > 0)
                    {
                        MessageBox.Show("The Amount Entered Exceeds the required amount!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    AmountUsednow = amount;
                    Amountpaidnow = amount;
                }
                else
                {
                    return;
                }
                var db = new PosDbContext();
                InvoicesMaster invm = null; 
                if (db.InvoicesMaster.AsNoTracking().FirstOrDefault(k => k.InvoiceNo == TextBlock_TicketNo.Text) != null)
                {
                    invm = db.InvoicesMaster.FirstOrDefault(k => k.InvoiceNo == TextBlock_TicketNo.Text);
                }
                else
                {
                    MessageBox.Show("The Invoice does not Exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } 
                TicketPaymentItem pp = new TicketPaymentItem
                {
                    ParentSourceRef = TextBlock_TicketNo.Text,
                    PaymentDate = SharedVariables.CurrentDate(),
                    AmountPaid = Amountpaidnow,
                    AmountUsed = AmountUsednow, 
                    PayForSource = PosEnums.PaymentForSources.InvoicePay.ToString(),
                    Method = Label_SelectedModeofpayment.Content.ToString(),
                    SecondaryRefference = "None",
                    Workperiod = SharedVariables.CurrentOpenWorkPeriod().WorkperiodName,
                    MasterTransNo = invm.InvoiceNo,
                    ReceivingUsername = SharedVariables.CurrentUser.UserName,
                };

                if (New_balance < 0)
                {
                    pp.PaymentBalance = 0;
                }
                else
                {
                    pp.PaymentBalance = New_balance;
                }
                if (pp.Method == PosEnums.TicketPaymentMethods.Card.ToString())
                {
                    if (Combobos_BanksList.Text.Trim() == "")
                    {
                        MessageBox.Show("Select CardName!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    pp.PrimaryRefference = Combobos_BanksList.Text;
                } 
                else
                {
                    pp.PrimaryRefference = "None";
                } 
                
                db.TicketPaymentItem.Add(pp);
                db.SaveChanges();
                MessageBox.Show("Payment Saved!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Button_ResetPayparameters_Click(null, new RoutedEventArgs());

                GetPartialPayments();
            }
        }

        private decimal CalculateCashtotal()
        {
            try
            {
                decimal casht = 0;
                var approved = payments.Where(k => k.IsInvoiceApproved && k.Method == PosEnums.TicketPaymentMethods.Cash.ToString());
                foreach (var x in approved)
                {
                    casht += x.AmountPaid;
                }
                return casht;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        private void Calculatetotal()
        {
            try
            {
                AmountPaid = 0;
                //if (IsInitializing)
                //{
                //    return;
                //}
                var approved = payments; 
                foreach (var x in approved)
                {
                    AmountPaid += x.AmountPaid;
                }
                
                Current_balance = AmountPaid - AmountToPay;
                Textblock_Amountpaid.Text = AmountPaid.ToString();
                Textblock_Balance.Text = Current_balance.ToString();
                if (AmountPaid  >= AmountToPay)
                {
                    Button_checkoutinvoice.IsEnabled = true;
                }
                else
                {
                    Button_checkoutinvoice.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
          
        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
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
                    if (Datagrid_Payments.SelectedItem == null)
                    {
                        return;
                    }
                    TicketPaymentItem selected_payment = (TicketPaymentItem)Datagrid_Payments.SelectedItem;
                    if (MessageBox.Show("Are you sure you wnat to Cancel the Payment?", "Message Box", MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        PromptAdminPin p = new PromptAdminPin();
                        if ((bool)p.ShowDialog())
                        {
                            WorkPeriod wp = SharedVariables.CurrentOpenWorkPeriod();
                            if (wp == null)
                            {
                                MessageBox.Show("No WorkPeriod Open!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }

                            //remove item and add voided item
                            using (var db = new PosDbContext())
                            {
                                TicketPaymentItem x = db.TicketPaymentItem.Where(a => a.PaymentGuid == selected_payment.PaymentGuid).FirstOrDefault();
                                db.TicketPaymentItem.Remove(x);
                                db.SaveChanges();
                                MessageBox.Show("Payment Canecelled Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            GetPartialPayments();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetPartialPayments()
        {
            try
            {
                payments.Clear();
                using (var db = new PosDbContext())
                {
                   
                    var items = db.TicketPaymentItem.AsNoTracking().Where(k => k.ParentSourceRef == TextBlock_TicketNo.Text).ToList();
                    foreach (var x in items)
                    {
                        if (x.Method == PosEnums.TicketPaymentMethods.Invoice.ToString())
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    payments = new ObservableCollection<TicketPaymentItem>(items);
                    Datagrid_Payments.ItemsSource = payments;
                    Calculatetotal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
 
        private void CloseInvoice()
        {
            try
            {
                TextBox_TotalAmount.Text = "0.00";
                TextBlock_TicketNo.Text = "";
                TextBlock_TicketDate.Text = ""; 
                Datagrid_Payments.ItemsSource = null;
                LisTview_InvoiceList.SelectedItem = null;
                payments.Clear();
                RefreshInvoiceList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                //DialogResult = false;
            }
        }

        private void Button_closeinvoice_Click(object sender, RoutedEventArgs e)
        {
            CloseInvoice();
        }

        private void Button_checkoutinvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetPartialPayments();
                List<String> errors = new List<string>();
                R_balance = 0;
                R_totalpaid = 0;
                R_TotalCharged = 0; 

                var db1 = new PosDbContext();
                DateTime Tdate = GlobalVariables.SharedVariables.CurrentDate();
                WorkPeriod wp = GlobalVariables.SharedVariables.CurrentOpenWorkPeriod();
                InvoicesMaster om = db1.InvoicesMaster.Where(h => h.InvoiceNo == TextBlock_TicketNo.Text).FirstOrDefault();
                if (om == null)
                {
                    MessageBox.Show("The Invoice does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (om.InvoiceStatus == PosEnums.InvoiceStatuses.Paid.ToString())
                {
                    MessageBox.Show("The Invoice has been SETTLED and PAID!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (AmountPaid < AmountToPay)
                {
                    MessageBox.Show("Failed! Insufficient Amount!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                else
                {
                    om.InvoiceStatus = PosEnums.InvoiceStatuses.Paid.ToString();
                    om.LastPaymentDate = SharedVariables.CurrentDate(); 
                    db1.SaveChanges();
                    PrintReceipt();
                    MessageBox.Show("Checkout Completed Successfuly!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseInvoice();                    
                    RefreshInvoiceList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                //DialogResult = false;
            }
        }

        /// <summary>
        /// printing begins here
        /// </summary>
        readonly string R_receiptno = "";
        decimal R_totalpaid = 0;
        decimal R_TotalCharged = 0;
        decimal R_balance = 0;
        
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
            List<InvoicePaymentItem> receiptitems = Datagrid_Payments.Items.Cast<InvoicePaymentItem>().ToList();

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
            graphics.DrawString("Invoice Payment Receipt", new winformdrawing.Font("Palatino Linotype", 12f, winformdrawing.FontStyle.Bold), new winformdrawing.SolidBrush(winformdrawing.Color.Black), (float)Center_X, (float)topoffset, format);
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
            foreach (InvoicePaymentItem ord_i in receiptitems)
            {
                if (ord_i.Method.ToString().Length <= 0x1f)
                {
                    graphics.DrawString(ord_i.Method.ToString(), new winformdrawing.Font("Arial", 10f), new winformdrawing.SolidBrush(winformdrawing.Color.Black), 10f, (float)topoffset);
                }
                else
                {
                    Array array = ord_i.Method.ToString().ToCharArray(0, 30);
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
                string[] textArray1 = new string[] { "                                                     ", "q1", " *  ", "p1", "   ", ord_i.AmountPaid.ToString() };
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

        #endregion


    }
}
