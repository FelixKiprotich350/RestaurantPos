using DatabaseModels.Accounts;
using DatabaseModels.OrderTicket;
using RestaurantManager.ActivityLogs;
using RestaurantManager.ApplicationFiles;
using RestaurantManager.GlobalVariables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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
                Datagrid_InvoiceList.ItemsSource = null;
                List<InvoicesMaster> item = new List<InvoicesMaster>(); 
                using (var db = new PosDbContext())
                { 
                    item = db.InvoicesMaster.AsNoTracking().ToList();
                } 
                Datagrid_InvoiceList.ItemsSource = item;
                TextBox_ProductsCount.Text = Datagrid_InvoiceList.Items.Count.ToString();
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
                if (Datagrid_InvoiceList.ItemsSource == null)
                {
                    return;
                }
                ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_InvoiceList.ItemsSource);
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

        private void Button_Reject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 
                if (MessageBox.Show("Are you sure you want to Reject This Invoice ?. IRREVERSIBLE PROCESS!","MESSAGE BOX",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
                {
                    var db = new PosDbContext();
                    var inv = db.InvoicesMaster.FirstOrDefault(k => k.InvoiceNo == Textbox_InvoiceNumber.Text);
                    if (inv != null)
                    {
                        inv.InvoiceStatus = GlobalVariables.PosEnums.InvoiceStatuses.Rejected.ToString();
                        db.SaveChanges();
                        MessageBox.Show("The Invoice has been Rejected Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Rejected invoice", "invoice no=" + inv.InvoiceNo);
                        RefreshInvoices();

                    }

                }
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
                if (MessageBox.Show("Are you sure you want to APPROVE This Invoice ?. IRREVERSIBLE PROCESS!", "MESSAGE BOX", MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var db = new PosDbContext();
                    var inv = db.InvoicesMaster.FirstOrDefault(k => k.InvoiceNo == Textbox_InvoiceNumber.Text);
                    if (inv != null)
                    {
                         
                        inv.InvoiceStatus = GlobalVariables.PosEnums.InvoiceStatuses.Approved.ToString();
                        
                        db.SaveChanges() ;
                        MessageBox.Show("The Invoice has been Approved Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Approved Invoice", "invoice no=" + inv.InvoiceNo); 
                        RefreshInvoices(); 
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Datagrid_TicketsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ResetInvoiceDetails();
                //var db = new PosDbContext();
                //var items = db.OrderItem.AsNoTracking().Where(k=>k.OrderID==om.OrderNo).ToList();
                if (Datagrid_InvoiceList.SelectedItem is InvoicesMaster om)
                {
                    Textbox_InvoiceNumber.Text = om.InvoiceNo;
                    Textbox_postedby.Text = om.SystemUser;
                    Textbox_Status.Text = om.InvoiceStatus;
                    Textbox_Date.Text = om.InvoiceDate.ToString();
                    Textbox_Amount.Text = om.InvoiceAmount.ToString("N2");
                    if (om.InvoiceStatus == GlobalVariables.PosEnums.InvoiceStatuses.Issued.ToString())
                    {
                        Button_Approve.Visibility = Visibility.Visible;
                        Button_Reject.Visibility = Visibility.Visible;
                    } 
                    else if (om.InvoiceStatus == GlobalVariables.PosEnums.InvoiceStatuses.Rejected.ToString())
                    {
                        Button_Approve.Visibility = Visibility.Visible;
                    }
                    else if (om.InvoiceStatus == GlobalVariables.PosEnums.InvoiceStatuses.Approved.ToString())
                    {
                        Button_Reject.Visibility = Visibility.Visible;
                    }
                    else 
                    {
                        Button_Approve.Visibility = Visibility.Collapsed;
                        Button_Reject.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       private void ResetInvoiceDetails()
        {
            Textbox_InvoiceNumber.Text = "";
            Textbox_postedby.Text = "";
            Textbox_Status.Text = "";
            Textbox_Date.Text = "";
            Textbox_Amount.Text = "";
            Button_Approve.Visibility = Visibility.Collapsed;
            Button_Reject.Visibility = Visibility.Collapsed;
            Datagrid_Invoicepayments.ItemsSource = null;
        }
    }
}
