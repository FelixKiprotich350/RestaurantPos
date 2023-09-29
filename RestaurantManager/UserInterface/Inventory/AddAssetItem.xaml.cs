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
    /// Interaction logic for NewMenuProduct.xaml
    /// </summary>
    public partial class AddAssetItem : Window
    {
        bool returnvalue = false;
        AssetItem pitem = null;
        public AddAssetItem()
        {
            InitializeComponent();
        }
        public AddAssetItem(AssetItem item)
        {
            InitializeComponent();
            pitem = item;
        }

        private void Button_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (Textbox_ProductName.Text.Trim() == "")
                {
                    MessageBox.Show("Enter the name of the Asset.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Combobox_AssetGroup.SelectedItem == null)
                {
                    MessageBox.Show("Select the Asset Group!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } 
                if (Combobox_AssetType.SelectedItem == null)
                {
                    MessageBox.Show("Select the Asset Type!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } 
                if (Combobox_AssetUOM.SelectedItem == null)
                {
                    MessageBox.Show("Select the Asset Unit of Measure!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } 
                if (Combobox_Precount.SelectedItem == null)
                {
                    MessageBox.Show("Select the Asset Count Status!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } 
                if (!decimal.TryParse(Textbox_AssetCost.Text.Trim(), out decimal assetcost))
                {
                    MessageBox.Show("The Asset Value Cost value entered is not allowed !", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Textbox_AssetCost.Focus();
                        return;
                }
                if (!decimal.TryParse(Textbox_InitialQuantity.Text.Trim(), out decimal qty))
                {
                    MessageBox.Show("The Asset Quantity value entered is not allowed !", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Textbox_InitialQuantity.Focus();
                        return;
                }

                if (Button_Save.Content.ToString()!="Update Asset")
                {
                    returnvalue = true;
                    this.Close();
                }
                else
                {
                    string category = "";
                    AssetGroup productCategory = (AssetGroup)Combobox_AssetGroup.SelectedItem;
                    category = productCategory.GroupGuid;
                    if (!decimal.TryParse(Textbox_AssetCost.Text.Trim(), out decimal AssetCost))
                    {
                        MessageBox.Show("The Cost value entered is not allowed!.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (!int.TryParse(Textbox_InitialQuantity.Text.Trim(), out int AssetCount))
                    {
                        MessageBox.Show("The Quantity value entered is not allowed!.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    using (var db = new PosDbContext())
                    {
                        var edititem=db.AssetItem.FirstOrDefault(k => k.AssetItemGuid == pitem.AssetItemGuid);
                        if (edititem != null)
                        {
                            edititem.AssetName = Textbox_ProductName.Text;
                            edititem.AssetDescription = Textbox_Description.Text.Trim();
                            edititem.AssetGroupGuid = productCategory.GroupGuid;
                            edititem.GroupName = productCategory.GroupName;
                            edititem.AssetItemCost = AssetCost;
                            edititem.UOM = ((AssetUOM)Combobox_AssetUOM.SelectedItem).UnitGuid;
                            edititem.InStockQuantity = AssetCount;
                            edititem.IsPrecount = Convert.ToBoolean(((ComboBoxItem)Combobox_Precount.SelectedValue).Content.ToString());
                            edititem.typeofasset = Combobox_AssetType.SelectedItem.ToString();
                            edititem.RegistrationDate = GlobalVariables.SharedVariables.CurrentDate();
                            edititem.LastUpdateDate = GlobalVariables.SharedVariables.CurrentDate();
                            db.SaveChanges();
                            MessageBox.Show("Success. Item Updated.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        { 
                            MessageBox.Show("The Asset Item does not Exist!!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new PosDbContext())
                {
                   Combobox_AssetGroup.ItemsSource = db.AssetGroup.AsNoTracking().ToList();
                   Combobox_AssetUOM.ItemsSource = db.AssetUOM.AsNoTracking().ToList();
                    Combobox_AssetType.ItemsSource = Enum.GetNames(typeof(GlobalVariables.PosEnums.AssetTypes)).ToList();
                }
                if (pitem != null)
                {
                    Button_Save.Content = "Update Asset";
                    this.Title = "Edit Asset - "+ pitem.AssetName;
                    Textbox_ProductName.Text = pitem.AssetName;
                    Textbox_Description.Text = pitem.AssetDescription.ToString();
                    Textbox_AssetCost.Text = pitem.AssetItemCost.ToString();
                    Textbox_InitialQuantity.Text = pitem.InStockQuantity.ToString();
                    Combobox_AssetGroup.SelectedItem = Combobox_AssetGroup.Items.Cast<AssetGroup>().FirstOrDefault(x => x.GroupGuid == pitem.AssetGroupGuid);
                    Combobox_AssetUOM.SelectedItem = Combobox_AssetUOM.Items.Cast<AssetUOM>().FirstOrDefault(x => x.UnitGuid == pitem.UOM);
                    Combobox_AssetType.SelectedItem = Combobox_AssetType.Items.Cast<string>().FirstOrDefault(x => x == pitem.typeofasset);
                    Combobox_Precount.SelectedItem = Combobox_Precount.Items.Cast<ComboBoxItem>().FirstOrDefault(x => x.Content.ToString()==pitem.IsPrecount.ToString());

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = returnvalue;
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
