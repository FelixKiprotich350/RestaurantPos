using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for SelectTicketToEdit.xaml
    /// </summary>
    public partial class SelectTicketToEdit : Window
    {
        public SelectTicketToEdit()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var b = new PosDbContext())
                {
                    var a = b.OrderMaster.Where(c => c.UserServing == ErpShared.CurrentUser.UserName && c.OrderStatus == "Pending").ToList();
                    Datagrid_Tickets.ItemsSource = a;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Message Box",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void Datagrid_Tickets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //DependencyObject dep = (DependencyObject)e.OriginalSource;

                //// iteratively traverse the visual tree
                //while ((dep != null) & !(dep is DataGridCell) & !(dep is DataGridColumnHeader))
                //{
                //    dep = VisualTreeHelper.GetParent(dep);
                //}

                //if (dep == null)
                //{
                //    return;
                //}
                //if (dep is DataGridCell)
                //{
                //    if (Datagrid_OrderItems.SelectedItem == null)
                //    {
                //        return;
                //    }
                //    OrderItem o = (OrderItem)Datagrid_OrderItems.SelectedItem;
                //    EditOrderItemQuantity ei = new EditOrderItemQuantity()
                //    {
                //        Title = o.ItemName
                //    };
                //    ei.TextBox_Quantity.Text = o.Quantity.ToString();
                //    ei.ShowDialog();
                //    if (ei.ReturningAction == "Delete")
                //    {
                //        OrderItems.Remove(o);
                //    }
                //    else if (ei.ReturningAction == "Update")
                //    {
                //        decimal total = (decimal)ei.ReturningQuantity * o.Price;
                //        OrderItems.Where(h => h.ParentProductItemGuid == o.ParentProductItemGuid).First().Quantity = ei.ReturningQuantity;
                //        OrderItems.Where(h => h.ParentProductItemGuid == o.ParentProductItemGuid).First().ServiceType = ei.ItemServiceType;
                //        OrderItems.Where(h => h.ParentProductItemGuid == o.ParentProductItemGuid).First().Total = total;
                //        Datagrid_OrderItems.Items.Refresh();
                //    }
                //    CalculateTotal();
                //}
                //using (var b = new PosDbContext())
                //{
                //    var a = b.OrderMaster.Where(c => c.UserServing == ErpShared.CurrentUser.UserName && c.OrderStatus == "Pending").ToList();
                //    Datagrid_Tickets.ItemsSource = a;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
