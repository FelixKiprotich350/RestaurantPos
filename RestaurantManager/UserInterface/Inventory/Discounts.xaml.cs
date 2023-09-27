using DatabaseModels.Warehouse;
using RestaurantManager.ApplicationFiles; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for Discounts.xaml
    /// </summary>
    public partial class Discounts : Page
    {
        public Discounts()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateExpiredDiscounts();
                RefreshProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateExpiredDiscounts()
        {
            try
            { 
                List<DiscountItem> item = new List<DiscountItem>();
                using (var db = new PosDbContext())
                {
                    foreach (var x in db.DiscountItem)
                    {
                        if (x.EndDate < GlobalVariables.SharedVariables.CurrentDate() && x.DiscStatus == "Active")
                        {
                            x.DiscStatus = "Expired";
                        }
                    }
                    db.SaveChanges();
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }private void RefreshProducts()
        {
            try
            { 
                List<DiscountItem> item = new List<DiscountItem>();
                using (var db = new PosDbContext())
                { 
                   var x = db.DiscountItem.AsNoTracking().ToList();
                    if (x.Count > 0)
                    {
                        item = x;
                        foreach (DiscountItem i in item)
                        {
                            i.ProductName = db.MenuProductItem.First(k => k.ProductGuid == i.ProductGuid).ProductName;
                        }
                    }
                    
                }
             
                Datagrid_DiscountProductItems.ItemsSource = item; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Textbox_SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

       
        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshProducts();
        }

        private void Button_AddDiscountproduct_Click(object sender, RoutedEventArgs e)
        {
            NewDiscountItem newitem = new NewDiscountItem();
            newitem.ShowDialog();
            RefreshProducts();
        }

        private void Datagrid_DiscountProductItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            try
            {
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                // iteratively traverse the visual tree
                while ((dep != null) & !(dep is DataGridCell) & !(dep is DataGridColumnHeader))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }
                if (dep == null)
                {
                    return;
                }
                if (dep is DataGridCell)
                {
                    if (Datagrid_DiscountProductItems.SelectedItem == null)
                    {
                        return;
                    }
                    DiscountItem o = (DiscountItem)Datagrid_DiscountProductItems.SelectedItem;
                    EditDiscount ed = new EditDiscount()
                    {
                      
                        Title = o.ProductName
                    };
                    ed.Textblock_CurrentStatus.Text = o.DiscStatus;
                    ed.Item = o;
                    if ((bool)ed.ShowDialog())
                    {
                        RefreshProducts();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
