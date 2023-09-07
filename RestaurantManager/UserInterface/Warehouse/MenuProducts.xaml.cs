﻿using DatabaseModels.Warehouse;
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

namespace RestaurantManager.UserInterface.Warehouse
{
    /// <summary>
    /// Interaction logic for MenuProducts.xaml
    /// </summary>
    public partial class MenuProducts : Page
    {
        public MenuProducts()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshMenuProducts();
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
                if (Datagrid_ProductItems.ItemsSource==null)
                {
                    return;
                }
                ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_ProductItems.ItemsSource);
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
            MenuProductItem item = de as MenuProductItem;
            return item.ProductName.ToLower().Contains(Textbox_SearchBox.Text.ToLower()) | item.CategoryName.ToLower().Contains(Textbox_SearchBox.Text.ToLower());

        }

        private void Button_NewProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewMenuProduct nmp = new NewMenuProduct();
                if (nmp.ShowDialog() == false)
                {
                    return;
                }
                string category = "";
                ProductCategory productCategory = (ProductCategory)nmp.Combobox_Category.SelectedItem;
                category = productCategory.CategoryGuid;
                if (!decimal.TryParse(nmp.Textbox_BuyingPrice.Text.Trim(), out decimal buyingprice))
                {
                    MessageBox.Show("The Buying Price value entered is not allowed!.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!decimal.TryParse(nmp.Textbox_ProductPrice.Text.Trim(), out decimal price))
                {
                    MessageBox.Show("The ProductPrice value entered is not allowed!.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!decimal.TryParse(nmp.Textbox_PackagingPrice.Text.Trim(), out decimal packagingprice))
                {
                    MessageBox.Show("The Packaging Cost value entered is not allowed!.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                using (var db = new PosDbContext())
                {
                    db.MenuProductItem.Add(new MenuProductItem() 
                    { 
                        ProductGuid = Guid.NewGuid().ToString(), 
                        ProductName = nmp.Textbox_ProductName.Text, 
                        AvailabilityStatus = "Available", 
                        Department = productCategory.Department,
                        SellingPrice = price, 
                        PackagingCost = packagingprice,
                        CategoryGuid = category ,
                        BuyingPrice=buyingprice,
                        TotalCost=packagingprice+price
                    });
                    db.SaveChanges();
                    MessageBox.Show("Success. Item Saved.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshMenuProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           

        }

        private void RefreshMenuProducts()
        {
            try
            {
                List<ProductCategory> cat = new List<ProductCategory>();
                List<MenuProductItem> item = new List<MenuProductItem>();
                using (var db = new PosDbContext())
                {
                    cat = db.ProductCategory.ToList();
                    item = db.MenuProductItem.ToList();
                }
                foreach (var x in item)
                {
                    x.CategoryName = cat.Where(y => y.CategoryGuid == x.CategoryGuid).FirstOrDefault().CategoryName;
                }
                Datagrid_ProductItems.ItemsSource = item;
                TextBox_ProductsCount.Text = Datagrid_ProductItems.Items.Count.ToString() ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshMenuProducts();
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
                        if (Datagrid_ProductItems.SelectedItem == null)
                        {
                            return;
                        }
                        MenuProductItem m = (MenuProductItem)Datagrid_ProductItems.SelectedItem;
                        EditProduct ed = new EditProduct(m);
                        ed.ShowDialog();
                        RefreshMenuProducts();
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
