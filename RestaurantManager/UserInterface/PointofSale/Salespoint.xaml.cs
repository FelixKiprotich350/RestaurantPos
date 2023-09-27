using RestaurantManager.GlobalVariables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
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
using RestaurantManager.ApplicationFiles;
using DatabaseModels.OrderTicket;
using DatabaseModels.Warehouse;
using DatabaseModels.CustomersManagement;
using DatabaseModels.WorkPeriod;

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for NewOrder.xaml
    /// </summary>
    public partial class Salespoint : Page
    {
        Brush defaultbuttonbrush = null;
        readonly Random R = new Random();
        private readonly ObservableCollection<OrderItem> OrderItems;
        private ObservableCollection<ProductCategory> Category_Items;


        //54603228237,,,,0703070707
        public Salespoint()
        {
            InitializeComponent();
            defaultbuttonbrush = Button_RestaurantDept.Background;
            OrderItems = new ObservableCollection<OrderItem>();
            Category_Items = new ObservableCollection<ProductCategory>();
            Grid_EditTicketHeader.Visibility = Visibility.Collapsed;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
              
                Button_RestaurantDept_Click(Button_RestaurantDept, new RoutedEventArgs());
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetMenuItems(string department)
        {
            try
            {

                using (var db = new PosDbContext())
                {
                    var a = db.ProductCategory.AsNoTracking().Where(k=>k.Department.ToLower()==department.ToLower()).ToList();
                    foreach (var x in a)
                    {
                        x.MenuItems = db.MenuProductItem.Where(m => m.CategoryGuid == x.CategoryGuid).OrderBy(i=>i.ProductName).ToList();
                    } 
                    Category_Items= new ObservableCollection<ProductCategory>(a);
                    Categories_ListView.ItemsSource = Category_Items;
                }
                Datagrid_OrderItems.ItemsSource = OrderItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView Lv = sender as ListView;
            try
            { 
                if (Lv.SelectedItem == null)
                {
                    return;
                } 
                //get bought item
                MenuProductItem mpi = (MenuProductItem)Lv.SelectedItem;
                AddNewItemToOrder(mpi);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Lv.SelectedItem = null;
            }
        }
     
        private void AddNewItemToOrder(MenuProductItem mpii)
        {
            try
            {  
                decimal Sellingprice = 0; 
                decimal Buyingprice = 0;
                int icount = 0;
                MenuProductItem mpi = mpii;
                ServiceType st = new ServiceType();
                st.Topmost = true;
                if ((bool)st.ShowDialog())
                {
                    Buyingprice = mpi.BuyingPrice;
                    if (st.ItemServiceType == "Out")
                    {
                        Sellingprice = mpi.TotalCost;
                        icount = st.ItemQty;
                    }
                    else
                    {
                        Sellingprice = mpi.SellingPrice;
                        icount = st.ItemQty;
                    }
                }
                else
                { 
                    return;
                }
                //check if the Item has discount(gift or pricediscount)
                DiscountItem disc_i = null;
                using (var db = new PosDbContext())
                {
                    var date = SharedVariables.CurrentDate();
                    var r = db.DiscountItem.AsNoTracking().FirstOrDefault(k => k.ProductGuid == mpi.ProductGuid && k.DiscStatus == "Active" && k.StartDate <= date && k.EndDate >= date);
                    if (r != null)
                    {
                        if (r.IsRepetitive)
                        {
                            if (date.DayOfWeek.ToString().ToLower() == r.OfferDay.ToLower())
                            {
                                disc_i = r;
                            }
                        }
                        else
                        {
                            disc_i = r;
                        }

                    }
                }

                //set item order
                OrderItem i = new OrderItem();
                using (var db = new PosDbContext())
                {
                    var a = db.MenuProductItem.AsNoTracking().Where(o => o.ProductGuid == mpi.ProductGuid);
                    if (a.Count() > 0)
                    {
                        MenuProductItem b = a.First();
                        i.ItemRowGuid = Guid.NewGuid().ToString();
                        i.ProductItemGuid = b.ProductGuid;
                        i.ItemName = b.ProductName;
                        i.Quantity = icount;
                        i.Price = Sellingprice;
                        i.Total = Sellingprice * icount;
                        i.BuyingPriceTotal = Buyingprice * icount;
                        i.BuyingPrice = Buyingprice;
                        i.ServiceType = st.ItemServiceType;
                        i.IsItemVoided = false;
                        i.IsGiftItem = false;
                        i.DiscPercent = 0;
                        i.GiftItemGuid = "None";
                        i.ParentItemGuid = "None";
                        if (disc_i != null)
                        {
                            if (disc_i.DiscType == "PricePercentage")
                            {
                                i.DiscPercent = disc_i.DiscPercentage;
                            }
                            else if (disc_i.DiscType == "GiftItem")
                            {
                                i.GiftItemGuid = disc_i.AttachedProduct;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("The Item does not Exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                       
                        return;
                    }
                }

                if (OrderItems.Where(a => a.ProductItemGuid == i.ProductItemGuid && a.ServiceType == i.ServiceType&& a.IsGiftItem==false).Count() > 0)
                {
                    MessageBox.Show("The Product is in the Order list!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                     
                    return;
                }
                //finally add item
                OrderItems.Add(i);
                //add gift item if present
                if (disc_i != null)
                {
                    if (disc_i.DiscType == "GiftItem")
                    {
                        var db1 = new PosDbContext();
                        var giftoriginalproduct = db1.MenuProductItem.FirstOrDefault(k => k.ProductGuid == disc_i.AttachedProduct&&k.ProductGuid==i.GiftItemGuid);
                        OrderItem gift = new OrderItem
                        {
                            ItemRowGuid = Guid.NewGuid().ToString(),
                            ProductItemGuid = disc_i.AttachedProduct,//real product guid
                            ItemName = giftoriginalproduct.ProductName + " - Gift",
                            Quantity = icount,
                            Price = giftoriginalproduct.TotalCost,
                            Total = icount* giftoriginalproduct.TotalCost,
                            ServiceType = st.ItemServiceType,
                            IsItemVoided = false,
                            IsGiftItem = true,
                            DiscPercent = 0,
                            GiftItemGuid = "None",
                            ParentItemGuid = disc_i.ProductGuid
                        };
                        OrderItems.Add(gift);
                    }
                }

                CalculateTotal(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Datagrid_OrderItems_MouseUp(object sender, MouseButtonEventArgs e)
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
                    if (Datagrid_OrderItems.SelectedItem == null)
                    {
                        return;
                    }
                    OrderItem o = (OrderItem)Datagrid_OrderItems.SelectedItem;
                    UpdateDeleteOrderItem(o);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateDeleteOrderItem(OrderItem item)
        {
            try
            {
                EditOrderItemQuantity ei = new EditOrderItemQuantity()
                {
                    Title = item.ItemName
                };
                if (item.IsGiftItem)
                {
                    MessageBox.Show("You cannot Edit a Gift!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                ei.TextBox_Quantity.Text = item.Quantity.ToString();
                ei.ShowDialog();
                if (ei.ReturningAction == "Delete")
                {
                    OrderItem x = OrderItems.Where(h => h.ProductItemGuid == item.GiftItemGuid&&h.ParentItemGuid==item.ProductItemGuid && h.ServiceType == item.ServiceType && h.IsGiftItem == true).FirstOrDefault();
                    if (x != null)
                    {
                        OrderItems.Remove(x);
                    }
                    OrderItems.Remove(item);
                }
                else if (ei.ReturningAction == "Update")
                {
                    decimal total = (decimal)ei.ReturningQuantity * item.Price;
                    OrderItems.Where(h => h.ProductItemGuid == item.ProductItemGuid && h.ServiceType == item.ServiceType).First().Quantity = ei.ReturningQuantity;
                    OrderItems.Where(h => h.ProductItemGuid == item.ProductItemGuid && h.ServiceType == item.ServiceType).First().Total = total;
                    //update gifts quantity also
                    OrderItem x = OrderItems.Where(h => h.ProductItemGuid == item.GiftItemGuid&&h.ParentItemGuid == item.ProductItemGuid && h.ServiceType == item.ServiceType && h.IsGiftItem == true).FirstOrDefault();
                    if (x != null)
                    {
                        OrderItems.Where(h => h.ProductItemGuid == item.GiftItemGuid && h.ParentItemGuid == item.ProductItemGuid && h.ServiceType == item.ServiceType && h.IsGiftItem == true).FirstOrDefault().Quantity = ei.ReturningQuantity;
                    } 

                    Datagrid_OrderItems.Items.Refresh();
                }
                CalculateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculateTotal()
        {
            try
            { 
                //update every item total in the list
                foreach (var a in OrderItems)
                {
                    a.Total = a.Quantity * a.Price;
                }
                //find sum of all items total
                decimal t = 0;
                var chargeditems = OrderItems.Where(k => k.IsGiftItem == false);
                foreach (var a in chargeditems)
                { 
                    t += a.Total * ((100 - a.DiscPercent) / 100);
                }
                Textbox_TotalAmount.Text = t.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetForm()
        {
            try
            {
                OrderItems.Clear();
                Textbox_TotalAmount.Text = "0.00";
                Label_Table.Content = "";
                LabelCustomer.Content = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Complete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to Make an Order ?", "Message Box", MessageBoxButton.YesNo, MessageBoxImage.Question)==MessageBoxResult.Yes)
                {
                    int total = 0;
                    WorkPeriod w;
                    using (var db = new PosDbContext())
                    {
                        w = db.WorkPeriod.Where(x => x.WorkperiodStatus == PosEnums.WorkPeriodStatuses.Open.ToString()).First();
                        if (w == null)
                        {
                            MessageBox.Show("There is No Work Period Open!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }
                    string ordno = "T" + SharedVariables.CurrentDate().ToString("ddmmyy") + "-" + R.Next(0, 999).ToString();
                    Customer cust = GetCustomer();
                    OrderMaster om = new OrderMaster
                    {
                        OrderGuid = Guid.NewGuid().ToString(),
                        OrderDate = SharedVariables.CurrentDate(),
                        CustomerRefference = cust != null ? cust.PhoneNumber : "None",
                        TicketTable = Label_Table.Content.ToString(),
                        OrderStatus = PosEnums.OrderTicketStatuses.Pending.ToString(),
                        UserServing = SharedVariables.CurrentUser.UserName,
                        PaymentDate = SharedVariables.CurrentDate(),
                        IsPrinted = false,
                        OrderNo = ordno,
                        VoidReason = "None",
                        Workperiod = w.WorkperiodName
                    };
                    foreach (var a in OrderItems)
                    {
                        a.OrderID = om.OrderNo;
                    }
                    foreach (var x in OrderItems)
                    {
                        total += (int)(x.Quantity * x.Price);
                    }
                    using (var db = new PosDbContext())
                    {
                        db.OrderMaster.Add(om);
                        db.OrderItem.AddRange(OrderItems);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Order Sent Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    OrderItems.Clear();
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 

        private Customer GetCustomer()
        {
            try
            {
                if (LabelCustomer.Tag!=null)
                {
                    if (LabelCustomer.Tag.GetType() == typeof(Customer))
                    {
                        if ((Customer)LabelCustomer.Tag != null)
                        {
                            return (Customer)LabelCustomer.Tag;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
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

        private void Button_SelectTable_Click(object sender, RoutedEventArgs e)
        {
            SelectTableForTicket st = new SelectTableForTicket();
            st.ShowDialog();
            Label_Table.Tag = st.SelectedTable;
            Label_Table.Content = st.SelectedTable;
        }

        private void Button_SelectCustomer_Click(object sender, RoutedEventArgs e)
        {
            SelectCustomerName sc = new SelectCustomerName();
            sc.ShowDialog();
            if (sc.SelectedCustomer != null)
            {
                LabelCustomer.Tag = sc.SelectedCustomer;
                LabelCustomer.Content = sc.SelectedCustomer.CustomerName;
            }
            else
            {
                LabelCustomer.Tag = null;
                LabelCustomer.Content = "";
            }
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            try
            {
                Expander exp = sender as Expander;
                foreach (var x in Category_Items)
                {
                    if (x.CategoryGuid != exp.Tag.ToString())
                    {
                        x.IsSelected = false;
                    }
                } 
                Categories_ListView.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_RestaurantDept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = sender as Button;
                Button_Services.Background = defaultbuttonbrush;
                Button_BarDept.Background = defaultbuttonbrush;
                b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF29400");
                GetMenuItems(PosEnums.Departments.Restaurant.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_BarDept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = sender as Button;
                Button_RestaurantDept.Background = defaultbuttonbrush;
                Button_Services.Background = defaultbuttonbrush;
                b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF29400");
                GetMenuItems(PosEnums.Departments.Bar.ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                var db = new PosDbContext();
                var item = db.MenuProductItem.FirstOrDefault(k => k.ProductGuid == button.Tag.ToString()) ;
                if (item== null)
                {
                    return;
                } 
                AddNewItemToOrder(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }

        private void Button_Services_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = sender as Button;
                Button_RestaurantDept.Background = defaultbuttonbrush;
                Button_BarDept.Background = defaultbuttonbrush;
                b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF29400");
                GetMenuItems(PosEnums.Departments.Services.ToString());


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
