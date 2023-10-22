
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

namespace RestaurantManager.UserInterface.PosReports
{
    /// <summary>
    /// Interaction logic for SalesReport.xaml
    /// </summary>
    public partial class ProfitAnalysis : Page
    {
        readonly List<dynamic> MainList = new List<dynamic>();
        List<OrderItem> MainList_OItems = new List<OrderItem>();  
        List<OrderMaster> MainList_Orders = new List<OrderMaster>();
        List<MenuProductItem> MainList_ProductItems = new List<MenuProductItem>(); 
        List<PosUser> Users = new List<PosUser>();
        List<CustomerAccount> Customers = new List<CustomerAccount>();
        List<TicketPaymentMaster> TickPayMaster = new List<TicketPaymentMaster>();
        public ProfitAnalysis()
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
                //grids
                Datagrid_OrderItems.ItemsSource=null; 
                Datagrid_OrderItems.Items.Clear();  
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
                //MessageBox.Show("Loading Done!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
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
                   //productwise
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
                            item.ParentProductItemGuid = x.ProductItemGuid;
                            item.ItemName = x.ItemName;
                            item.Quantity = qty;
                            item.Total = total;
                            displist.Add(item);
                        }
                        decimal totals = 0;
                        int count = 0;
                        foreach (var x in displist)
                        {
                            totals += x.Total;
                            count++;
                        }
                        Label_Products_Count.Content = count.ToString();
                        Label_Products_Total.Content = totals.ToString();
                        Datagrid_OrderItems.ItemsSource = displist;
                    }
                }  
                else if (position == 1)
                {
                    //departmentwise
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
                        var xcount = finalitems.Where(a => a.Department == x).ToList().Count();
                        departmentsobject.Add(new
                        {
                            DepartmentName=x,
                            DepartmentSalesCount=xcount,
                            DepartmentSalesTotal=xtotal,
                        });
                    }
                    Label_PerDepartment_Count.Content = count.ToString();
                    Label_PerDepartment_Total.Content = totals.ToString();
                    Datagrid_DepartmentSales.ItemsSource = departmentsobject;
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
             
    }
}
