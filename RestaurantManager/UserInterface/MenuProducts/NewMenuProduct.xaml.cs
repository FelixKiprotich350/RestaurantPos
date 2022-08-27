using RestaurantManager.BusinessModels.Menu;
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

namespace RestaurantManager.UserInterface.MenuProducts
{
    /// <summary>
    /// Interaction logic for NewMenuProduct.xaml
    /// </summary>
    public partial class NewMenuProduct : Window
    {
        public NewMenuProduct()
        {
            InitializeComponent();
        }

        private void Button_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal price = 0;
                string category = "";
                if (Combobox_Category.SelectedItem == null)
                {
                    return;
                }
                ProductCategory productCategory = (ProductCategory)Combobox_Category.SelectedItem;
                category = productCategory.CategoryGuid;
                using (var db = new PosDbContext())
                {
                    db.MenuProductItem.Add(new MenuProductItem() { ProductGuid = Guid.NewGuid().ToString(), ProductName = Textbox_ProductName.Text, AvailabilityStatus = "Available", Price = price, CategoryGuid = category });
                    db.SaveChanges();
                    MessageBox.Show("Success. Item Saved.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    Combobox_Category.ItemsSource = db.ProductCategory.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
