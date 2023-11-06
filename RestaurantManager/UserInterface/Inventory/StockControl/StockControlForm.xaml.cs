using DatabaseModels.Inventory;
using DatabaseModels.OrderTicket;
using DatabaseModels.Payments;
using DatabaseModels.WorkPeriod;
using RestaurantManager.ApplicationFiles;
using RestaurantManager.GlobalVariables; 
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

namespace RestaurantManager.UserInterface.Inventory
{
    /// <summary>
    /// Interaction logic for KitchenAdds.xaml
    /// </summary>
    public partial class StockControlForm : Page
    {
        private bool ShowUnprecountableitems = false;
        public StockControlForm()
        {
            InitializeComponent();
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowUnprecountableitems = false;
                Checkbox_ShowNonprecountableItems.IsChecked = false;
                RefreshMenuProducts();
                Combobox_AdjustmentReason.ItemsSource = Enum.GetNames(typeof(PosEnums.StockFlowAdjustmentReason)).ToList();
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
                var db = new PosDbContext();
                List<MenuProductItem> products = new List<MenuProductItem>();
                List<ProductCategory> categories = db.ProductCategory.AsNoTracking().ToList();
                List<OrderItem> orderitemsall = db.OrderItem.AsNoTracking().ToList();
                List<OrderItem> orderitems = new List<OrderItem>();
                List<OrderMaster> orderimaster = db.OrderMaster.AsNoTracking().Where(k=>k.OrderStatus==PosEnums.OrderTicketStatuses.Pending.ToString()).ToList();
                List<StockFlowTransaction> transactions = db.StockFlowTransaction.AsNoTracking().ToList();
                foreach (var x in orderimaster)
                {
                    orderitems.AddRange(orderitemsall.Where(k => k.OrderID == x.OrderNo));
                }
                if (ShowUnprecountableitems)
                {
                    products = db.MenuProductItem.AsNoTracking().ToList();
                }
                else
                {
                    products = db.MenuProductItem.AsNoTracking().Where(k => k.IsPrecount).ToList();
                }

                foreach (var x in products)
                {
                    x.ReceivedQuantity= (int)transactions.Where(k => k.ProductGuid==x.ProductGuid&k.FlowDirection == "IN").Sum(p => p.Quantity);
                    x.SoldQuantity= (int)transactions.Where(k => k.ProductGuid == x.ProductGuid & k.FlowDirection == "OUT").Sum(p => p.Quantity);
                    x.PendingQuantity= (int)orderitems.Where(k => k.ProductItemGuid == x.ProductGuid).Sum(p => p.Quantity);
                    if (x.IsPrecount)
                    {
                        x.AvailableQuantity = (x.ReceivedQuantity - (x.SoldQuantity+x.PendingQuantity)).ToString();
                    }
                    else
                    {
                        x.AvailableQuantity = "N/A";
                    }
                    var catname = categories.FirstOrDefault(k=>k.CategoryGuid==x.CategoryGuid);
                    x.CategoryName = (catname != null) ? catname.CategoryName : "UNKNOWN";

                }
                Datagrid_ItemsList.ItemsSource = products;
             }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Datagrid_ItemsList_MouseUp(object sender, MouseButtonEventArgs e)
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
                    if (cell.Column.DisplayIndex != 1)
                    {
                        return;
                    }
                    if (Datagrid_ItemsList.SelectedItem == null)
                    {
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool Contains(object de)
        {
            MenuProductItem item = de as MenuProductItem;
            return item.ProductName.ToLower().Contains(Textbox_ProductSearchBox.Text.ToLower());

        }

        private void Textbox_ProductSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox t = (TextBox)sender;
                string filter = t.Text;
                if (Datagrid_ItemsList.ItemsSource == null)
                {
                    return;
                }
                ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_ItemsList.ItemsSource);
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

