using DatabaseModels.Accounts;
using DatabaseModels.OrderTicket;
using RestaurantManager.ApplicationFiles; 
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
    public partial class ViewInvoicesmaster : Page
    {
        public ViewInvoicesmaster()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshInvoices();
        }

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshInvoices();
            MessageBox.Show("Refresh Success. Done.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void RefreshInvoices()
        {
            try
            {
                List<InvoicesMaster> item = new List<InvoicesMaster>(); 
                using (var db = new PosDbContext())
                { 
                    item = db.InvoicesMaster.ToList();
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
                    var db = new PosDbContext();
                    InvoicesMaster om = Datagrid_TicketsList.SelectedItem as InvoicesMaster;
                    //var items = db.OrderItem.AsNoTracking().Where(k=>k.OrderID==om.OrderNo).ToList();
                    Textbox_InvoiceNumber.Text = om.InvoiceNo;
                    Textbox_postedby.Text = om.SystemUser;
                    Textbox_Status.Text = om.InvoiceStatus;
                    Textbox_Date.Text = om.InvoiceDate.ToString(); 
                    Textbox_Workperiodd.Text = om.Workperiod.ToString();
                   // Datagrid_TicketItems.ItemsSource = items;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
         
        public bool Contains(object de)
        {
            InvoicesMaster item = de as InvoicesMaster;
            return item.InvoiceNo.ToLower().Contains(Textbox_TicketSearchBox.Text.ToLower()) | item.InvoiceStatus.ToLower().Contains(Textbox_TicketSearchBox.Text.ToLower())| item.Workperiod.ToLower().Contains(Textbox_TicketSearchBox.Text.ToLower())| item.InvoiceDate.ToString().ToLower().Contains(Textbox_TicketSearchBox.Text.ToLower());

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

        private void Button_Approve_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ApproveInvoice a = new ApproveInvoice();
                if ((bool)a.ShowDialog())
                {
                    var db = new PosDbContext();
                   var inv= db.InvoicesMaster.FirstOrDefault(k=>k.InvoiceNo== Textbox_InvoiceNumber.Text);
                    if (inv != null)
                    {
                        inv.InvoiceStatus = GlobalVariables.PosEnums.InvoiceStatuses.Approved.ToString();
                    }
                    db.SaveChanges();
                    MessageBox.Show("The Invoice has been Approved Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
