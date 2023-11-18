using DatabaseModels.OrderTicket;
using DatabaseModels.Payments;
using DatabaseModels.Security;
using DatabaseModels.WorkPeriod;  
using RestaurantManager.ApplicationFiles; 
using RestaurantManager.GlobalVariables;
using RestaurantManager.UserInterface.Accounts.ReportModels;
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
    public partial class AccountsDashboard : Page
    {
        public bool IsChartsView = false;
        public AccountsDashboard()
        {
            InitializeComponent();
            // unsubscribe from the default right-click menu event
            //Plot_Gen_Topselling.RightClicked -= Plot_Gen_Topselling.DefaultRightClickEvent;
 
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
        /// <summary>
        /// Get top summaries
        /// </summary> 

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

        private void Button_Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                DateTime? startdate = null;
                DateTime? enddate = null;
                WorkPeriod wperiod = null;
                if (Button_SelectWorkPeriod.Content.ToString() == "All")
                {
                    wperiod = null;
                }
                else
                {
                    wperiod = (WorkPeriod)Button_SelectWorkPeriod.Tag;
                }
                if ((bool)Checkbox_PaymentByDate.IsChecked)
                {
                    if (Datepicker_From.SelectedDate == null | Datepicker_To.SelectedDate == null)
                    {
                        MessageBox.Show("Select Start Date & End Date!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    startdate = Datepicker_From.SelectedDate;
                    enddate = Datepicker_To.SelectedDate;

                }
                LoadUIData(wperiod, startdate, enddate);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_ToggleView_Click(object sender, RoutedEventArgs e)
        {
            if (IsChartsView) { IsChartsView = false; }
            else { IsChartsView = true; }
            SwitchTableGraphView(); 
 
        }
        private void SwitchTableGraphView()
        {
            try
            {
                if (IsChartsView)
                {
                    //received payments
                    GridPanel_Receivedpayments.Visibility = Visibility.Visible;
                    Datagrid_SalesPaymentsSummary.Visibility = Visibility.Collapsed;
                    //invoice payments
                    GridPanel_Invoicepayments.Visibility = Visibility.Visible;
                    Datagrid_InvoicePaymentsSummary.Visibility = Visibility.Collapsed;
                    //sales department
                    GridPanel_Departmentsales.Visibility = Visibility.Visible;
                    Datagrid_DepartmentsSalesSummary.Visibility = Visibility.Collapsed;
                    //sales department
                    GridPanel_Ticketsales.Visibility = Visibility.Visible;
                    Datagrid_TicketsSales.Visibility = Visibility.Collapsed;
                }
                else
                {
                    //received payments
                    GridPanel_Receivedpayments.Visibility = Visibility.Collapsed;
                    Datagrid_SalesPaymentsSummary.Visibility = Visibility.Visible;
                    //invoice payments
                    GridPanel_Invoicepayments.Visibility = Visibility.Collapsed;
                    Datagrid_InvoicePaymentsSummary.Visibility = Visibility.Visible;
                    //sales department
                    GridPanel_Departmentsales.Visibility = Visibility.Collapsed;
                    Datagrid_DepartmentsSalesSummary.Visibility = Visibility.Visible;
                    //sales department
                    GridPanel_Ticketsales.Visibility = Visibility.Collapsed;
                    Datagrid_TicketsSales.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
         
        void LoadUIData(WorkPeriod wp, DateTime? startdate, DateTime? enddate)
        {
            try
            {
                var db = new PosDbContext();
                var paylist = db.TicketPaymentItem.AsNoTracking().ToList();
                var paymaster = new PosDbContext().TicketPaymentMaster.AsNoTracking().ToList();
                var ticketmaster = new PosDbContext().OrderMaster.AsNoTracking().ToList();
                var orderitems = new PosDbContext().OrderItem.AsNoTracking().ToList();

                var sales = from item in db.OrderItem.AsNoTracking()
                            join master in db.OrderMaster.AsNoTracking().Where(p => p.OrderStatus != PosEnums.OrderTicketStatuses.Voided.ToString()) on item.OrderID equals master.OrderNo
                            join inventory in db.MenuProductItem.AsNoTracking() on item.ProductItemGuid equals inventory.ProductGuid
                            select new { master.Workperiod, inventory.Department, item.PostDate, item.Total, item.BuyingPriceTotal, item.ProductItemGuid, item.Quantity };
                var allsales = sales.ToList();
                if (wp != null)
                {
                    paylist.RemoveAll(w => w.Workperiod != wp.WorkperiodName);
                    allsales.RemoveAll(w => w.Workperiod != wp.WorkperiodName);
                    paymaster.RemoveAll(w => w.WorkPeriod != wp.WorkperiodName);
                }
                if (startdate != null)
                {
                    paylist.RemoveAll(w => w.PaymentDate < startdate);
                    allsales.RemoveAll(w => w.PostDate < startdate);
                    paymaster.RemoveAll(w => w.PaymentDate < startdate);

                }
                if (enddate != null)
                {
                    paylist.RemoveAll(w => w.PaymentDate > enddate);
                    allsales.RemoveAll(w => w.PostDate > enddate);
                    paymaster.RemoveAll(w => w.PaymentDate > startdate);
                }


                GetSalesPayment(paylist.Where(k => k.PayForSource == PosEnums.PaymentForSources.SalesBill.ToString()).ToList());
                GetInvoicePayments(paylist.Where(k => k.PayForSource == PosEnums.PaymentForSources.InvoicePay.ToString()).ToList());
                GetSalesSummary(allsales);
                GetSalesTickets();
                GetTopSellingProducts(allsales);
                GetWeekDaySales(allsales);
                GetMonthlySales(allsales);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        void GetSalesPayment(List<TicketPaymentItem> salesp)
        {
            try
            {
                decimal total = 0;
                decimal cash = 0;
                decimal mpesa = 0;
                decimal cards = 0;
                decimal invoice = 0;
                var forsum = salesp;
                total = forsum.Sum(t => t.AmountUsed);
                cash = forsum.Where(k => k.Method == PosEnums.TicketPaymentMethods.Cash.ToString()).Sum(t => t.AmountUsed);
                mpesa = forsum.Where(k => k.Method == PosEnums.TicketPaymentMethods.Mpesa.ToString()).Sum(t => t.AmountUsed);
                cards = forsum.Where(k => k.Method == PosEnums.TicketPaymentMethods.Card.ToString()).Sum(t => t.AmountUsed);
                invoice = forsum.Where(k => k.Method == PosEnums.TicketPaymentMethods.Invoice.ToString()).Sum(t => t.AmountUsed);
                List<RowRecordObject> rowRecords = new List<RowRecordObject>
                {
                    new RowRecordObject() { VariableName = "Cash", DecimalValue1 = cash },
                    new RowRecordObject() { VariableName = "Mpesa", DecimalValue1 = mpesa },
                    new RowRecordObject() { VariableName = "Cards", DecimalValue1 = cards },
                    new RowRecordObject() { VariableName = "Invoices", DecimalValue1 = invoice },
                    new RowRecordObject() { VariableName = "Total", DecimalValue1 = total }
                };
                Datagrid_SalesPaymentsSummary.ItemsSource = rowRecords;
                //charts
                var plt = Plot_Receivedpayments.Plot;
                plt.Clear();
                double[] values = { Convert.ToDouble(cash), Convert.ToDouble(mpesa), Convert.ToDouble(cards),  Convert.ToDouble(invoice) };
                string[] labels = { "Cash", "Mpesa", "Cards" ,"Invoice"};
                var pie = plt.AddPie(values);
                pie.SliceLabels = labels;
                pie.ShowPercentages = true;
                pie.ShowValues = false;
                pie.ShowLabels = true; 
                Plot_Receivedpayments.Configuration.Quality = ScottPlot.Control.QualityMode.Low;
                plt.Legend().UpdateLegendItems(plt,false);  
                Plot_Receivedpayments.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        void GetInvoicePayments(List<TicketPaymentItem> salesp)
        {
            try
            {
                decimal total = 0;
                decimal cash = 0;
                decimal mpesa = 0;
                decimal cards = 0; 
                var forsum = salesp;
                total = forsum.Sum(t => t.AmountUsed);
                cash = forsum.Where(k => k.Method == PosEnums.TicketPaymentMethods.Cash.ToString()).Sum(t => t.AmountUsed);
                mpesa = forsum.Where(k => k.Method == PosEnums.TicketPaymentMethods.Mpesa.ToString()).Sum(t => t.AmountUsed);
                cards = forsum.Where(k => k.Method == PosEnums.TicketPaymentMethods.Card.ToString()).Sum(t => t.AmountUsed);
                List<RowRecordObject> rowRecords = new List<RowRecordObject>
                {
                    new RowRecordObject() { VariableName = "Cash", DecimalValue1 = cash },
                    new RowRecordObject() { VariableName = "Mpesa", DecimalValue1 = mpesa },
                    new RowRecordObject() { VariableName = "Cards", DecimalValue1 = cards },
                    new RowRecordObject() { VariableName = "Total", DecimalValue1 = total }
                };
                Datagrid_InvoicePaymentsSummary.ItemsSource = rowRecords;
                //charts
                var plt = Plot_Invoicepayments.Plot;
                plt.Clear();
                double[] values = { Convert.ToDouble(cash), Convert.ToDouble(mpesa), Convert.ToDouble(cards)};
                string[] labels = { "Cash", "Mpesa", "Cards" };
                var pie = plt.AddPie(values);
                pie.SliceLabels = labels;
                pie.ShowPercentages = true; 
                pie.ShowValues = false;
                pie.ShowLabels = true;
                Plot_Invoicepayments.Configuration.Quality = ScottPlot.Control.QualityMode.Low;
                plt.Legend();

                Plot_Invoicepayments.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void GetSalesSummary(dynamic salesp)
        {
            try
            {
                IEnumerable<dynamic> salesitems = salesp;
                decimal total = salesitems.Sum(t => Convert.ToInt32(t.Total));
                decimal restaurant = salesitems.Where(k => k.Department == PosEnums.Departments.Restaurant.ToString()).Sum(t => Convert.ToInt32(t.Total));
                decimal bar = salesitems.Where(k => k.Department == PosEnums.Departments.Bar.ToString()).Sum(t => Convert.ToInt32(t.Total));
                decimal services = salesitems.Where(k => k.Department == PosEnums.Departments.Services.ToString()).Sum(t => Convert.ToInt32(t.Total));
                List<RowRecordObject> rowRecords = new List<RowRecordObject>
                {
                    new RowRecordObject() { VariableName = "Restaurant", DecimalValue1 = restaurant },
                    new RowRecordObject() { VariableName = "Bar", DecimalValue1 = bar },
                    new RowRecordObject() { VariableName = "Services", DecimalValue1 = services },
                    new RowRecordObject() { VariableName = "Total", DecimalValue1 = total }
                };
                Datagrid_DepartmentsSalesSummary.ItemsSource = rowRecords;
                //charts
                var plt = Plot_departmentsales.Plot;  
                plt.Clear();
                double[] values = { Convert.ToDouble(restaurant), Convert.ToDouble(bar), Convert.ToDouble(services) };
                string[] labels = { "Restaurant", "Bar", "Services"};
                var pie = plt.AddPie(values);
                pie.SliceLabels = labels;
                pie.ShowPercentages = true;
                pie.ShowValues = false;
                pie.ShowLabels = true;
                Plot_departmentsales.Configuration.Quality = ScottPlot.Control.QualityMode.Low;
                plt.Legend(); 
                Plot_departmentsales.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        void GetSalesTickets()
        {
            try
            {
                var db = new PosDbContext();
                var completedtickets = db.OrderMaster.AsNoTracking().Where(k => k.OrderStatus == PosEnums.OrderTicketStatuses.Completed.ToString()).ToList();
                var pendingtickets = db.OrderMaster.AsNoTracking().Where(k => k.OrderStatus == PosEnums.OrderTicketStatuses.Pending.ToString()).ToList();
                var orderitems = db.OrderItem.AsNoTracking().ToList();
                var payments = db.TicketPaymentItem.AsNoTracking().ToList();
                decimal total = 0;
                decimal pending = 0;
                decimal pendingpaid = 0;
                foreach (var x in completedtickets)
                {
                    total += orderitems.Where(k => k.OrderID == x.OrderNo).Sum(k => k.Total);
                }
                foreach (var x in pendingtickets)
                {
                    pending += orderitems.Where(k => k.OrderID == x.OrderNo).Sum(k => k.Total);
                }   
                
                foreach (var x in pendingtickets)
                {
                    pendingpaid += payments.Where(k => k.ParentSourceRef == x.OrderNo).Sum(k => k.AmountUsed);
                }
                decimal balance = pendingpaid - pending;
                List<RowRecordObject> rowRecords = new List<RowRecordObject>
                {
                    new RowRecordObject() { VariableName = "Checked Out", DecimalValue1 = total },
                    new RowRecordObject() { VariableName = "Pending Total", DecimalValue1 = pending },
                    new RowRecordObject() { VariableName = "Partially Paid", DecimalValue1 = pendingpaid },
                    new RowRecordObject() { VariableName = "Pending Balance", DecimalValue1 = balance < 0 ? balance : 0 }
                };
                Datagrid_TicketsSales.ItemsSource = rowRecords;
                //charts
                var plt = Plot_Ticketsales.Plot;  
                plt.Clear();
                double[] values = { Convert.ToDouble(total), Convert.ToDouble(pending), Convert.ToDouble(pendingpaid),Convert.ToDouble(balance) };
                string[] labels = { "Checked Out", "Pending Total", "Partially Paid","Balance"};
                var pie = plt.AddPie(values);
                pie.SliceLabels = labels;
                pie.ShowPercentages = true;
                pie.ShowValues = false;
                pie.ShowLabels = true;
                Plot_Ticketsales.Configuration.Quality = ScottPlot.Control.QualityMode.Low;
                plt.Legend();
                Plot_Ticketsales.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

          
        /// <summary>
        /// General division data
        /// </summary> 

        private void GetTopSellingProducts(dynamic items)
        {
            try
            {
                var db = new PosDbContext();
                var products = db.MenuProductItem.AsNoTracking().ToList();
                IEnumerable<dynamic> itemssold = items;
               
                var topselling = itemssold.GroupBy(item => Convert.ToString(item.ProductItemGuid)).Select(group => new
                {
                    products.FirstOrDefault(a => a.ProductGuid == group.Key).ProductGuid,
                    ProductName = (products.FirstOrDefault(a => a.ProductGuid == group.Key) != null) ? products.FirstOrDefault(a => a.ProductGuid == group.Key).ProductName : "Unknown",
                    SumQuantity = group.Sum(item => item.Quantity),
                    SumTotal = group.Sum(k => Convert.ToInt32(k.Total))
                }).OrderByDescending(item => item.SumQuantity).Take(20);
                 
                 
                Datagrid_Gen_TopsellingProducts.ItemsSource = topselling.ToList();
                var plt = Plot_Gen_Topselling.Plot;
                plt.Clear();
                List<string> productX = new List<string>();
                List<double> qtyX = new List<double>();
                List<double> TotalX = new List<double>();

                foreach(var x in topselling)
                {
                    productX.Add(x.ProductName);
                    qtyX.Add(x.SumQuantity);
                    TotalX.Add(x.SumTotal);
                }
                // group all data together
                string[] groupNames = productX.ToArray() ;
                string[] seriesNames = { "Quantity", "Total Sales",  };
                double[][] valuesBySeries = { qtyX.ToArray(), TotalX.ToArray(),  }; 

                // add the grouped bar plots and show a legend
                plt.AddBarGroups(groupNames, seriesNames, valuesBySeries,null);
                var leg = plt.Legend(location: Alignment.UpperRight);
                leg.FillColor = System.Drawing.Color.White;
                leg.FontColor = System.Drawing.Color.Black; 

                // adjust axis limits so there is no padding below the bar graph
                plt.SetAxisLimits(yMin: 0); 
                // Set custom X-axis tick labels and rotate them vertically
                plt.XAxis.AxisTicks.TickLabelRotation = 90;
                plt.Title("Products Sales vs Quantity", true, System.Drawing.Color.Black, null, null);
                Plot_Gen_Topselling.Configuration.Quality = ScottPlot.Control.QualityMode.Low;
                plt.Render();
                Plot_Gen_Topselling.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
         
        private void GetWeekDaySales(dynamic items)
        {
            try
            {
                var db = new PosDbContext();
                var products = db.MenuProductItem.AsNoTracking().ToList();
                IEnumerable<dynamic> itemssold = items;
                var weekday = itemssold.GroupBy(item => Convert.ToDateTime(item.PostDate).ToString("dddd")).Select(group => new
                {
                    PostDate = group.Key,
                    SumTotal = group.Sum(k => Convert.ToInt32(k.Total)),
                    Percentage = 0
                }).OrderByDescending(item => item.SumTotal);
                 
                
                var plt = Plot_Gen_WeekDayDistribution.Plot;
                plt.Clear();
                List<string> dayX = new List<string>(); 
                List<double> TotalX = new List<double>();
                List<dynamic> Weekdaysale = new List<dynamic>();

                decimal weektotal = weekday.Sum(k => k.SumTotal);
                foreach(var x in weekday)
                {
                    dayX.Add(x.PostDate); 
                    TotalX.Add(Convert.ToDouble(x.SumTotal));
                    Weekdaysale.Add(new 
                    {
                        x.PostDate,
                        x.SumTotal,
                        Percentage = x.SumTotal * 100 / weektotal 
                    }); 
                }
                Datagrid_Gen_WeekDayDistribution.ItemsSource = Weekdaysale;
                List<double> pos = new List<double>();
                int totaldays = 0;
                while (totaldays<Weekdaysale.Count)
                {
                    pos.Add(totaldays);
                    totaldays++;
                }
                string customFormatter(double y) => $"%={y * Convert.ToDouble(100 / weektotal):N2}";
                double[] positions = pos.ToArray();
               var bar= plt.AddBar(TotalX.ToArray(), positions);
                bar.ValueFormatter = customFormatter;
                bar.ShowValuesAboveBars = true;
                bar.Font.Color = System.Drawing.Color.Black;
                bar.Font.Size = 14;
                bar.Font.Bold = true;
                bar.FillColor = System.Drawing.Color.Orange;
                bar.BarWidth = 0.5;
                plt.XTicks(positions, dayX.ToArray());
                plt.SetAxisLimits(yMin: 0); 
                plt.Title("Sales per Day of the Week", true, System.Drawing.Color.Black, null, null);
                Plot_Gen_WeekDayDistribution.Configuration.Quality = ScottPlot.Control.QualityMode.Low;
                plt.Render();
                Plot_Gen_WeekDayDistribution.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetMonthlySales(dynamic items)
        {
            try
            {
                var db = new PosDbContext();
                var products = db.MenuProductItem.AsNoTracking().ToList();
                IEnumerable<dynamic> itemssold = items;
                var monthly = itemssold.GroupBy(item => Convert.ToDateTime(item.PostDate).ToString("MMMM")).Select(group => new
                {
                    PostDate = group.Key,
                    SumTotal = group.Sum(k => Convert.ToInt32(k.Total)),
                    Percentage = 0
                }).OrderByDescending(item => item.SumTotal);


                var plt = Plot_Gen_MonthlyDistribution.Plot;
                plt.Clear();
                List<string> monthX = new List<string>();
                List<double> TotalX = new List<double>();
                List<dynamic> Monthlysale = new List<dynamic>();

                decimal monthlytotal = monthly.Sum(k => k.SumTotal);
                foreach (var x in monthly)
                {
                    monthX.Add(x.PostDate);
                    TotalX.Add(Convert.ToDouble(x.SumTotal));
                    Monthlysale.Add(new
                    {
                        x.PostDate,
                        x.SumTotal,
                        Percentage = x.SumTotal * 100 / monthlytotal
                    });
                }
                Datagrid_Gen_MonthlyDistribution.ItemsSource = Monthlysale;
                List<double> pos = new List<double>();
                int totaldays = 0;
                while (totaldays < Monthlysale.Count)
                {
                    pos.Add(totaldays);
                    totaldays++;
                }
                string customFormatter(double y) => $"%={y * Convert.ToDouble(100 / monthlytotal):N2}";
                double[] positions = pos.ToArray();
                var bar = plt.AddBar(TotalX.ToArray(), positions);
                bar.ValueFormatter = customFormatter;
                bar.ShowValuesAboveBars = true;
                bar.Font.Color = System.Drawing.Color.Black;
                bar.Font.Size = 14;
                bar.Font.Bold = true;
                bar.FillColor = System.Drawing.Color.Orange;
                bar.BarWidth = 0.5;
                plt.XTicks(positions, monthX.ToArray());
                plt.SetAxisLimits(yMin: 0);
                plt.Title("Sales per Month of the Year", true, System.Drawing.Color.Black, null, null);
                Plot_Gen_MonthlyDistribution.Configuration.Quality = ScottPlot.Control.QualityMode.Low;
                plt.Render();
                Plot_Gen_MonthlyDistribution.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
                 
        private void Button_test_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = new PosDbContext();
                var paylist = db.TicketPaymentItem.AsNoTracking().ToList();
                var sales = db.OrderItem.AsNoTracking().ToList(); 
                var salesmaster = db.OrderMaster.AsNoTracking().ToList();

                var distinct = db.TicketPaymentMaster.AsNoTracking().ToList();
                decimal pending = 0;
                foreach (var x in salesmaster.Where(k=>k.OrderStatus=="Pending").ToList())
                {

                    pending += paylist.Where(p=>p.ParentSourceRef==x.OrderNo).Sum(k=>k.AmountUsed);
                }
                MessageBox.Show(pending.ToString());
                //db.SaveChanges();
                //List<string> bb = new List<string>();
                //List<string> aaa = new List<string>();
                //string m = "";
                //foreach (var x in salesmaster.Where(k=>k.OrderStatus=="Completed"))
                //{

                //    var a = sales.Where(l => l.OrderID == x.OrderNo).Sum(t => t.Total);
                //    var b = paylist.Where(l => l.ParentSourceRef == x.OrderNo).Sum(t => t.AmountUsed);
                //    var d = db.TicketPaymentMaster.FirstOrDefault(l => l.TicketNo == x.OrderNo);
                //    if(d == null)
                //    {
                //       aaa.Add(x.OrderNo);
                //    }
                //    else
                //    {
                //        var ty = db.TicketPaymentMaster.Where(l => l.TicketNo == x.OrderNo).Sum(y => y.TotalAmountPaid);
                //        if (b != a | b !=ty )
                //        {
                //            m += x.OrderNo + "-paid:" + b.ToString() + "-charged:" + a.ToString() + "\n";
                //            bb.Add(x.OrderNo);
                //        }
                //    }

                //}

                //MessageBox.Show(aaa.Count.ToString());

                //foreach (var x in bb)
                //{
                //    var a = sales.Where(l => l.OrderID == x).Sum(t => t.Total);
                //    var b = paylist.Where(l => l.ParentSourceRef == x).Count();
                //    var c = db.TicketPaymentMaster.AsNoTracking().Where(l => l.TicketNo == x).Count();
                //    if (b == 1 && c == 1)
                //    {
                //        db.TicketPaymentMaster.First(l => l.TicketNo == x).TotalAmountCharged = a;
                //        db.TicketPaymentMaster.First(l => l.TicketNo == x).TotalAmountPaid = a;
                //        db.TicketPaymentMaster.First(l => l.TicketNo == x).TicketBalanceReturned = 0;
                //        db.TicketPaymentItem.First(l => l.ParentSourceRef == x).AmountPaid = a;
                //        db.TicketPaymentItem.First(l => l.ParentSourceRef == x).AmountUsed = a;
                //    }

                //} 
                MessageBox.Show("done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
    }
    public class PlotDataModel
    {
        public double[] XData { get; set; }
        public double[] YData { get; set; }
    }

}
