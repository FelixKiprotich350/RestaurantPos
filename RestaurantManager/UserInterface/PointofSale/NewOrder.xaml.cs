﻿using RestaurantManager.BusinessModels.Menu;
using RestaurantManager.BusinessModels.PointofSale;
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

        public NewOrder()
        {
            InitializeComponent();
            OrderItems = new ObservableCollection<OrderItem>();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var b = new PosDbContext())
                {
                    var a = b.ProductCategory.ToList();
                    a.ForEach(s => s.GetMenuItems(s.CategoryGuid));
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
                OrderItems.Add(i);
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
                string ordno = ErpShared.CurrentDate().ToString("ddmmyyyy")+ "-" + R.Next(0, 999).ToString();
                OrderMaster om = new OrderMaster
                {
                    OrderGuid = Guid.NewGuid().ToString(),
                    OrderDate = ErpShared.CurrentDate(),
                    OrderStatus = "Pending",
                    User =  ErpShared.CurrentUser.UserName,
                    PaymentDate = ErpShared.CurrentDate(),
                    OrderNo = ordno
                };
                foreach (var a in OrderItems)
                {
                    a.OrderID = om.OrderNo; 
                }
                using (var db = new PosDbContext())
                {
                    db.Database.Log = s => Trace.WriteLine(s);
                    db.OrderMaster.Add(om);
                    db.OrderItem.AddRange(OrderItems);
                    db.SaveChanges();
                }
                MessageBox.Show("Order Sent Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                OrderItems.Clear();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
