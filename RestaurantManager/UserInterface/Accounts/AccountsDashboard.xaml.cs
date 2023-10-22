using DatabaseModels.Payments;
using DatabaseModels.Security;
using DatabaseModels.WorkPeriod;
using RestaurantManager.ApplicationFiles; 
using RestaurantManager.GlobalVariables;
using RestaurantManager.UserInterface.PosReports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace RestaurantManager.UserInterface.Accounts
{
    /// <summary>
    /// Interaction logic for Receivables.xaml
    /// </summary>
    public partial class AccountsDashboard : Page
    {
        public AccountsDashboard()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                 
                    using (var b = new PosDbContext())
                    {
                       Combobox_UserName.ItemsSource = b.PosUser.AsNoTracking().ToList();
                    }
                 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Apply1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearTotals();
                List<TicketPaymentMaster> tlist = new List<TicketPaymentMaster>();
                List<InvoicePaymentItem> invlist = new List<InvoicePaymentItem>();
                using (var b = new PosDbContext())
                {
                   
                    tlist = b.TicketPaymentMaster.AsNoTracking().ToList();
                    invlist = b.InvoicePaymentItem.AsNoTracking().ToList();

                }  
                if ((bool)Checkbox_ByUser.IsChecked)
                {
                    if (Combobox_UserName.SelectedItem != null)
                    {
                         tlist.RemoveAll(b => b.PosUser != ((PosUser)Combobox_UserName.SelectedItem).UserName);
                    }
                    else
                    {
                        MessageBox.Show("Select UserName!","Message Box",MessageBoxButton.OK);
                        return;
                    }
                }
                if ((bool)Checkbox_PaymentByDate.IsChecked)
                { }
                GetAccountsTotal(tlist,invlist);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearTotals()
        {
            Textbox_Cards.Text="0.00";
            Textbox_Mpesa.Text="0.00";
            TextBox_Vouchers.Text="0.00";
            Textbox_CashTotal.Text="0.00";
            Textbox_Totals.Text="0.00";
        }

        private void GetAccountsTotal(List<TicketPaymentMaster> tlist, List<InvoicePaymentItem> invlist)
        {
            try
            {
                decimal mpesa = 0;
                decimal cash = 0;
                decimal cards = 0; 
                decimal voucher = 0;
                decimal invoice = 0;
                decimal invoicecheked = 0;
                decimal cashbalance = 0;
                decimal unknown = 0;

                var db = new PosDbContext();
                if (tlist.Count <= 0)
                {
                    return;
                }
                var innerGroupJoinQuery = from m in tlist
                                          join t in db.TicketPaymentItem.AsNoTracking().Where(k=>k.IsVoided==false) on m.TicketNo equals t.ParentOrderNo
                                          select new { m, t };


                foreach (var x in innerGroupJoinQuery)
                {

                    if (x.t.Method == PosEnums.TicketPaymentMethods.Cash.ToString())
                    {
                        cash += x.t.AmountPaid;
                    }
                    else if (x.t.Method == PosEnums.TicketPaymentMethods.Mpesa.ToString())
                    {
                        mpesa += x.t.AmountPaid;
                    }
                    else if (x.t.Method.ToLower().Contains(PosEnums.TicketPaymentMethods.Card.ToString().ToLower()))
                    {
                        cards += x.t.AmountPaid;
                    }
                    else if (x.t.Method == PosEnums.TicketPaymentMethods.Voucher.ToString())
                    {
                        voucher += x.t.AmountPaid;
                    }
                    else if (x.t.Method == PosEnums.TicketPaymentMethods.Invoice.ToString())
                    {
                        invoice += x.t.AmountPaid;
                    }
                    else
                    {
                        unknown += x.t.AmountPaid;
                    }
                }
                foreach (var y in tlist)
                {
                    cashbalance += y.TicketBalanceReturned;
                }
                foreach (var m in invlist)
                {
                    invoicecheked += m.AmountPaid;
                }
                Textbox_CashTotal.Text = (cash-cashbalance).ToString();
                Textbox_Mpesa.Text = mpesa.ToString();
                TextBox_Vouchers.Text = voucher.ToString();
                Textbox_Cards.Text = cards.ToString();
                TextBox_Invoice.Text = invoice.ToString();
                TextBox_CheckedInvoice.Text = invoicecheked.ToString();
                Textbox_Totals.Text = (cash +invoice+ mpesa + cards-cashbalance).ToString();
                if (unknown > 0)
                {
                    MessageBox.Show("The following amount cannot be accounted for!\n" + unknown.ToString("N2"), "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearTotals();
                DateTime? startdate = null;
                DateTime? enddate = null;
                WorkPeriod wperiod = null;
                if (Button_SelectWorkPeriod.Content.ToString() == "All")
                {
                    wperiod = (WorkPeriod)Button_SelectWorkPeriod.Tag;
                }
                else
                {
                    wperiod = (WorkPeriod)Button_SelectWorkPeriod.Tag;
                }
                if ((bool)Checkbox_PaymentByDate.IsChecked)
                {
                    if (Datepicker_From.SelectedDate==null| Datepicker_To.SelectedDate==null)
                    {
                        MessageBox.Show("Select Start Date & End Date!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    startdate = Datepicker_From.SelectedDate;
                    enddate = Datepicker_To.SelectedDate;

                } 
                LoadPayments(wperiod, startdate, enddate);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void LoadPayments(WorkPeriod wp, DateTime? startdate, DateTime? enddate)
        {
            try
            {
                decimal total = 0;
                decimal cash = 0;
                decimal mpesa = 0;
                decimal cards = 0;
                decimal invoice = 0;
                decimal change = 0;
                ObservableCollection<TicketPaymentItem> payments = new ObservableCollection<TicketPaymentItem>();
                ObservableCollection<InvoicePaymentItem> invpayments = new ObservableCollection<InvoicePaymentItem>();
                var db = new PosDbContext();
                var paylist = db.TicketPaymentItem.AsNoTracking().ToList();
                var invlist = db.InvoicePaymentItem.AsNoTracking().ToList();

                if (wp != null)
                {
                    paylist.RemoveAll(w => w.Workperiod != wp.WorkperiodName);
                }
                if (startdate != null)
                {
                    paylist.RemoveAll(w => w.PaymentDate < startdate);

                }
                if (enddate != null)
                {
                    paylist.RemoveAll(w => w.PaymentDate > enddate);
                }
                payments = new ObservableCollection<TicketPaymentItem>(paylist);
                var forsum = paylist.Where(k => k.IsVoided == false);
                total = forsum.Sum(t => t.AmountPaid);
                cash = forsum.Where(k => k.Method == PosEnums.TicketPaymentMethods.Cash.ToString()).Sum(t => t.AmountPaid);
                mpesa = forsum.Where(k => k.Method == PosEnums.TicketPaymentMethods.Mpesa.ToString()).Sum(t => t.AmountPaid);
                cards = forsum.Where(k => k.Method == PosEnums.TicketPaymentMethods.Card.ToString()).Sum(t => t.AmountPaid);
                invoice = forsum.Where(k => k.Method == PosEnums.TicketPaymentMethods.Invoice.ToString()).Sum(t => t.AmountPaid);
                Textbox_CashTotal.Text = (cash - change).ToString();
                Textbox_Mpesa.Text = mpesa.ToString(); 
                Textbox_Cards.Text = cards.ToString();
                TextBox_Invoice.Text = invoice.ToString(); 
                Textbox_Totals.Text = (cash + invoice + mpesa + cards - change).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RadioButton_SingleDay_Checked(object sender, RoutedEventArgs e)
        {
            try
            {  
                Datepicker_From.SelectedDate = null;
                Datepicker_To.SelectedDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Radiobutton_Period_Checked(object sender, RoutedEventArgs e)
        {
            try
            { 
                Datepicker_From.SelectedDate = null;
                Datepicker_To.SelectedDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
 
        private void Checkbox_PaymentByDate_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            { 
                Datepicker_From.SelectedDate = null;
                Datepicker_To.SelectedDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
 
        private void Checkbox_ByUser_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Combobox_UserName.SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_SelectWorkPeriod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectWorkPeriod w = new SelectWorkPeriod();
                if ((bool)w.ShowDialog())
                {
                    if (w.SelectedWorkperiod != null)
                    {
                        Button_SelectWorkPeriod.Tag = w.SelectedWorkperiod;
                        Button_SelectWorkPeriod.Content = w.SelectedWorkperiod.WorkperiodName;
                    }
                    else
                    {
                        Button_SelectWorkPeriod.Tag = null;
                        Button_SelectWorkPeriod.Content = "All";
                    }
                }
                else
                {
                    Button_SelectWorkPeriod.Tag = null;
                    Button_SelectWorkPeriod.Content = "All";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
