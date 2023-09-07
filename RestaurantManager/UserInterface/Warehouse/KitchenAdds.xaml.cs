using DatabaseModels.Warehouse;
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

namespace RestaurantManager.UserInterface.Warehouse
{
    /// <summary>
    /// Interaction logic for KitchenAdds.xaml
    /// </summary>
    public partial class KitchenAdds : Page
    {
        public KitchenAdds()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshEntries();
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadProducts()
        {
            try
            {
                List<MenuProductItem> item = new List<MenuProductItem>();
                using (var db = new PosDbContext())
                {
                    item = db.MenuProductItem.ToList();
                }
                Combobox_Product.ItemsSource = item; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void RefreshEntries()
        {
            try
            {
                var db = new PosDbContext();
                db.KitchenAddItem.AsNoTracking();
                db.MenuProductItem.AsNoTracking();
                var products = db.MenuProductItem.ToList();
                List<KitchenAddItem> item = new List<KitchenAddItem>();
                item = db.KitchenAddItem.ToList(); 
                Datagrid_ItemsEntry.ItemsSource = item;
                Label_Count.Content = Datagrid_ItemsEntry.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Datagrid_ItemsEntry_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_AddQuantity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Combobox_Product.SelectedItem is null)
                {
                    MessageBox.Show("Select a Product to Add the Quantity!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } 

                if (!int.TryParse(TextBox_Quantity.Text, out int qty))
                {
                    MessageBox.Show("Enter the correct Quantity for this item!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var wp = GlobalVariables.SharedVariables.CurrentOpenWorkPeriod();
                if (wp is null)
                {
                    MessageBox.Show("The work Period Is not Open!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                MenuProductItem pi = null;
                pi = Combobox_Product.SelectedItem as MenuProductItem;
                if (pi is null)
                {
                    MessageBox.Show("The selected Product is Unknown!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                using (var db = new PosDbContext())
                {
                    KitchenAddItem k = new KitchenAddItem()
                    {
                        ItemGuid = Guid.NewGuid().ToString(),
                        ProductGuid = pi.ProductGuid,
                        ProductName=pi.ProductName,
                        Quantity = qty,
                        WorkPeriod = wp.WorkperiodName,
                        InsertionDate = GlobalVariables.SharedVariables.CurrentDate(),
                        InsertionBy = GlobalVariables.SharedVariables.CurrentUser.UserName
                    };
                    int val = (int)db.MenuProductItem.First(x => x.ProductGuid == pi.ProductGuid).PackagingCost;
                    int total = qty + val;
                    //db.MenuProductItem.First(x => x.ProductGuid == pi.ProductGuid).RemainingQuantity = total;
                    db.KitchenAddItem.Add(k);
                    db.SaveChanges();
                    MessageBox.Show("Success. Item Quantity Added!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshEntries();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       

        private void Textbox_SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
