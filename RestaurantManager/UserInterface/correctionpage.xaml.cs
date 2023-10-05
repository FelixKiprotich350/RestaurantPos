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

namespace RestaurantManager.UserInterface
{
    /// <summary>
    /// Interaction logic for correctionpage.xaml
    /// </summary>
    public partial class correctionpage : Page
    {
        public correctionpage()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                //get ticket items
                var db = new PosDbContext();

                var original = db.MenuProductItem.AsNoTracking().ToList();
                foreach (var x in original)
                {
                    var y = db.MenuProductItem.First(k => k.ProductGuid == x.ProductGuid);
                    y.BuyingPrice = x.PackagingCost;
                    y.SellingPrice = x.TotalCost;
                    y.PackagingCost = decimal.Parse(x.Department);
                    y.RemainingQuantity = 0;
                    y.TotalCost = x.BuyingPrice;
                    var cat = db.ProductCategory.AsNoTracking().FirstOrDefault(k => k.CategoryGuid == x.CategoryGuid);
                    if (cat != null)
                    {
                        y.Department = cat.Department;
                    }
                    else
                    {
                        y.Department = "a";
                    }
                     
                }
                db.SaveChanges();
                original = db.MenuProductItem.AsNoTracking().ToList();
                Datagrid_TicketItems.ItemsSource = original; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
