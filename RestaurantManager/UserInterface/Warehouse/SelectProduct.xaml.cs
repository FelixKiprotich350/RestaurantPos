using RestaurantManager.BusinessModels.Warehouse;
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

namespace RestaurantManager.UserInterface.Warehouse
{
    /// <summary>
    /// Interaction logic for SelectProduct.xaml
    /// </summary>
    public partial class SelectProduct : Window
    {
        public List<MenuProductItem> AllProducts = new List<MenuProductItem>();
        public List<MenuProductItem> Returnlist = new List<MenuProductItem>();
        public SelectProduct()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db=new PosDbContext())
                {
                    AllProducts = db.MenuProductItem.AsNoTracking().ToList();
                }
                Datagrid_AllProductItems.ItemsSource = AllProducts;
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message,"Message Box",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void Button_Select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var r = AllProducts.Where(k => k.IsSelected).ToList();
                Returnlist = r; 
                DialogResult = true;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error); 
                DialogResult = false;
            }
            finally
            {
                Close();
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}
