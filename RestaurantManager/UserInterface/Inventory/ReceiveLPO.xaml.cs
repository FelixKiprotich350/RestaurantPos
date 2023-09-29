using DatabaseModels.Inventory;
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

namespace RestaurantManager.UserInterface.Inventory
{
    /// <summary>
    /// Interaction logic for KitchenAdds.xaml
    /// </summary>
    public partial class ReceiveLPO : Page
    {
        public ReceiveLPO()
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
                var db = new PosDbContext();  
                var products = db.StockFlowTransaction.AsNoTracking().ToList();  
                Datagrid_ItemsEntry.ItemsSource = products;
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
                if (Textbox_productid.Text.Trim()=="")
                {
                    MessageBox.Show("Select a Product to Receive!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } 
                if (Textbox_BatchNumber.Text.Trim()=="")
                {
                    MessageBox.Show("Enter the processing Batch Number!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                var db = new PosDbContext();
                 pi = db.MenuProductItem.FirstOrDefault(m => m.ProductGuid == Textbox_productid.Text.Trim());
                if (pi is null)
                {
                    MessageBox.Show("The selected Product does not Exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                StockFlowTransaction sft = new StockFlowTransaction
                {
                    ProductGuid = pi.ProductGuid,
                    ProductName = pi.ProductName,
                    TransactionDate = GlobalVariables.SharedVariables.CurrentDate(),
                    IsCancelled = false,
                    OutTransactionCode = "N/A",
                    FlowDirection = "IN",
                    InTransactionCode = Textbox_BatchNumber.Text.Trim(),
                    Quantity = qty
                };
                db.StockFlowTransaction.Add(sft);
                pi.RemainingQuantity += qty;
                db.SaveChanges();
                MessageBox.Show("Success. Item Quantity Added!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                TextBox_Quantity.Text = "";
                Textbox_productid.Text = "";
                RefreshEntries();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       

        private void Textbox_SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_SelectProduct_Click(object sender, RoutedEventArgs e)
        {
            SelectProduct s = new SelectProduct();
            if ((bool)s.ShowDialog())
            {
               if (s.Returnlist.Count > 0)
                {
                    Textbox_productid.Text = s.Returnlist[0].ProductGuid;
                    Textbox_productname.Text = s.Returnlist[0].ProductName;
                }
            }
        }

        
    }
}
