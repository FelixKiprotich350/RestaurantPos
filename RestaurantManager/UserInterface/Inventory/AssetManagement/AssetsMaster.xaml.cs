using DatabaseModels.Inventory;
using RestaurantManager.ApplicationFiles; 
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace RestaurantManager.UserInterface.Inventory.AssetManagement
{
    /// <summary>
    /// Interaction logic for MenuProducts.xaml
    /// </summary>
    public partial class AssetsMaster : Page
    {
        public AssetsMaster()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //RefreshAssetsProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void Textbox_SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox t = (TextBox)sender;
                string filter = t.Text;
                if (Datagrid_Assets.ItemsSource==null)
                {
                    return;
                }
                ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_Assets.ItemsSource);
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
            AssetItem item = de as AssetItem;
            return item.AssetName.ToLower().Contains(Textbox_SearchBox.Text.ToLower()) | item.GroupName.ToLower().Contains(Textbox_SearchBox.Text.ToLower());

        }

        private void Button_NewProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddAssetItem nmp = new AddAssetItem();
                nmp.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           

        }

        private void RefreshAssetsProducts()
        {
            try
            {
                List<AssetGroup> cat = new List<AssetGroup>();
                List<AssetItem> item = new List<AssetItem>();
                List<AssetUOM> uoms = new List<AssetUOM>();
                using (var db = new PosDbContext())
                {
                    cat = db.AssetGroup.ToList();
                    item = db.AssetItem.ToList();
                    uoms = db.AssetUOM.ToList();
                }
                foreach (var x in item)
                {
                    x.GroupName = cat.Where(y => y.GroupGuid == x.AssetGroupGuid).FirstOrDefault().GroupName;
                    x.UOMName = uoms.Where(y => y.UnitGuid == x.UOM).FirstOrDefault().UnitName;
                }
                Datagrid_Assets.ItemsSource = item;
                TextBox_AssetsCount.Text = Datagrid_Assets.Items.Count.ToString() ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshAssetsProducts();
            MessageBox.Show("Refresh Success. Done.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Datagrid_ProductItems_MouseUp(object sender, MouseButtonEventArgs e)
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
                if (dep is DataGridCell cell)
                {
                    if (cell.Column.DisplayIndex==1)
                    {
                        if (Datagrid_Assets.SelectedItem == null)
                        {
                            return;
                        }
                        AssetItem m = (AssetItem)Datagrid_Assets.SelectedItem;
                        AddAssetItem ed = new AddAssetItem(m);
                        ed.ShowDialog();
                        RefreshAssetsProducts();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// tab 2 : categories
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Datagrid_AssetsGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_SaveCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_UpdateCategory_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
