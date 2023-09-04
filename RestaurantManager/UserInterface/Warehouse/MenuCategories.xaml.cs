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
    /// Interaction logic for MenuCategories.xaml
    /// </summary>
    public partial class MenuCategories : Page
    {
        public MenuCategories()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListView_Categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ListView_Categories.SelectedItem == null)
                {
                    ClearSelectedItem();
                    return;
                }
                ClearSelectedItem();
                Button_Update.IsEnabled = false;
                Button_Delete.IsEnabled = true;
                CheckBox_Edit.IsChecked = false;
                Textbox_CategoryName.IsReadOnly = true;
                
                using (var db = new PosDbContext())
                {
                    ProductCategory pc=(ProductCategory)ListView_Categories.SelectedItem;
                    Textbox_CategoryName.Text = pc.CategoryName;
                    ListView_SelectedCategoryItems.ItemsSource = db.MenuProductItem.Where(b=>b.CategoryGuid==pc.CategoryGuid).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 

        private void RefreshCategories()
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    ListView_Categories.ItemsSource = db.ProductCategory.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearSelectedItem()
        {
            try
            {
                Button_Update.IsEnabled = false;
                Button_Delete.IsEnabled = false;
                CheckBox_Edit.IsChecked = false;
                Textbox_CategoryName.IsReadOnly = true;
                Textbox_CategoryName.Text = "";
                ListView_SelectedCategoryItems.ItemsSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckBox_Edit_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Button_Update.IsEnabled = true;
                Button_Delete.IsEnabled = false; 
                Textbox_CategoryName.IsReadOnly = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckBox_Edit_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Button_Update.IsEnabled = false;
                Button_Delete.IsEnabled = true; 
                Textbox_CategoryName.IsReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListView_Categories.SelectedItem == null)
                {
                    return;
                }
                ProductCategory pc = null;
                using (var db = new PosDbContext())
                {
                    pc = (ProductCategory)ListView_Categories.SelectedItem;
                    db.ProductCategory.Where(t => t.CategoryGuid == pc.CategoryGuid).First().CategoryName = Textbox_CategoryName.Text;
                    int x = db.SaveChanges();
                    if (x != 1)
                    {
                        MessageBox.Show("Failed to Update the Category", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    db.SaveChanges();
                    MessageBox.Show("Success. Category Updated!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                }
                RefreshCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to Delete this Category ?\nAll the products under the category will also be deleted","Message Box",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_NewCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddnewCategory a = new AddnewCategory
                {
                    Topmost = true
                };
                if (!(bool)a.ShowDialog())
                {
                    return;
                }
                using (var db = new PosDbContext())
                {
                    db.ProductCategory.Add(new ProductCategory() { CategoryGuid = Guid.NewGuid().ToString(), CategoryName = a.Textbox_Categoryname.Text, CreationDate = GlobalVariables.SharedVariables.CurrentDate(),Department=a.Combobox_department.Text });
                    int x=db.SaveChanges();
                    if (x==1)
                    {
                        MessageBox.Show("Success. Category Added!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
                RefreshCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
