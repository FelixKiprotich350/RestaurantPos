using DatabaseModels.Payments;
using DatabaseModels.WorkPeriod;
using RestaurantManager.GlobalVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantManager.UserInterface.PosReports
{
    /// <summary>
    /// Interaction logic for PaymentsReport.xaml
    /// </summary>
    public partial class PaymentsReport : Page
    {
        readonly List<dynamic> MainList = new List<dynamic>(); 
        public PaymentsReport()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int item = TabControl_PaymentsTabControl.SelectedIndex;
                ClearLists();
                DateTime? startdate = null;
                DateTime? enddate = null;
                WorkPeriod wperiod = null;
                if (Button_SelectWorkPeriod.Content.ToString() == "All")
                {
                    if (Button_SelectWorkPeriod.Tag != null)
                    {
                        if (Button_SelectWorkPeriod.Tag.GetType() == typeof(WorkPeriod))
                        {
                            wperiod = (WorkPeriod)Button_SelectWorkPeriod.Tag;
                        }
                    }

                }
                if (Datepicker_Startdate.SelectedDate != null)
                {
                    startdate = Datepicker_Startdate.SelectedDate;
                }
                if (Datepicker_Enddate.SelectedDate != null)
                {
                    enddate = Datepicker_Enddate.SelectedDate;
                }
                LoadItems(wperiod, startdate, enddate);
                TabControl_PaymentsTabControl.SelectedIndex = -1;
                TabControl_PaymentsTabControl.SelectedIndex = item;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearLists()
        {
            try
            {
                MainList.Clear();  
                //grids
                Datagrid_Payments.ItemsSource = null;
                Datagrid_Payments.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadItems(WorkPeriod wp, DateTime? startdate, DateTime? enddate)
        {
            try
            {
                var db = new PosDbContext();
                //load Items 
                //ListofPayItems = db.TicketPaymentItem.AsNoTracking().ToList();
                //ListofPayMaster = db.TicketPaymentMaster.AsNoTracking().ToList();

                var leftOuterJoin = from pitem in db.TicketPaymentItem.AsNoTracking()
                                    join m in db.TicketPaymentMaster.AsNoTracking() on pitem.ParentTransNo equals m.TransNo into masterlist
                                    from master in masterlist.DefaultIfEmpty()
                                    select new
                                    {
                                        tpm = master,
                                        tpi = pitem
                                        //EmployeeCode = e.ParentProductItemGuid,
                                        //EmployeeName = e.ItemName,
                                        //DepartmentName = department.OrderNo
                                    };
                var t = leftOuterJoin.ToList();
                if (wp != null)
                {
                    t = t.Where(a => a.tpm.WorkPeriod == wp.WorkperiodName).ToList();
                }
                if (startdate != null && enddate != null)
                {
                    t = t.Where(a => a.tpm.PaymentDate >= startdate && a.tpm.PaymentDate <= enddate).ToList();
                }
                MainList.AddRange(t);
                MessageBox.Show("Loading Done!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button_SelectWorkPeriod.Content = "All";
                Datepicker_Startdate.SelectedDate = null;
                Datepicker_Enddate.SelectedDate = null;
                MainList.Clear();
  
                //List<dynamic> list = new List<dynamic>();
                //int c = 9000000;
                //DateTime a = DateTime.Now;
                //while (c>0)
                //{
                //    list.Add(new { TransNo = "S" , AmountCharged = c, AmountPaid = c, Balance = c });
                //    c--;
                //}
                //DateTime b = DateTime.Now;
                //double d = (b - a).TotalMilliseconds;
                //MessageBox.Show("Done",d.ToString());
                //Datagrid_Payments.ItemsSource =list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_SelectWorkPeriod_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TabControl_PaymentsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int position = TabControl_PaymentsTabControl.SelectedIndex;
                if (position == 0)
                {

                    List<dynamic> percheckout = new List<dynamic>();
                    var master = MainList.Select(k => (TicketPaymentMaster)k.tpm).Distinct(new PaymentMaster_Comparer()).ToList();
                    var items = MainList.Select(k => (TicketPaymentItem)k.tpi).ToList();
                    if (master.Count > 0)
                    {
                        decimal cash = 0;
                        decimal mpesa = 0;
                        decimal voucher = 0;
                        decimal cards = 0;
                        foreach (var x in master)
                        {
                            cash = items.Where(k => k.ParentTransNo == x.TransNo && k.Method == PosEnums.TicketPaymentMethods.Cash.ToString()).Sum(j => j.AmountPaid);
                            mpesa = items.Where(k => k.ParentTransNo == x.TransNo && k.Method == PosEnums.TicketPaymentMethods.Mpesa.ToString()).Sum(j => j.AmountPaid);
                            voucher = items.Where(k => k.ParentTransNo == x.TransNo && k.Method == PosEnums.TicketPaymentMethods.Voucher.ToString()).Sum(j => j.AmountPaid);
                            cards = items.Where(k => k.ParentTransNo == x.TransNo && k.Method.Contains(PosEnums.TicketPaymentMethods.Card.ToString())).Sum(j => j.AmountPaid);
                            percheckout.Add(new { x.TransNo, AmountCharged = x.TotalAmountCharged, AmountPaid = x.TotalAmountPaid, Balance = x.TicketBalanceReturned, Cash = cash, Mpesa = mpesa, Cards = cards, Voucher = voucher });
                        }
                        decimal totals = 0;
                        int count = 0;
                        foreach (var x in master)
                        {
                            totals += x.TotalAmountCharged;
                            count++;
                        }
                        Label_Transactions_Count.Content = count.ToString();
                        Label_Transactions_Total.Content = totals.ToString();
                        Datagrid_Payments.ItemsSource = percheckout;
                    }
                }
                else if (position == 1)
                {

                    List<dynamic> cardpayments = new List<dynamic>();
                    List<dynamic> cardpaymentsdistincttotals = new List<dynamic>();
                    var master = MainList.Select(k => (TicketPaymentMaster)k.tpm).Distinct(new PaymentMaster_Comparer()).ToList();
                    var items = MainList.Select(k => (TicketPaymentItem)k.tpi).ToList();
                    var cardpaymentsonly = MainList.Select(k => (TicketPaymentItem)k.tpi).Where(k => k.Method.ToLower().Contains(PosEnums.TicketPaymentMethods.Card.ToString().ToLower())).ToList();
                    var cardpaymentsdistinctt = cardpaymentsonly.Distinct(new PaymentItem_method_Comparer());
                    if (master.Count > 0)
                    {
                        decimal cards = 0;
                        foreach (var x in master)
                        {
                            var a = cardpaymentsonly.Where(k => k.ParentTransNo == x.TransNo).ToList();
                            cards += a.Sum(j => j.AmountPaid);
                            foreach (var y in a)
                            {
                                cardpayments.Add(new { Tpi = y, Tpm = x });
                            }

                        }
                        decimal totaldistinct = 0;
                        foreach (var x in cardpaymentsdistinctt)
                        {
                            decimal t = cardpaymentsonly.Where(k => k.Method == x.Method).Sum(j => j.AmountPaid);
                            totaldistinct += t;
                            cardpaymentsdistincttotals.Add(new { CardName = x.Method, Total = t });
                        }
                        Label_Bankcards_Total.Content = totaldistinct.ToString();
                        Datagrid_BankCardsAll.ItemsSource = cardpayments;
                        Datagrid_BankCardsWise.ItemsSource = cardpaymentsdistincttotals;
                    }
                }
                else if (position == 2)
                {

                    List<dynamic> mpesapayments = new List<dynamic>();
                    var master = MainList.Select(k => (TicketPaymentMaster)k.tpm).Distinct(new PaymentMaster_Comparer()).ToList();
                    var items = MainList.Select(k => (TicketPaymentItem)k.tpi).ToList();
                    var mpesapaymentsonly = MainList.Select(k => (TicketPaymentItem)k.tpi).Where(k => k.Method.ToLower() == PosEnums.TicketPaymentMethods.Mpesa.ToString().ToLower()).ToList();
                    if (master.Count > 0)
                    {
                        decimal total = 0;
                        int count = 0;
                        foreach (var x in master)
                        {
                            var a = mpesapaymentsonly.Where(k => k.ParentTransNo == x.TransNo).ToList();
                            total += a.Sum(j => j.AmountPaid);
                            count += a.Count;
                            foreach (var y in a)
                            {
                                mpesapayments.Add(new { Tpi = y, Tpm = x });
                            }

                        }
                        Label_MpesaCount_Count.Content = count.ToString();
                        Label_Mpesa_Total.Content = total.ToString();
                        Datagrid_MpesaPayments.ItemsSource = mpesapayments;
                    }
                }
                else if (position == 3)
                {

                    List<dynamic> voucherpayments = new List<dynamic>();
                    var master = MainList.Select(k => (TicketPaymentMaster)k.tpm).Distinct(new PaymentMaster_Comparer()).ToList();
                    var items = MainList.Select(k => (TicketPaymentItem)k.tpi).ToList();
                    var voucherpaymentsonly = MainList.Select(k => (TicketPaymentItem)k.tpi).Where(k => k.Method.ToLower() == PosEnums.TicketPaymentMethods.Voucher.ToString().ToLower()).ToList();
                    if (master.Count > 0)
                    {
                        decimal total = 0;
                        int count = 0;
                        foreach (var x in master)
                        {
                            var a = voucherpaymentsonly.Where(k => k.ParentTransNo == x.TransNo).ToList();
                            total += a.Sum(j => j.AmountPaid);
                            count += a.Count;
                            foreach (var y in a)
                            {
                                voucherpayments.Add(new { Tpi = y, Tpm = x });
                            }

                        }
                        Label_MpesaCount_Count.Content = count.ToString();
                        Label_Mpesa_Total.Content = total.ToString();
                        Datagrid_Vouchers.ItemsSource = voucherpayments;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public class PaymentMaster_Comparer : IEqualityComparer<TicketPaymentMaster>
        {
            public bool Equals(TicketPaymentMaster x, TicketPaymentMaster y)
            {
                // compare multiple fields
                return x.PaymentMasterGuid == y.PaymentMasterGuid;
            }

            public int GetHashCode(TicketPaymentMaster obj)
            {
                return obj.PaymentMasterGuid.GetHashCode();
            }

        }

        public class PaymentItem_method_Comparer : IEqualityComparer<TicketPaymentItem>
        {
            public bool Equals(TicketPaymentItem x, TicketPaymentItem y)
            {
                // compare multiple fields
                return x.Method == y.Method;
            }

            public int GetHashCode(TicketPaymentItem obj)
            {
                return obj.Method.GetHashCode();
            }

        }

        private void Datagrid_Payments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
