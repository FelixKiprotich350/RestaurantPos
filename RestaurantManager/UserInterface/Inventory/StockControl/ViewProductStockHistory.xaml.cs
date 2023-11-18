using DatabaseModels.Inventory;
using RestaurantManager.ActivityLogs;
using RestaurantManager.ApplicationFiles;
using RestaurantManager.GlobalVariables;
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
    /// Interaction logic for SelectProduct.xaml
    /// </summary>
    public partial class ViewProductStockHistory : Window
    {
        public List<StockFlowTransaction> AllProducts = new List<StockFlowTransaction>();
        public string ProductId = "";
        public string ProductName = "";
        public ViewProductStockHistory()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db=new PosDbContext())
                {
                    AllProducts = db.StockFlowTransaction.AsNoTracking().Where(k=>k.ProductGuid==ProductId).ToList();
                }
                Datagrid_AllProductItems.ItemsSource = AllProducts;
                this.Title = ProductName + " Stocki IN and OUT History";
                ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Viewed Product stock History", "Product code=" + ProductId+",product name="+ProductName);

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message,"Message Box",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        { 
            Close();
        }

    }
}
