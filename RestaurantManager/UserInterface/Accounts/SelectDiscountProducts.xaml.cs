 
using RestaurantManager.GlobalVariables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using RestaurantManager.ApplicationFiles;
using DatabaseModels.Inventory;

namespace RestaurantManager.UserInterface.Accounts
{
    /// <summary>
    /// Interaction logic for MergeTickets.xaml
    /// </summary>
    public partial class SelectDiscountProducts : Window
    {
        public ObservableCollection<MenuProductItem> Products = new ObservableCollection<MenuProductItem>();
        public SelectDiscountProducts()
        {
            InitializeComponent(); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }
         private void LoadProducts()
        {
            try
            {
                var db = new PosDbContext();
                Products = new ObservableCollection<MenuProductItem>(db.MenuProductItem.AsNoTracking().ToList());
                LisTview_ProductsList.ItemsSource = Products;
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
