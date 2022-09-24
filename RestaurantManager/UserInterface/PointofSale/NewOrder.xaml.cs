﻿using RestaurantManager.BusinessModels.CustomersManagement;
using RestaurantManager.BusinessModels.Warehouse;
using RestaurantManager.BusinessModels.OrderTicket; 
using RestaurantManager.BusinessModels.WorkPeriod;
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

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for NewOrder.xaml
    /// </summary>
    public partial class NewOrder : Page
    {
        readonly Random R = new Random();
        private readonly ObservableCollection<OrderItem> OrderItems;
        //54603228237,,,,0703070707
        public NewOrder()
        {
            InitializeComponent();
            OrderItems = new ObservableCollection<OrderItem>(); 
        }
        //R2711220900172
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            { 
                GetMenuItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetMenuItems()
        {
            try
            {

                using (var b = new PosDbContext())
                {
                    var a = b.ProductCategory.ToList();
                    a.ForEach(s => s.GetAllMenuItems(s.CategoryGuid,true));
                    Categories_ListView.ItemsSource = a;
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
            try
            {
                ListView Lv = sender as ListView;
                if (Lv.SelectedItem == null)
                {
                    return;
                }
                MenuProductItem mpi = (MenuProductItem)Lv.SelectedItem;
                OrderItem i = new OrderItem();
                using (var db = new PosDbContext())
                {
                    var a = db.MenuProductItem.Where(o => o.ProductGuid == mpi.ProductGuid);
                    if (a.Count() > 0)
                    {
                        MenuProductItem b = a.First();
                        i.ItemRowGuid = Guid.NewGuid().ToString();
                        i.ParentProductItemGuid = b.ProductGuid;
                        i.ItemName = b.ProductName; 
                        i.Quantity = 1;
                        i.Price = b.Price;
                        i.Total = b.Price;
                        i.ServiceType = "In";
                    }
                    else
                    {
                        MessageBox.Show("The Item does not Exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                if (OrderItems.Where(a => a.ParentProductItemGuid == i.ParentProductItemGuid).Count() > 1)
                {
                    MessageBox.Show("The Product has been added to the Order before!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                OrderItems.Add(i);
                CalculateTotal();
                Lv.SelectedItem = null;
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
                    EditOrderItemQuantity ei = new EditOrderItemQuantity()
                    {
                        Title = o.ItemName
                    };
                    ei.TextBox_Quantity.Text = o.Quantity.ToString();
                    ei.ShowDialog();
                    if (ei.ReturningAction == "Delete")
                    {
                        OrderItems.Remove(o);
                    }
                    else if (ei.ReturningAction == "Update")
                    {
                        decimal total = (decimal)ei.ReturningQuantity * o.Price;
                        OrderItems.Where(h => h.ParentProductItemGuid == o.ParentProductItemGuid).First().Quantity = ei.ReturningQuantity;
                        OrderItems.Where(h => h.ParentProductItemGuid == o.ParentProductItemGuid).First().ServiceType = ei.ItemServiceType;
                        OrderItems.Where(h => h.ParentProductItemGuid == o.ParentProductItemGuid).First().Total = total;
                        Datagrid_OrderItems.Items.Refresh();
                    }
                    CalculateTotal();
                }
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
                decimal t = 0;
                foreach (var a in OrderItems)
                {
                    t += (decimal)a.Total;
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
                        Workperiod = w.WorkperiodName,
                        IsKitchenServed = false,
                        IsInPreparation = false
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
    }

}
