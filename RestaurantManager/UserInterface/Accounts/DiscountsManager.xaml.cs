using DatabaseModels.Vouchers;
using DatabaseModels.Inventory;
using RestaurantManager.ApplicationFiles; 
using System;
using System.Collections.Generic;
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
using static RestaurantManager.GlobalVariables.PosEnums;

namespace RestaurantManager.UserInterface.Accounts
{
    /// <summary>
    /// Interaction logic for GenerateVouchers.xaml
    /// </summary>
    public partial class DiscountsManager : Page
    {
        readonly Random R = new Random();
        public DiscountsManager()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBatchVoucher();
        }

        private void LoadBatchVoucher()
        {
            try
            {
                ComboBox_VoucherType.Items.Clear();
                var x = Enum.GetValues(typeof(GlobalVariables.PosEnums.VoucherTypes)).Cast<VoucherTypes>().ToList();
                foreach (var y in x)
                {
                    if (y != VoucherTypes.CustomerRedeemed)
                    {
                        ComboBox_VoucherType.Items.Add(y);
                    }
                   
                }
                 using (var db = new PosDbContext())
                {
                    var data = db.DiscountVoucher.ToList();
                    Datagrid_Vouchers.ItemsSource = data;

                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 

        private void Button_CreateVouchers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!decimal.TryParse(TextBox_VoucherAmount.Text, out decimal VoucherAmount))
                {
                    MessageBox.Show("Invalid Discount Amount!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                decimal BulkLimit = 0;
                if (ComboBox_VoucherType.SelectedItem.ToString() == VoucherTypes.ProductDiscount.ToString())
                {
                    BulkLimit = 0;
                    if (ListView_ProductstoDiscount.Items.Count <= 0)
                    {
                        MessageBox.Show("You must select atleast one Product to Discount!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else if (ComboBox_VoucherType.SelectedItem.ToString() == VoucherTypes.BulkSales.ToString())
                {

                    if (!decimal.TryParse(TextBox_BulkSalesLimit.Text, out BulkLimit))
                    {
                        MessageBox.Show("Invalid Sales Limit Amount!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                if (DatePicker_StartDate.SelectedDate == null | DatePicker_EndDate == null)
                {
                    MessageBox.Show("You must select the StartDate and EndDate!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                DateTime dtime = GlobalVariables.SharedVariables.CurrentDate();
                DiscountVoucher v = new DiscountVoucher
                {
                    BatchGuid = Guid.NewGuid().ToString(),
                    BatchNumber = R.Next(100000, 999999).ToString(),
                    VoucherType = ComboBox_VoucherType.SelectedItem.ToString(),
                    CreatedBy = GlobalVariables.SharedVariables.CurrentUser.UserName,
                    VoucherAmount = VoucherAmount, 
                    BatchDescription = TextBox_BatchDescription.Text,
                    BulkSalesLimitAmount=BulkLimit,
                    IsActiveStatus=true,
                    CreationDate = dtime,
                    StartDate = (DateTime)DatePicker_StartDate.SelectedDate,
                    EndDate = (DateTime)DatePicker_EndDate.SelectedDate
                };  
                using (var db = new PosDbContext())
                {
                    if (ComboBox_VoucherType.SelectedItem.ToString() == VoucherTypes.ProductDiscount.ToString())
                    {
                        var ite = ListView_ProductstoDiscount.Items.Cast<MenuProductItem>().ToList();
                        foreach(var x in ite)
                        {
                           // db.DiscountedItem.Add(new DiscountedItem() { ItemRowGuid = Guid.NewGuid().ToString(), ProductItemGuid = x.ProductGuid, BatchNumber = v.BatchNumber });
                        }
                    }
                    db.DiscountVoucher.Add(v);
                    db.SaveChanges();
                }
                MessageBox.Show("Successfully Saved!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadBatchVoucher();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }  

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadBatchVoucher();
        }

        private void ComboBox_VoucherType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Ugrid_Productitem.Visibility = Visibility.Collapsed;
                Ugrid_BulkSales.Visibility = Visibility.Collapsed;
                if (ComboBox_VoucherType.SelectedItem.ToString() == VoucherTypes.ProductDiscount.ToString())
                {
                    Ugrid_Productitem.Visibility = Visibility.Visible;
                }
                else if (ComboBox_VoucherType.SelectedItem.ToString() == VoucherTypes.BulkSales.ToString())
                {
                    Ugrid_BulkSales.Visibility = Visibility.Visible;
                    ListView_ProductstoDiscount.Items.Clear();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_SelectProducts_Click(object sender, RoutedEventArgs e)
        {
            //SelectDiscountProducts s = new SelectDiscountProducts();
            //s.ShowDialog();
            //if ((bool)s.DialogResult)
            //{
            //    ListView_ProductstoDiscount.ItemsSource = s.Products.Where(k=>k.IsSelected);
            //}
        }
    }
}
