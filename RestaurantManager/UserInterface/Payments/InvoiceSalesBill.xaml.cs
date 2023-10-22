using DatabaseModels.CRM;
using RestaurantManager.GlobalVariables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace RestaurantManager.UserInterface.Payments
{
    /// <summary>
    /// Interaction logic for InvoiceSalesBill.xaml
    /// </summary>
    public partial class InvoiceSalesBill : Window
    {
        public ObservableCollection<CustomerAccount> billableaccounts = new ObservableCollection<CustomerAccount>();

        public InvoiceSalesBill()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Datagrid_billableaccounts.ItemsSource = billableaccounts;
                RefreshAccountsList();
                Textbox_SelectedCustomerPhone.Text = "";
                Textbox_SelectedCustomerPhone.Tag = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void RefreshAccountsList()
        {
            try
            {
                billableaccounts.Clear(); 
                var db = new PosDbContext();
                List<CustomerAccount> rawlist = db.CustomerAccount.AsNoTracking().Where(p => p.AccountStatus == PosEnums.PersonAccountStatus.Active.ToString()).ToList();
                foreach (var x in rawlist)
                {
                    x.FullName = db.PersonalAccount.First(k=>k.AccountNo==x.PersonAccNo).FullName;
                    x.PhoneNo = db.PersonalAccount.First(k => k.AccountNo == x.PersonAccNo).PhoneNumber;
                    x.Gender = db.PersonalAccount.First(k => k.AccountNo == x.PersonAccNo).Gender;
                    billableaccounts.Add(x);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Textbox_SearchCustomer_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox t = (TextBox)sender;
                string filter = t.Text;
                if (Datagrid_billableaccounts.ItemsSource == null)
                {
                    return;
                }
                ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_billableaccounts.ItemsSource);
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

        public bool Contains(object de)
        {
           PersonalAccount  item = de as PersonalAccount;
            return item.AccountNo.ToLower().Contains(Textbox_SearchAccount.Text.ToLower()) | item.FullName.ToLower().Contains(Textbox_SearchAccount.Text.ToLower());

        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Accept_Click(object sender, RoutedEventArgs e)
        {
            var acc = (CustomerAccount)Textbox_SelectedCustomerPhone.Tag;
            if (acc!=null)
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("The account does not Exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Datagrid_billableaccounts_MouseUp(object sender, MouseButtonEventArgs e)
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
                    if (Datagrid_billableaccounts.SelectedItem == null)
                    {
                        return;
                    }
                    //BillableAccount acc = (BillableAccount)Datagrid_billableaccounts.SelectedItem; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Datagrid_billableaccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CustomerAccount acc = (CustomerAccount)Datagrid_billableaccounts.SelectedItem;
                if (acc != null)
                { 
                    Textbox_SelectedCustomerPhone.Text = acc.FullName;
                    Textbox_SelectedCustomerPhone.Tag = acc;
                }
                else
                {
                    Textbox_SelectedCustomerPhone.Text = "";
                    Textbox_SelectedCustomerPhone.Tag = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
