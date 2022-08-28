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
        bool returnvalue = false;
        public NewMenuProduct()
        {
            InitializeComponent();
        }

        private void Button_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal price = 0; 
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
                if (!decimal.TryParse(Textbox_Price.Text.Trim(), out price))
                {
                    MessageBox.Show("The Price value entered is not allowed!.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
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
    }
}
