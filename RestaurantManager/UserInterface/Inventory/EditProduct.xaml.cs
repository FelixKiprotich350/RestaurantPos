using DatabaseModels.Warehouse;
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
using System.Windows.Shapes;

namespace RestaurantManager.UserInterface.Inventory
{
    /// <summary>
    /// Interaction logic for EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Window
    {
        readonly MenuProductItem pitem = null;
        public EditProduct(MenuProductItem p)
        {
            InitializeComponent();
            pitem = p;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (pitem is null)
                {
                    Close();
                }
                using (var db = new PosDbContext())
                {
                    Combobox_Category.ItemsSource = db.ProductCategory.ToList();
                }
                Textbox_Productname.Text = pitem.ProductName;
                Textbox_BuyingPrice.Text = pitem.BuyingPrice.ToString();
                Textbox_ProductPrice.Text = pitem.SellingPrice.ToString();
                Textbox_ProductPackagePrice.Text = pitem.PackagingCost.ToString();
                Combobox_Status.SelectedItem = pitem.AvailabilityStatus;
                Combobox_Category.SelectedItem = Combobox_Category.Items.Cast<ProductCategory>().FirstOrDefault(x => x.CategoryGuid == pitem.CategoryGuid);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_Productname.Text=="")
                {
                    MessageBox.Show("Enter the Name of the Product!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Textbox_Productname.Focus();
                    return;
                }
                if (Combobox_Category.SelectedItem==null)
                {
                    MessageBox.Show("Select the Product Category!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Combobox_Category.Focus();
                    Combobox_Category.IsDropDownOpen = true;
                    return;
                }
                if (Combobox_Status.SelectedItem==null)
                {
                    MessageBox.Show("Select the Product Status!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Combobox_Status.Focus();
                    Combobox_Status.IsDropDownOpen = true;
                    return;
                }
                if (!decimal.TryParse(Textbox_BuyingPrice.Text,out decimal buyingprice))
                {
                    MessageBox.Show("Enter correct value for the Buying  Price!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Textbox_ProductPrice.Focus(); 
                    return;
                }
                if (!decimal.TryParse(Textbox_ProductPrice.Text,out decimal price))
                {
                    MessageBox.Show("Enter correct value for the Product Price!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Textbox_ProductPrice.Focus(); 
                    return;
                }
                if (!decimal.TryParse(Textbox_ProductPackagePrice.Text,out decimal packagingprice))
                {
                    MessageBox.Show("Enter correct value for the Product Packaging Price!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Textbox_ProductPrice.Focus(); 
                    return;
                }
                using (var db = new PosDbContext())
                {
                    MenuProductItem item = db.MenuProductItem.FirstOrDefault(k=>k.ProductGuid==pitem.ProductGuid);
                    if (item != null)
                    {
                        ProductCategory pc = Combobox_Category.SelectedItem as ProductCategory;
                        item.ProductName = Textbox_Productname.Text;
                        item.AvailabilityStatus = Combobox_Status.SelectedItem.ToString();
                        item.CategoryGuid = pc.CategoryGuid;
                        item.CategoryName = pc.CategoryName;
                        item.BuyingPrice = buyingprice;
                        item.SellingPrice = price;
                        item.PackagingCost = packagingprice;
                        item.TotalCost = price + packagingprice;
                        item.Department = pc.Department;
                        db.SaveChanges();
                        MessageBox.Show("Product Updated Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                } 

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
    }
}
