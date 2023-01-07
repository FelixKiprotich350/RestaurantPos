using RestaurantManager.ApplicationFiles;
using RestaurantManager.BusinessModels.Warehouse;
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
    /// Interaction logic for StockEntry.xaml
    /// </summary>
    public partial class StockEntry : Page
    {
        public StockEntry()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshEntries();
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
                List<StockEntryItem> item = new List<StockEntryItem>();
                using (var db = new PosDbContext())
                { 
                    item = db.StockEntryItem.ToList();
                }
                Datagrid_ItemsEntry.ItemsSource = item;
                Label_Count.Content = Datagrid_ItemsEntry.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Textbox_SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Datagrid_ItemsEntry_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_SaveEntry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_Description.Text == "")
                {
                    MessageBox.Show("Enter description for this item!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } 
                if (Textbox_UOM.Text == "")
                {
                    MessageBox.Show("Enter the Unit of Measure for this item!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                if (!int.TryParse(Textbox_Quantity.Text,out int qty))
                {
                    MessageBox.Show("Enter the correct Quantity for this item!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var wp = GlobalVariables.SharedVariables.CurrentOpenWorkPeriod();
                if(wp is null)
                {
                    MessageBox.Show("The work Period Is not Open!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                using (var db = new PosDbContext())
                {
                    StockEntryItem s = new StockEntryItem()
                    {
                        ItemGuid = Guid.NewGuid().ToString(),
                        ItemDescription = Textbox_Description.Text,
                        Quantity = qty,
                        WorkPeriod = wp.WorkperiodName,
                        UOM = Textbox_UOM.Text,
                        InsertionDate = GlobalVariables.SharedVariables.CurrentDate(),
                        InsertionBy = GlobalVariables.SharedVariables.CurrentUser.UserName
                    };
                    db.StockEntryItem.Add(s);
                    db.SaveChanges();
                    MessageBox.Show("Success Item Saved!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshEntries();
                }
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
                RefreshEntries();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
