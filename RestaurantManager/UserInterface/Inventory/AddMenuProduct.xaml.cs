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

namespace RestaurantManager.UserInterface.Inventory
{
    /// <summary>
    /// Interaction logic for NewMenuProduct.xaml
    /// </summary>
    public partial class AddMenuProduct : Window
    {
        bool returnvalue = false;
        public AddMenuProduct()
        {
            InitializeComponent();
        }

        private void Button_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_ProductName.Text.Trim() == "")
                {
                    MessageBox.Show("Enter the name of the Product.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Combobox_Category.SelectedItem == null)
                {
                    MessageBox.Show("Select Category", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } 
                if (!decimal.TryParse(Textbox_ProductPrice.Text.Trim(), out decimal productprice))
                {
                    MessageBox.Show("The ProductPrice value entered is not allowed !", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Textbox_ProductPrice.Focus();
                        return;
                }
                if (!decimal.TryParse(Textbox_PackagingPrice.Text.Trim(), out decimal packagingprice))
                {
                    MessageBox.Show("The Packaging Cost value entered is not allowed !", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Textbox_PackagingPrice.Focus();
                        return;
                }
                returnvalue = true;
                this.Close();
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = returnvalue;
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
