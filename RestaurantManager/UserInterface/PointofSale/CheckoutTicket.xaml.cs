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
using RestaurantManager.Printing;

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for CheckoutTicket.xaml
    /// </summary>
    public partial class CheckoutTicket : Page
    {
        public decimal AmountToPay = 0;
        public decimal AmountPaid = 0;
        public decimal Current_balance = 0;
        public decimal discount = 0;
        //public bool IsInitializing = true;
        public ObservableCollection<TicketPaymentItem> payments = new ObservableCollection<TicketPaymentItem>();
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
                SwitchView(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SwitchView(bool isTicketView)
        {
            if (isTicketView)
            {
                Grid_ticketsview.Visibility = Visibility.Visible;
                Grid_TicketLowerbuttonbar.Visibility = Visibility.Visible;
                Grid_paymentsview.Visibility = Visibility.Collapsed;
                Grid_PaymentLowerbuttonbar.Visibility = Visibility.Collapsed;
                Border_Ticketslist.Visibility = Visibility.Visible;

            }
            else
            {

                Button_ResetPayparameters_Click(null, new RoutedEventArgs());
                Grid_ticketsview.Visibility = Visibility.Collapsed;
                Grid_TicketLowerbuttonbar.Visibility = Visibility.Collapsed;
                Grid_paymentsview.Visibility = Visibility.Visible;
                Grid_PaymentLowerbuttonbar.Visibility = Visibility.Visible;
                Border_Ticketslist.Visibility = Visibility.Collapsed;
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
                    var data = db.OrderMaster.AsNoTracking().Where(p => p.OrderStatus == PosEnums.OrderTicketStatuses.Pending.ToString()).ToList().OrderByDescending(m => m.OrderDate);
                    foreach (var x in data)
                    {
                        var t = db.PersonalAccount.AsNoTracking().SingleOrDefault(k => k.AccountNo == x.CustomerRefference);
                        if (t != null)
                        {
                            x.WaiterName = t.FullName.Split(' ')[0];
                        }
                        else
                        {
                            x.WaiterName = "None";
                        }
                    }
                    LisTview_TicketsList.ItemsSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private CustomerAccount GetCustomer(string custno)
        {
            try
            {
                if (custno == null)
                {
                    return null;
                }
                var db = new PosDbContext();
                DatabaseModels.CRM.CustomerAccount cust = db.CustomerAccount.Where(x => x.PersonAccNo == custno).FirstOrDefault();
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
            catch (Exception ex)
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
                SwitchView(true);
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
                AmountPaid = 0;
                Current_balance = 0;
                R_receiptno = "";
                R_totalpaid = 0;
                R_TotalCharged = 0;
                R_balance = 0;
                TextBlock_TicketNo.Text = "";
                Datagrid_TicketItems.ItemsSource = null;
                //get ticket items
                var db = new PosDbContext();
                var a = db.OrderItem.AsNoTracking().Where(o => o.OrderID == order.OrderNo).ToList();
                Datagrid_TicketItems.ItemsSource = a;
                //find total
                decimal total = 0;
                var b = Datagrid_TicketItems.Items.Cast<OrderItem>().Where(m => m.IsGiftItem == false).ToList();
                foreach (OrderItem x in b)
                {
                    total += x.Price * x.Quantity * ((100 - x.DiscPercent) / 100);
                }
                TextBox_TotalAmount.Text = total.ToString("N2");
                AmountToPay = total;
                Textbox_TaxAmount.Text = (total * SharedVariables.ClientInfo().TaxPercentage / 100).ToString("N2");
                TextBlock_TicketNo.Text = order.OrderNo;
                TextBlock_TicketDate.Text = order.OrderDate.ToString();
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
                var db1 = new PosDbContext();
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
                if (om.OrderStatus == PosEnums.OrderTicketStatuses.Completed.ToString())
                {
                    MessageBox.Show("The Ticket has been Checked Out. Select another ticket to Check it out also!", "Message Box", MessageBoxButton.OK);
                    return;
                }

                SwitchView(false);
                LoadTicketCheckoutDetails(om);
                LoadBankCards();
                GetPartialPayments();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void LoadTicketCheckoutDetails(OrderMaster order)
        {
            try
            {
                AmountToPay = 0;
                AmountPaid = 0;
                Current_balance = 0;
                discount = 0;
                //get ticket items
                var db = new PosDbContext();
                var b = db.OrderItem.AsNoTracking().Where(o => o.OrderID == order.OrderNo && o.IsGiftItem == false).ToList();
                //find total
                decimal total = 0;
                foreach (OrderItem x in b)
                {
                    total += x.Price * x.Quantity * ((100 - x.DiscPercent) / 100);
                }
                TextBox_TotalAmount.Text = total.ToString("N2");
                AmountToPay = total;
                Textbox_TaxAmount.Text = (total * SharedVariables.ClientInfo().TaxPercentage / 100).ToString("N2");
                TextBlock_TicketNo.Text = order.OrderNo;
                TextBlock_TicketDate.Text = order.OrderDate.ToString();
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
                if (Label_SelectedModeofpayment.Content.ToString() == "Discount")
                {
                    if (!Textbox_DiscountOffered.IsReadOnly)
                    {
                        Textbox_DiscountOffered.Text += num;
                    }
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
                if (Label_SelectedModeofpayment.Content.ToString() != "Discount")
                {
                    Textbox_AmountToAdd.Clear();
                }
                else
                {
                    Textbox_DiscountOffered.Clear();
                }

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

        private void Btn_Voucher_Click(object sender, RoutedEventArgs e)
        {
            PaymentMethodSelected(PosEnums.TicketPaymentMethods.Voucher.ToString());
        }

        private void Btn_Invoice_Click(object sender, RoutedEventArgs e)
        {
            PaymentMethodSelected(PosEnums.TicketPaymentMethods.Invoice.ToString());
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
                if (pmethod == "" | pmethod == "Discount")
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
                else if (pmethod == PosEnums.TicketPaymentMethods.Mpesa.ToString() | pmethod == PosEnums.TicketPaymentMethods.Card.ToString() | pmethod == PosEnums.TicketPaymentMethods.Invoice.ToString())
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
                TicketPaymentMaster tpm = null;
                if (db.TicketPaymentMaster.AsNoTracking().FirstOrDefault(k => k.TicketNo == TextBlock_TicketNo.Text) != null)
                {
                    tpm = db.TicketPaymentMaster.FirstOrDefault(k => k.TicketNo == TextBlock_TicketNo.Text);
                    tpm.TotalAmountPaid = AmountPaid + AmountUsednow;
                    tpm.TicketBalanceReturned = New_balance;
                }
                else
                {
                    tpm = new TicketPaymentMaster
                    {
                        PaymentMasterGuid = Guid.NewGuid().ToString(),
                        TicketNo = TextBlock_TicketNo.Text,
                        TransNo = "C" + SharedVariables.CurrentDate().ToString("ddmmyy") + "-" + R.Next(0, 999).ToString(),
                        PosUser = SharedVariables.CurrentUser.UserName.ToString(),
                        TotalAmountPaid = AmountPaid + AmountUsednow,
                        TotalAmountCharged = AmountToPay,
                        TicketBalanceReturned = New_balance,
                        PaymentDate = SharedVariables.CurrentDate(),
                        WorkPeriod = SharedVariables.CurrentOpenWorkPeriod().WorkperiodName
                    };
                    db.TicketPaymentMaster.Add(tpm);
                }

                TicketPaymentItem pp = new TicketPaymentItem
                { 
                    ParentSourceRef = TextBlock_TicketNo.Text,
                    PaymentDate = SharedVariables.CurrentDate(),
                    AmountPaid = Amountpaidnow,
                    AmountUsed = AmountUsednow,
                    PayForSource=PosEnums.PaymentForSources.SalesBill.ToString(),
                    Method = Label_SelectedModeofpayment.Content.ToString(),
                    SecondaryRefference = "None", 
                    Workperiod = SharedVariables.CurrentOpenWorkPeriod().WorkperiodName,
                    MasterTransNo = tpm.TransNo,
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
                else if (pp.Method == PosEnums.TicketPaymentMethods.Invoice.ToString())
                {
                    CustomerAccount cust = (CustomerAccount)Textbox_SelectedAccount.Tag;
                    if (cust == null)
                    {
                        MessageBox.Show("The Customer Account Does Not Exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (cust.InvoiceLimit < AmountUsednow)
                    {
                        MessageBox.Show("The Amount invoiced Exceeds the Limit !", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    var newno = SharedVariables.GenerateInvoiceNumber();
                    if (newno < 1)
                    {
                        return;
                    }
                    //generate invoice
                    invm = new InvoicesMaster()
                    {
                        InvoiceGuid = Guid.NewGuid().ToString(),
                        InvoiceNo = "INV/S/" + newno,
                        InvoiceAmount = AmountUsednow,
                        InvoiceStatus = PosEnums.InvoiceStatuses.Issued.ToString(),
                        InvoiceSource = PosEnums.InvoiceSources.SalesBill.ToString(),
                        CancelReason = "None",
                        SystemUser = SharedVariables.CurrentUser.UserName,
                        CustomerAccNo = cust.PersonAccNo,
                        InvoiceDate = SharedVariables.CurrentDate(),
                        LastPaymentDate = SharedVariables.CurrentDate(),
                        PrimaryRefference = tpm.TransNo,
                        ExpectedPayDate = SharedVariables.CurrentDate().AddMonths(1),
                        Notes = "Invoiced due to Pending Bills",
                        Workperiod = SharedVariables.CurrentOpenWorkPeriod().WorkperiodName

                    };
                    pp.PrimaryRefference = invm.InvoiceNo;
                    db.InvoicesMaster.Add(invm);
                }

                else
                {
                    pp.PrimaryRefference = "None";
                }
                //to be revisited during vouchers
                //if (pp.Method == PosEnums.TicketPaymentMethods.Voucher.ToString())
                //{
                //    db.VoucherCard.Where(i => i.VoucherNumber == pp.PrimaryRefference).First().VoucherStatus = GlobalVariables.PosEnums.VoucherStatuses.Redeemed.ToString();
                //}

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

        private void Calculatetotal()
        {
            try
            {
                AmountPaid = 0;
                var approved = payments.Where(k => k.IsInvoiceApproved );
                foreach (var x in approved)
                {
                    AmountPaid += x.AmountPaid;
                }
                if (int.TryParse(Textbox_DiscountOffered.Text.Trim(), out int disc))
                {
                    discount = disc;
                }
                else
                {
                    discount = 0;
                }
                Current_balance = AmountPaid - (AmountToPay - discount);
                Textblock_Amountpaid.Text = AmountPaid.ToString();
                Textblock_Balance.Text = Current_balance.ToString();
                if (AmountPaid + discount >= AmountToPay)
                {
                    Button_CompletePayment.IsEnabled = true;
                }
                else
                {
                    Button_CompletePayment.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private decimal CalculateCashtotal()
        {
            try
            {
                decimal casht = 0;
                var approved = payments.Where(k => k.IsInvoiceApproved  && k.Method == PosEnums.TicketPaymentMethods.Cash.ToString());
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

        private void Combobox_SetDiscount_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Textbox_DiscountOffered.IsReadOnly = false;
                Textbox_DiscountOffered.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Combobox_SetDiscount_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Textbox_DiscountOffered.Text = "0";
                Textbox_DiscountOffered.IsReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Textbox_DiscountOffered_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Label_SelectedModeofpayment.Content = "Discount";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Textbox_DiscountOffered_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

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
                    if (MessageBox.Show("Are you sure you wnat to Cancel the Payment?", "Message Box", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        WorkPeriod wp = SharedVariables.CurrentOpenWorkPeriod();
                        if (wp == null)
                        {
                            MessageBox.Show("No WorkPeriod Open!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        PromptAdminPin p = new PromptAdminPin();
                        if ((bool)p.ShowDialog())
                        {
                            using (var db = new PosDbContext())
                            {
                                TicketPaymentItem x = db.TicketPaymentItem.Where(a => a.PaymentGuid == selected_payment.PaymentGuid).FirstOrDefault();

                                if (x.Method == PosEnums.TicketPaymentMethods.Invoice.ToString())
                                {
                                    var invpay = db.InvoicesMaster.FirstOrDefault(k => k.InvoiceNo == x.PrimaryRefference);
                                    if (invpay != null)
                                    {
                                        if (invpay.InvoiceStatus == PosEnums.InvoiceStatuses.Approved.ToString()| invpay.InvoiceStatus == PosEnums.InvoiceStatuses.Rejected.ToString())
                                        {
                                            MessageBox.Show("The INVOICE of the following code has been Approved. Kindly ask the administrator to cancel the Invoice before proceeding!\n" + x.PrimaryRefference, "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        else if (invpay.InvoiceStatus == PosEnums.InvoiceStatuses.Paid.ToString())
                                        {
                                            MessageBox.Show("The INVOICE of the following Number has been Paid. Kindly ask the administrator to Intervene!\nRefference Number : " + x.PrimaryRefference, "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        else
                                        {
                                            db.TicketPaymentItem.Remove(x);
                                            if(invpay.InvoiceStatus == PosEnums.InvoiceStatuses.Issued.ToString())
                                            {
                                                db.InvoicesMaster.Remove(invpay);
                                            } 
                                            
 
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("The INVOICE of the following code does not Exist!\n" + x.PrimaryRefference, "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                                        return;
                                    }
                                }
                                else
                                {
                                    db.TicketPaymentItem.Remove(x);

                                }


                                db.SaveChanges();
                                MessageBox.Show("Payment Cancelled Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                                GetPartialPayments();
                            }

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
                using (var db = new PosDbContext())
                {
                    var items = db.TicketPaymentItem.AsNoTracking().Where(k => k.ParentSourceRef == TextBlock_TicketNo.Text ).ToList();
                    foreach (var x in items)
                    {
                        if (x.Method == PosEnums.TicketPaymentMethods.Invoice.ToString())
                        {
                            var inv = db.InvoicesMaster.FirstOrDefault(k => k.InvoiceNo == x.PrimaryRefference);
                            if (inv != null & (inv.InvoiceStatus == PosEnums.InvoiceStatuses.Approved.ToString() | inv.InvoiceStatus == PosEnums.InvoiceStatuses.Paid.ToString()))
                            {
                                x.IsInvoiceApproved = true;
                            }
                            else
                            {
                                x.IsInvoiceApproved = false;
                            }
                        }
                        else
                        {
                            x.IsInvoiceApproved = true;
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

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            SwitchView(true);
        }

        private async void Button_CompletePayment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetPartialPayments();
                List<String> errors = new List<string>();
                //R_balance = 0;
                R_totalpaid = 0;
                R_TotalCharged = 0;
                var a = Datagrid_TicketItems.Items.Cast<OrderItem>().Where(m => m.IsGiftItem == false).ToList();
                var db1 = new PosDbContext();
                DateTime Tdate = SharedVariables.CurrentDate();
                WorkPeriod wp = db1.WorkPeriod.Where(h => h.WorkperiodStatus == PosEnums.WorkPeriodStatuses.Open.ToString()).FirstOrDefault();
                OrderMaster om = db1.OrderMaster.Where(h => h.OrderNo == TextBlock_TicketNo.Text).FirstOrDefault();
                TicketPaymentMaster FinalTPM = db1.TicketPaymentMaster.Where(h => h.TicketNo == TextBlock_TicketNo.Text).FirstOrDefault();
                if (AmountPaid < AmountToPay - discount)
                {
                    MessageBox.Show("Failed! Insufficient Amount!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                //customer service
                DatabaseModels.CRM.CustomerAccount cust = GetCustomer(om.CustomerRefference);
                CustomerPointsAccount ca = null;

                //saving data
                using (var db = new PosDbContext())
                {
                    using (DbContextTransaction tr = db.Database.BeginTransaction())
                    {
                        //foreach (var item in tpi)
                        //{
                        //    if (item.Method == PosEnums.TicketPaymentMethods.Voucher.ToString())
                        //    {
                        //        db.VoucherCard.Where(i => i.VoucherNumber == item.PrimaryRefference).First().VoucherStatus = GlobalVariables.PosEnums.VoucherStatuses.Redeemed.ToString();
                        //    }
                        //}
                        FinalTPM.TicketBalanceReturned = AmountPaid - AmountToPay;
                        db.SaveChanges();
                        var order = db.OrderMaster.FirstOrDefault(b => b.OrderNo == FinalTPM.TicketNo);
                        if (order != null)
                        {
                            order.OrderStatus = GlobalVariables.PosEnums.OrderTicketStatuses.Completed.ToString();
                            order.PaymentDate = GlobalVariables.SharedVariables.CurrentDate();
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
                        var solditems = db.OrderItem.Where(b => b.OrderID == FinalTPM.TicketNo);

                        foreach (var item_ in solditems)
                        {
                            db.StockFlowTransaction.Add(new DatabaseModels.Inventory.StockFlowTransaction()
                            {
                                ProductGuid = item_.ProductItemGuid,
                                ProductName = item_.ItemName,
                                Quantity = item_.Quantity,
                                SecondaryRefference = "Item Sold ",
                                TransactionDate = GlobalVariables.SharedVariables.CurrentDate(),
                                FlowDirection = "OUT",
                                StockFlowTrigger = PosEnums.StockFlowTriggerSource.Sold.ToString(),
                                Description = "Item Sold Out",
                                PrimaryRefference = order.OrderNo,
                                IsCancelled = false,
                            });

                        }
                        int transactions = db.SaveChanges();
                        if (transactions != solditems.Count())
                        {
                            tr.Rollback();
                            errors.Add("The Order Ticket does not exist!");

                        }
                        if (ca != null)
                        {
                            // db.CustomerPointsAccount.Add(ca);
                        }
                        db.SaveChanges();
                        if (errors.Count <= 0)
                        {
                            tr.Commit();
                            R_receiptno = FinalTPM.TransNo;
                            R_totalpaid = FinalTPM.TotalAmountPaid;
                            R_TotalCharged = FinalTPM.TotalAmountCharged;
                            R_balance = FinalTPM.TicketBalanceReturned;

                            MessageBox.Show("Checkout Completed Successfuly!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                            if (MessageBox.Show("Print Payment Receipt?", "Message Box", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                            {
                                await PrintReceipt();
                            }
                            Button_Discard_Click(new object(), new RoutedEventArgs());
                            SwitchView(true);
                            RefreshTicketList();
                        }
                        else
                        {
                            MessageBox.Show("The checkout Process was not completed!\n" + "ERROR : " + errors[0], "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
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
        string R_receiptno = "";
        decimal R_totalpaid = 0;
        decimal R_TotalCharged = 0;
        decimal R_balance = 0;

        public async Task PrintReceipt()
        {
            try
            {
                var print = new PrintPaymentReceipt
                {
                    R_receiptno = R_receiptno,
                    R_TotalCharged = R_TotalCharged,
                    R_totalpaid = R_totalpaid,
                    R_balance = R_balance,
                    Receiptitems = Datagrid_TicketItems.Items.Cast<OrderItem>().ToList()
                };
                print.PrepareReceiptInfoAsync();
                await print.PrintReceipt();
                //await Task.Run(() =>
                //{
                //    PrintDocument document = new PrintDocument();
                //    document.PrintPage += new PrintPageEventHandler(this.ProvideContentforsalesreceipt);
                //    document.PrintController = new StandardPrintController();
                //    //document.PrinterSettings.PrintFileName = "Receipt.pdf";
                //    //document.PrinterSettings.PrintToFile = true;
                //    document.Print();
                //});
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


        #endregion
    }
}