        private void Datagrid_ItemsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ResetQuantityForm();
                var db = new PosDbContext();
                if (Datagrid_ItemsList.SelectedItem is MenuProductItem om)
                {
                    Textbox_productid.Text = om.ProductGuid;
                    Textbox_productname.Text = om.ProductName;
                    Textbox_AvailableQuantity.Text = om.RemainingQuantity.ToString();
                    TextBox_SellingPrice.Text = om.SellingPrice.ToString();
                    TextBox_BuyingPrice.Text = om.BuyingPrice.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ViewItem_MouseUp(object sender, MouseButtonEventArgs e)
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
                    if (cell.Column.DisplayIndex == 8)
                    {
                        if (Datagrid_ItemsList.SelectedItem is MenuProductItem sft)
                        {
                            ViewProductStockHistory v = new ViewProductStockHistory()
                            {
                                ProductId = sft.ProductGuid,
                                ProductName=sft.ProductName
                            };
                            v.ShowDialog();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }
        private void Button_NewProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddMenuProduct nmp = new AddMenuProduct();
                if (nmp.ShowDialog() == false)
                {
                    return;
                }
                else
                {
                    RefreshMenuProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_RefreshProducts_Click(object sender, RoutedEventArgs e)
        {
            RefreshMenuProducts();

        }
        private void CheckBox_AutogenerateID_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Textbox_PrimaryRefference.Text = DateTime.Now.ToString("yyyyMMddHHmmss");
                Textbox_PrimaryRefference.IsReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CheckBox_AutogenerateID_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Textbox_PrimaryRefference.IsReadOnly = false;
                Textbox_PrimaryRefference.Text = "";


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_AddQuantity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_productid.Text.Trim() == "")
                {
                    MessageBox.Show("Select a Product to Receive!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Textbox_PrimaryRefference.Text.Trim() == "")
                {
                    MessageBox.Show("Enter the processing Batch Number!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(TextBox_AddingQuantity.Text, out int qty))
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
                if (!pi.IsPrecount)
                {
                    MessageBox.Show("The selected Product is not Precountable!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                StockFlowTransaction sft = new StockFlowTransaction
                {
                    ProductGuid = pi.ProductGuid,
                    ProductName = pi.ProductName,
                    TransactionDate = GlobalVariables.SharedVariables.CurrentDate(),
                    IsCancelled = false,
                    PrimaryRefference = Textbox_PrimaryRefference.Text.Trim(),
                    FlowDirection = "IN",
                    StockFlowTrigger = PosEnums.StockFlowTriggerSource.Purchased.ToString(),
                    Description = "Items received or Purchased",
                    SecondaryRefference = (Textbox_SecondaryRefference.Text.Trim() != "") ? Textbox_SecondaryRefference.Text.Trim() : "None",
                    Quantity = qty
                };
                db.StockFlowTransaction.Add(sft);
                pi.RemainingQuantity += qty;
                db.SaveChanges();
                MessageBox.Show("Success. Item Quantity Added!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetQuantityForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
  
        private void Checkbox_ShowNonprecountableItems_Checked(object sender, RoutedEventArgs e)
        {

            try
            {
                if ((bool)Checkbox_ShowNonprecountableItems.IsChecked)
                {
                    ShowUnprecountableitems = true;
                }
                else
                {
                    ShowUnprecountableitems = false;
                }
                RefreshMenuProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Checkbox_ShowNonprecountableItems_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((bool)Checkbox_ShowNonprecountableItems.IsChecked)
                {
                    ShowUnprecountableitems = true;
                }
                else
                {
                    ShowUnprecountableitems = false;
                }
                RefreshMenuProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        void ResetQuantityForm()
        {
            Textbox_productid.Text = "";
            Textbox_productname.Text = "";
            Textbox_AvailableQuantity.Text = "";
            TextBox_SellingPrice.Text = "";
            TextBox_BuyingPrice.Text = "";
            Textbox_SecondaryRefference.Text = "A";
            Textbox_PrimaryRefference.Text = "1234";
            CheckBox_AutogenerateID.IsChecked = false;
            TextBox_AddingQuantity.Text = "";
        }

        /// <summary>
        /// stock adjustment tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void Button_SelectProduct_Click(object sender, RoutedEventArgs e)
        {
            SelectProduct s = new SelectProduct();
            if ((bool)s.ShowDialog())
            {
                if (s.Returnlist.Count > 0)
                {
                    Textbox_Adjustmentproductid.Text = s.Returnlist[0].ProductGuid;
                    Textbox_Adjustmentproductname.Text = s.Returnlist[0].ProductName;
                }
            }
        }

        private void Button_AdjustmentSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_Adjustmentproductid.Text.Trim() == "")
                {
                    MessageBox.Show("Select a Product to Adjust!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Textbox_AdjustmentDescription.Text.Trim() == "")
                {
                    MessageBox.Show("Enter the adjustment description!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Combobox_AdjustmentReason.SelectedItem == null)
                {
                    MessageBox.Show("Enter the Adjustment Reason!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(TextBox_AdjustmentQuantity.Text, out int qty))
                {
                    MessageBox.Show("Enter the correct Quantity for this item to Adjust!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                MenuProductItem pi = null;
                var db = new PosDbContext();
                pi = db.MenuProductItem.FirstOrDefault(m => m.ProductGuid == Textbox_Adjustmentproductid.Text.Trim());
                if (pi is null)
                {
                    MessageBox.Show("The selected Product does not Exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!pi.IsPrecount)
                {
                    MessageBox.Show("The selected Product is not Precountable!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                StockFlowTransaction sft = new StockFlowTransaction
                {
                    ProductGuid = pi.ProductGuid,
                    ProductName = pi.ProductName,
                    TransactionDate = GlobalVariables.SharedVariables.CurrentDate(),
                    IsCancelled = false,
                    PrimaryRefference = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    FlowDirection = "OUT",
                    StockFlowTrigger = PosEnums.StockFlowTriggerSource.Adjusted.ToString(),
                    Description = Textbox_AdjustmentDescription.Text.Trim(),
                    SecondaryRefference = Combobox_AdjustmentReason.SelectedItem.ToString(),
                    Quantity = qty
                };
                db.StockFlowTransaction.Add(sft);
                pi.RemainingQuantity += qty;
                db.SaveChanges();
                MessageBox.Show("Success. Adjustment Received!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                TextBox_AddingQuantity.Text = "";
                Textbox_productid.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_AdjustmentRefreshment_Click(object sender, RoutedEventArgs e)
        {
            RefreshRecentAdjustments();
        }

        private void RefreshRecentAdjustments()
        {
            try
            {
                List<StockFlowTransaction> item = new List<StockFlowTransaction>();
                using (var db = new PosDbContext())
                {
                    item = db.StockFlowTransaction.AsNoTracking().Where(k=>k.StockFlowTrigger==PosEnums.StockFlowTriggerSource.Adjusted.ToString()).ToList();
                }
                Datagrid_AdjustmentsItemsEntry.ItemsSource = item;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// last tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_NewSTT_Click(object sender, RoutedEventArgs e)
        {
            new AddStockTakingNumber().ShowDialog();
        }

       
    }
}
