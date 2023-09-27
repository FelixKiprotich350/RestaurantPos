using DatabaseModels.Warehouse;
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

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for PriceList.xaml
    /// </summary>
    public partial class PriceList : Page
    {
        public PriceList()
        {
            InitializeComponent();
        }

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Textbox_SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox t = (TextBox)sender;
                string filter = t.Text;
                if (Datagrid_ProductItems.ItemsSource == null)
                {
                    return;
                }
                ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_ProductItems.ItemsSource);
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
            MenuProductItem item = de as MenuProductItem;
            return item.ProductName.ToLower().Contains(Textbox_SearchBox.Text.ToLower()) | item.CategoryName.ToLower().Contains(Textbox_SearchBox.Text.ToLower());

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
        private void RefreshMenuProducts()
        {
            try
            {
                List<ProductCategory> cat = new List<ProductCategory>();
                List<MenuProductItem> item = new List<MenuProductItem>();
                using (var db = new PosDbContext())
                {
                    cat = db.ProductCategory.ToList();
                    item = db.MenuProductItem.ToList();
                }
                foreach (var x in item)
                {
                    x.CategoryName = cat.Where(y => y.CategoryGuid == x.CategoryGuid).FirstOrDefault().CategoryName;
                }
                Datagrid_ProductItems.ItemsSource = item;
                TextBox_ProductsCount.Text = Datagrid_ProductItems.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
