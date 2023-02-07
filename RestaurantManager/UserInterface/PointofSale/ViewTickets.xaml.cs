using RestaurantManager.ApplicationFiles;
using RestaurantManager.BusinessModels.OrderTicket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ViewTickets.xaml
    /// </summary>
    public partial class ViewTickets : Page
    {
        public ViewTickets()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshMenuProducts();
        }

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshMenuProducts();
            MessageBox.Show("Refresh Success. Done.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void RefreshMenuProducts()
        {
            try
            {
                List<OrderMaster> item = new List<OrderMaster>(); 
                using (var db = new PosDbContext())
                { 
                    item = db.OrderMaster.ToList();
                } 
                Datagrid_TicketsList.ItemsSource = item;
                TextBox_ProductsCount.Text = Datagrid_TicketsList.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Datagrid_TicketsList_MouseUp(object sender, MouseButtonEventArgs e)
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
                if (dep is DataGridCell cell)
                {
                    if (cell.Column.DisplayIndex != 1)
                    {
                        return;
                    }
                    if (Datagrid_TicketsList.SelectedItem == null)
                    {
                        return;
                    }
                    //OrderMaster om = Datagrid_TicketsList.SelectedItem as OrderMaster;
                    //TrackTicketMaster t = new TrackTicketMaster(om);
                    //t.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
         
        public bool Contains(object de)
        {
            OrderMaster item = de as OrderMaster;
            return item.OrderNo.ToLower().Contains(Textbox_TicketSearchBox.Text.ToLower()) | item.OrderNo.ToLower().Contains(Textbox_TicketSearchBox.Text.ToLower());

        }

        private void Textbox_TicketSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox t = (TextBox)sender;
                string filter = t.Text;
                if (Datagrid_TicketsList.ItemsSource == null)
                {
                    return;
                }
                ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_TicketsList.ItemsSource);
                if (filter == "")
                {
                    cv.Filter = null;
                }
                else
                {
                    cv.Filter = new Predicate<object>(Contains);
                }
                //if (filter == "")
                //    cv.Filter = null;
                //else
                //{
                //    cv.Filter = o =>
                //    {
                //        MenuProductItem p = o as MenuProductItem;
                //        return p.ProductName.ToLower().Contains(filter.ToLower()) || p.AvailabilityStatus.ToString().ToLower().Contains(filter.ToLower());
                //    };
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
