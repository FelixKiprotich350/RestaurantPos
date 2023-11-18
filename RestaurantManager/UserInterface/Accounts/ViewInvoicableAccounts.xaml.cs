using DatabaseModels.Accounts;
using DatabaseModels.OrderTicket;
using RestaurantManager.UserInterface.Accounts.InvoiceAccount ; 
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
    public partial class ViewInvoicableAccounts : Page
    {
        public ViewInvoicableAccounts()
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
                Datagrid_InvoicableAccounts.ItemsSource = null;
                List<InvoicableAccount> item = new List<InvoicableAccount>();
                var db = new PosDbContext();
                item = db.InvoicableAccount.AsNoTracking().ToList();

                foreach (var x in item)
                {
                    var person = db.PersonalAccount.FirstOrDefault(k=>k.AccountNo==x.PersonAccNo);
                    if (person != null)
                    {
                        x.FullName = person.FullName;
                        x.Gender = person.Gender;
                    }
                }
                Datagrid_InvoicableAccounts.ItemsSource = item;
                TextBox_ProductsCount.Text = Datagrid_InvoicableAccounts.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
                  
        public bool Contains(object de)
        {
            InvoicableAccount item = de as InvoicableAccount;
            return item.PersonAccNo.ToLower().Contains(Textbox_TicketSearchBox.Text.ToLower()) | item.AccountStatus.ToLower().Contains(Textbox_TicketSearchBox.Text.ToLower())| item.Gender.ToLower().Contains(Textbox_TicketSearchBox.Text.ToLower())| item.FullName.ToString().ToLower().Contains(Textbox_TicketSearchBox.Text.ToLower());

        }

        private void Textbox_TicketSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox t = (TextBox)sender;
                string filter = t.Text;
                if (Datagrid_InvoicableAccounts.ItemsSource == null)
                {
                    return;
                }
                ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_InvoicableAccounts.ItemsSource);
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
 
        private void Datagrid_TicketsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ResetInvoiceDetails();
                //var db = new PosDbContext();
                //var items = db.OrderItem.AsNoTracking().Where(k=>k.OrderID==om.OrderNo).ToList();
                if (Datagrid_InvoicableAccounts.SelectedItem is InvoicableAccount om)
                {
                    Textbox_AccountNumber.Text = om.PersonAccNo;
                    Textbox_FullName.Text = om.FullName;
                    Textbox_Status.Text = om.AccountStatus; 
                    GetInvoicesonaccount(om.PersonAccNo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetInvoicesonaccount(string accno)
        {
            try
            { 
                using (var db = new PosDbContext())
                {
                    var list = db.InvoicesMaster.AsNoTracking().Where(x => x.CustomerAccNo == accno).ToList();
                    decimal total = list.Sum(k => k.InvoiceAmount);
                    Textblock_TotalInvoice.Text = total.ToString("N2");
                    Datagrid_Invoicesonaccount.ItemsSource = list;
                } 
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetInvoiceDetails()
        {
            Textbox_FullName.Text = "";
            Textbox_AccountNumber.Text = ""; 
            Textbox_Status.Text = ""; 
            Datagrid_Invoicesonaccount.ItemsSource = null;
        }

        private void Button_ViewFullReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = new PosDbContext();
                var acc = db.InvoicableAccount.AsNoTracking().FirstOrDefault(x => x.PersonAccNo == Textbox_AccountNumber.Text);
                
                if (acc!=null)
                {
                    var person = db.PersonalAccount.FirstOrDefault(k => k.AccountNo == acc.PersonAccNo);
                    acc.FullName = (person != null) ? person.FullName : "Name not Found";
                    new FullAccountStatement(acc) { Owner=GlobalVariables.SharedVariables.Backend_MainWindow}.ShowDialog();
                }
                else
                {
                    MessageBox.Show("The Account does not Exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Button_Print_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
