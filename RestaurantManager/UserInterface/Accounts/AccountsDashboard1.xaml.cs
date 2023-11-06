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
using static System.Windows.Forms.CheckedListBox; 

namespace RestaurantManager.UserInterface.Accounts
{
    /// <summary>
    /// Interaction logic for Receivables.xaml
    /// </summary>
    public partial class AccountsDashboard1 : Page
    {
        public AccountsDashboard1()
        {
            InitializeComponent();
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            { 
               
                
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadPieChartData();
        }
        private void LoadPieChartData()
        {
             

        }
        public List<test> Data { get; private set; }

        public void SampleDataVM()
        {
            Data = new List<test>();
            Data.Add(new test() { name = "Felix", amount = 20 });
            Data.Add(new test() { name = "value1", amount = 30 });
            Data.Add(new test() { name = "value 2", amount = 50 });
            Data.Add(new test() { name = "value3", amount = 60 });

         }

        private void Button_Apply1_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void ClearTotals()
        {
            
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
                                          join t in db.TicketPaymentItem.AsNoTracking() on m.TicketNo equals t.ParentSourceRef
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
        { }

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
                var forsum = paylist;
                total = forsum.Sum(t => t.AmountPaid);
                cash = forsum.Where(k => k.Method == PosEnums.TicketPaymentMethods.Cash.ToString()).Sum(t => t.AmountPaid);
                mpesa = forsum.Where(k => k.Method == PosEnums.TicketPaymentMethods.Mpesa.ToString()).Sum(t => t.AmountPaid);
                cards = forsum.Where(k => k.Method == PosEnums.TicketPaymentMethods.Card.ToString()).Sum(t => t.AmountPaid);
                invoice = forsum.Where(k => k.Method == PosEnums.TicketPaymentMethods.Invoice.ToString()).Sum(t => t.AmountPaid);
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
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       
    }
    public class test
    {
        public string name { get; set; }
        public int amount { get; set; }
    }
}
