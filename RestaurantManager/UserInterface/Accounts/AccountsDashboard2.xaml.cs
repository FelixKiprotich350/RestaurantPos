using DatabaseModels.Payments;
using DatabaseModels.Security;
using DatabaseModels.WorkPeriod;
using RestaurantManager.ApplicationFiles; 
using RestaurantManager.GlobalVariables;
using RestaurantManager.UserInterface.PosReports;
using ScottPlot;
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
    public partial class AccountsDashboard2 : Page
    {
        public AccountsDashboard2()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetTopSellingProducts();
                Plot_Gen_Topselling.Refresh();
                 
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
             
        }


        /// <summary>
        /// General division data
        /// </summary>
        /// <param name="tlist"></param>
        /// <param name="invlist"></param>

        private void GetTopSellingProducts()
        {
            try
            {
                var db = new PosDbContext();
                var itemssold = db.OrderItem.AsNoTracking().ToList();
                var products = db.MenuProductItem.AsNoTracking().ToList();
                var topselling = itemssold.GroupBy(item => item.ProductItemGuid).Select(group => new
                {
                    ProductGuid = group.Key,
                    ProductName = (products.FirstOrDefault(a => a.ProductGuid == group.Key)!=null)? products.FirstOrDefault(a=>a.ProductGuid==group.Key).ProductName:"Unknown",
                    SumQuantity = group.Sum(item => item.Quantity),
                    SumTotal=group.Sum(k=>k.Total)
                }).OrderByDescending(item => item.SumQuantity).Take(20); 
                Datagrid_Gen_TopsellingProducts.ItemsSource = topselling;
                var plt = Plot_Gen_Topselling.Plot; 
                List<string> productX = new List<string>();
                List<double> qtyX = new List<double>();
                List<double> TotalX = new List<double>();

                foreach(var x in topselling)
                {
                    productX.Add(x.ProductName);
                    qtyX.Add(x.SumQuantity);
                    TotalX.Add(Convert.ToDouble(x.SumTotal));
                }
                // group all data together
                string[] groupNames = productX.ToArray() ;
                string[] seriesNames = { "Quantity", "Total Sales",  };
                double[][] valuesBySeries = { qtyX.ToArray(), TotalX.ToArray(),  }; 

                // add the grouped bar plots and show a legend
                plt.AddBarGroups(groupNames, seriesNames, valuesBySeries,null);
                var leg = plt.Legend(location: Alignment.UpperLeft);
                leg.FillColor = System.Drawing.Color.White;
                leg.FontColor = System.Drawing.Color.Black; 

                // adjust axis limits so there is no padding below the bar graph
                plt.SetAxisLimits(yMin: 0);
                string[] customXTickLabels = { "Group 1", "Group 2" };

                // Set custom X-axis tick labels and rotate them vertically
                plt.XAxis.AxisTicks.TickLabelRotation = 90;
                plt.Title("Products Sales vs Quantity", true, System.Drawing.Color.Black, null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
