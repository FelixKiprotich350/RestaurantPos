
using RestaurantManager.UserInterface.PosReports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using RestaurantManager.GlobalVariables;
using RestaurantManager.ApplicationFiles;
using DatabaseModels.OrderTicket;
using DatabaseModels.Inventory;
using DatabaseModels.CRM;
using DatabaseModels.Security;
using DatabaseModels.Payments;
using DatabaseModels.WorkPeriod;
using System.IO;
using CsvHelper;
using System.Globalization;
using winforms=System.Windows.Forms;
using System.Threading;
using CsvHelper.Configuration;
using RestaurantManager.ActivityLogs;
using RestaurantManager.UserInterface.WorkPeriods;

namespace RestaurantManager.UserInterface.PosReports.SalesReport
{
    /// <summary>
    /// Interaction logic for SalesReport.xaml
    /// </summary>
    public partial class MSalesReport : Page
    {
        readonly List<dynamic> MainList = new List<dynamic>();
        List<OrderItem> MainList_OItems = new List<OrderItem>();  
        List<OrderMaster> MainList_Orders = new List<OrderMaster>();
        List<MenuProductItem> MainList_ProductItems = new List<MenuProductItem>(); 
        List<PosUser> Users = new List<PosUser>();
        List<CustomerAccount> Customers = new List<CustomerAccount>();
        List<TicketPaymentMaster> TickPayMaster = new List<TicketPaymentMaster>();
        List<Un_OrderItem> productwise = new List<Un_OrderItem>();
        public MSalesReport()
        {
            InitializeComponent();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearLists();
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
                MainList_OItems.Clear();
                MainList_Orders.Clear();
                MainList_ProductItems.Clear();
                Users.Clear();
                TickPayMaster.Clear(); 
                Customers.Clear();
                productwise.Clear();
                //grids
                Datagrid_OrderItems.ItemsSource=null; 
                Datagrid_OrderItems.Items.Clear();
                Label_Profit.Content = "0.00";
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
                //var db = new PosDbContext();
                //foreach (var x in db.OrderItem.ToList())
                //{
                //    if (db.MenuProductItem.FirstOrDefault(k => k.ProductGuid == x.ProductItemGuid) != null)
                //    {
                //        x.BuyingPrice = db.MenuProductItem.FirstOrDefault(k => k.ProductGuid == x.ProductItemGuid).BuyingPrice;
                //        x.BuyingPriceTotal = db.MenuProductItem.FirstOrDefault(k => k.ProductGuid == x.ProductItemGuid).BuyingPrice * x.Quantity;
                //    }
                //}
                //db.SaveChanges();
                Button_SelectWorkPeriod.Content = "All";  
                Datepicker_Startdate.SelectedDate = null;
                Datepicker_Enddate.SelectedDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString(), "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                int item = TabControl_SalesTabControl.SelectedIndex;
                ClearLists();
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
                if (Datepicker_Startdate.SelectedDate!=null)
                {
                    startdate = Datepicker_Startdate.SelectedDate;
                }
                if (Datepicker_Enddate.SelectedDate!=null)
                {
                    enddate = Datepicker_Enddate.SelectedDate;
                }
                LoadItems(wperiod, startdate, enddate);
                TabControl_SalesTabControl.SelectedIndex = -1;
                TabControl_SalesTabControl.SelectedIndex = item;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 

        private void LoadItems(WorkPeriod wp,DateTime? startdate,DateTime? enddate)
        {
            try
            {
                 var db = new PosDbContext(); 
                //load Items
                MainList_OItems = db.OrderItem.AsNoTracking().ToList(); 
                MainList_Orders = db.OrderMaster.AsNoTracking().ToList(); 
                TickPayMaster = db.TicketPaymentMaster.AsNoTracking().ToList();
                Users = db.PosUser.AsNoTracking().ToList();
                Customers = db.CustomerAccount.AsNoTracking().ToList();
                MainList_ProductItems = db.MenuProductItem.AsNoTracking().ToList();
 
                var leftOuterJoin = from e in db.OrderItem.AsNoTracking()
                                    join d in db.OrderMaster.AsNoTracking() on e.OrderID equals d.OrderNo into dept
                                    from department in dept.DefaultIfEmpty().Where(d=>d.OrderStatus == PosEnums.OrderTicketStatuses.Completed.ToString())
                                    select new
                                    {
                                        om = department,
                                        oi = e
                                        //EmployeeCode = e.ParentProductItemGuid,
                                        //EmployeeName = e.ItemName,
                                        //DepartmentName = department.OrderNo
                                    };
                var t = leftOuterJoin.Where(a => a.om.OrderStatus == PosEnums.OrderTicketStatuses.Completed.ToString()).ToList();
                if (wp != null)
                {
                    t = t.Where(a => a.om.Workperiod == wp.WorkperiodName).ToList();
                }
                if (startdate != null && enddate != null)
                {
                    t = t.Where(a => a.om.OrderDate >= startdate && a.om.OrderDate <= enddate).ToList();
                }
                MainList.AddRange(t);
                ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Viewed Sales Report", "WorkPeriod=" + wp?.WorkperiodName + ",startdate=" + startdate?.ToString() + ", enddate=" + enddate?.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
         
        private void TabControl_SalesTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int position = TabControl_SalesTabControl.SelectedIndex;
                if (position == 0)
                {
                   
                    List<dynamic> peruser = new List<dynamic>();
                    var items1 = MainList.Where(m => ((OrderMaster)m.om).OrderStatus == PosEnums.OrderTicketStatuses.Completed.ToString()).ToList();
                    var items2 = items1.Select(k => k.oi).Cast<OrderItem>().ToList();
                    var items3 = items1.Select(k => k.oi).Cast<OrderItem>().Distinct(new OrderItem_Comparer()).ToList();

                    List<Un_OrderItem> displist = new List<Un_OrderItem>();
                    if (items3.Count > 0)
                    {
                        foreach (var x in items3)
                        {
                            Un_OrderItem item = new Un_OrderItem();
                            int qty = items2.Where(k => k.ProductItemGuid == x.ProductItemGuid).Sum(l => l.Quantity);
                            decimal total = items2.Where(k => k.ProductItemGuid == x.ProductItemGuid).Sum(l => l.Total);
                             decimal buyingtotal = items2.Where(k => k.ProductItemGuid == x.ProductItemGuid).Sum(l => l.BuyingPriceTotal);
                            item.ParentProductItemGuid = x.ProductItemGuid;
                            item.ItemName = x.ItemName;
                            item.Quantity = qty;
                            item.Total = total; 
                            item.ProfitTotal = total-buyingtotal;
                            item.BuyingPriceTotal = buyingtotal;
                            displist.Add(item);
                        }
                        decimal totals = 0;
                        decimal profitstotal = 0;
                        decimal buyingpricetotal = 0;
                        int count = 0;
                        foreach (var x in displist)
                        {
                            totals += x.Total;
                            profitstotal += x.ProfitTotal;
                            buyingpricetotal += x.BuyingPriceTotal;
                            count++;
                        } 
                        Label_Products_Total.Content = totals.ToString();
                        Label_Products_BuyingpriceTotal.Content = buyingpricetotal.ToString();
                        Label_Profit.Content = profitstotal.ToString();
                        Datagrid_OrderItems.ItemsSource = displist.OrderBy(k=>k.ItemName);
                        productwise = displist;
                    }
                }
                else if (position == 1)
                {
                    List<dynamic> peruser = new List<dynamic>();
                    var orders = MainList.Select(k => k.om).Cast<OrderMaster>().ToList();
                    var Distorders = orders.Distinct(new OrderMaster_Comparer()).ToList();
                    foreach (var user in Users)
                    {
                        var ordnos = Distorders.Where(a => a != null && a.UserServing == user.UserName).ToList();
                        decimal total = 0;
                        int count = 0;
                        foreach (var ordm in ordnos)
                        {
                            total += TickPayMaster.Where(a => a.TicketNo == ordm.OrderNo).Sum(p => p.TotalAmountCharged);
                            count++;
                        }
                        peruser.Add(new { FullName = user.UserFullName, user.UserName, SalesCount = count, TotalSales = total });
                    }
                    decimal usertotals = 0;
                    int usercount = 0;
                    foreach (var x in peruser)
                    {
                        usertotals += x.TotalSales;
                        usercount++;
                    }
                    Label_User_Count.Content = usercount.ToString();
                    Label_user_Total.Content = usertotals.ToString();
                    DataGrid_PeruserSales.ItemsSource = peruser;
                }
                else if (position == 2)
                {
                    List<dynamic> percustomer = new List<dynamic>();
                    var orders = MainList.Select(k => k.om).Cast<OrderMaster>().ToList();
                    var Distorders = orders.Distinct(new OrderMaster_Comparer()).ToList();
                    foreach (var cust in Customers)
                    {
                        var ordnos = Distorders.Where(a => a != null && a.CustomerRefference == cust.PersonAccNo).ToList();
                        decimal total = 0;
                        int custcount = 0;
                        foreach (var ordm in ordnos)
                        {
                            total += TickPayMaster.Where(a => a.TicketNo == ordm.OrderNo).Sum(p => p.TotalAmountCharged);
                            custcount++;
                        }
                        percustomer.Add(new { Phone = cust.PersonAccNo, cust.FullName, AppearanceCount = custcount, Total = total });
                    }
                    //unregistered customers
                    int X_CustCount = 0;
                    decimal X_Total = 0;
                    var x_ord = Distorders.Where(k => k.CustomerRefference.ToLower() == "none").ToList();
                    foreach (var ord in x_ord)
                    {
                        X_Total += TickPayMaster.Where(a => a.TicketNo == ord.OrderNo).Sum(p => p.TotalAmountCharged);
                        X_CustCount++;
                    }
                    percustomer.Add(new { Phone = "Unregistered", FullName = "Unregistered", AppearanceCount = X_CustCount, Total = X_Total });
                    //summary
                    decimal totals = 0;
                    int count = 0;
                    foreach (var x in percustomer)
                    {
                        totals += x.Total;
                        count++;
                    }
                    Label_Customer_Count.Content = count.ToString();
                    Label_Customer_Total.Content = totals.ToString();
                    Datagrid_CustomerSales.ItemsSource = percustomer;

                }
                else if (position == 3)
                {
                    List<string> departments = Enum.GetNames(typeof(PosEnums.Departments)).ToList();
                    List<dynamic> departmentsobject = new List<dynamic>();
                    
                    var items = MainList.Select(k => k.oi).Cast<OrderItem>().ToList();
                    var finalitems = from ta in MainList
                                     join tb in MainList_ProductItems on ((OrderItem)ta.oi).ProductItemGuid equals tb.ProductGuid
                                     select new
                                     {
                                         Order = ta,
                                         tb.Department
                                     };
                    decimal totals = 0; 
                    int count = 0;
                    //MessageBox.Show(finalitems.Count().ToString());
                    foreach (var x in departments)
                    {
                        var xtotal = finalitems.Where(a=>a.Department==x).ToList().Sum(xa => ((OrderItem)xa.Order.oi).Total);
                        var xPurchasetotal = finalitems.Where(a=>a.Department==x).ToList().Sum(xa => ((OrderItem)xa.Order.oi).BuyingPriceTotal);
                        var xcount = finalitems.Where(a => a.Department == x).ToList().Count();
                        departmentsobject.Add(new
                        {
                            DepartmentName=x,
                            DepartmentSalesCount=xcount,
                            DepartmentSalesTotal=xtotal,
                            DepartmentSalesPurchaseTotal=xPurchasetotal,
                            DepartmentSalesProfit = xtotal-xPurchasetotal,
                        });
                    }
                    Label_PerDepartment_Count.Content = count.ToString();
                    Label_PerDepartment_Total.Content = totals.ToString();
                    Datagrid_DepartmentSales.ItemsSource = departmentsobject;
                }
                else if (position == 4)
                {
                    var items = MainList.Select(k => k.oi).Cast<OrderItem>().ToList();
                    decimal totals = 0;
                    int count = 0;
                    foreach (var x in items)
                    {
                        totals += x.Total;
                        count++;
                    }
                    Label_Allsales_Count.Content = count.ToString();
                    Label_Allsales_Total.Content = totals.ToString();
                    Datagrid_AllSales.ItemsSource = items;
                }
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
                    if (w.SelectedWorkperiod!=null)
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

        public class OrderItem_Comparer : IEqualityComparer<OrderItem>
        {
            public bool Equals(OrderItem x, OrderItem y)
            {
                // compare multiple fields
                return x.ProductItemGuid == y.ProductItemGuid;
            }

            public int GetHashCode(OrderItem obj)
            {
                return obj.ProductItemGuid.GetHashCode();
            }

        }
        public class OrderMaster_Comparer : IEqualityComparer<OrderMaster>
        {
            public bool Equals(OrderMaster x, OrderMaster y)
            {
                // compare multiple fields
                return x.OrderNo == y.OrderNo;
            }

            public int GetHashCode(OrderMaster obj)
            {
                return obj.OrderNo.GetHashCode();
            }

        }
        public class Customer_Comparer : IEqualityComparer<CustomerAccount>
        {
            public bool Equals(CustomerAccount x, CustomerAccount y)
            {
                // compare multiple fields
                return x.PersonAccNo == y.PersonAccNo;
            }

            public int GetHashCode(CustomerAccount obj)
            {
                return obj.PersonAccNo.GetHashCode();
            }

        }

        private   void Button_ExportProducts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (productwise.Count <= 0)
                {
                    MessageBox.Show("There are no items to Export!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                string path = "";
                using (var fbd = new winforms.FolderBrowserDialog())
                {
                    winforms.DialogResult result = fbd.ShowDialog();

                    if (result == winforms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        path = fbd.SelectedPath + "\\Export-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                        // File.Create(path);
                        Thread.Sleep(1000);

                    }
                } 
                //StreamWriter writer = new StreamWriter(new FileStream(path,FileMode.Create, FileAccess.Write,FileShare.Read));
                var configPersons = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true
                }; 
                using (var writer = new StreamWriter(path))
                using (var csv = new CsvWriter(writer, configPersons))
                {
                    csv.WriteRecords(productwise.OrderBy(k=>k.ItemName));
                }
                 
                MessageBox.Show("Data Exported Successfully!\n" + "File Path: " + path, "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Exported sales (prouctwise)", "Destination File=" + path);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
