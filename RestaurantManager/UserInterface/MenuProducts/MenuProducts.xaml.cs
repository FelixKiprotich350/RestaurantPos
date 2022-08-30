using RestaurantManager.BusinessModels.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace RestaurantManager.UserInterface.MenuProducts
{
    /// <summary>
    /// Interaction logic for MenuProducts.xaml
    /// </summary>
    public partial class MenuProducts : Page
    {
        public MenuProducts()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshMenuProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        

        private void Textbox_SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox t = (TextBox)sender;
                string filter = t.Text;
                if (Datagrid_ProductItems.ItemsSource==null)
                {
                    return;
                }
                ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_ProductItems.ItemsSource);
                if (filter == "")
                    cv.Filter = null;
                else
                {
                    cv.Filter = o =>
                    {
                        MenuProductItem p = o as MenuProductItem;
                        return p.ProductName.ToLower().Contains(filter.ToLower());
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_NewProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewMenuProduct nmp = new NewMenuProduct();
                if (nmp.ShowDialog() == false)
                {
                    return;
                }
                string category = "";
                ProductCategory productCategory = (ProductCategory)nmp.Combobox_Category.SelectedItem;
                category = productCategory.CategoryGuid;
                if (!decimal.TryParse(nmp.Textbox_Price.Text.Trim(), out decimal price))
                {
                    MessageBox.Show("The Price value entered is not allowed!.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                using (var db = new PosDbContext())
                {
                    db.MenuProductItem.Add(new MenuProductItem() { ProductGuid = Guid.NewGuid().ToString(), ProductName = nmp.Textbox_ProductName.Text, AvailabilityStatus = "Available", Price = price, CategoryGuid = category });
                    db.SaveChanges();
                    MessageBox.Show("Success. Item Saved.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshMenuProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           

        }
        private void RefreshMenuProducts()
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    Datagrid_ProductItems.ItemsSource = db.MenuProductItem.ToList();
                }
                TextBox_ProductsCount.Text = Datagrid_ProductItems.Items.Count.ToString() ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshMenuProducts();
            MessageBox.Show("Refresh Success. Done.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
