using DatabaseModels.Inventory;
using RestaurantManager.ActivityLogs;
using RestaurantManager.ApplicationFiles;
using RestaurantManager.GlobalVariables;
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

namespace RestaurantManager.UserInterface.Inventory.AssetManagement
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
                if (Combobox_AssetUOM.SelectedItem == null)
                {
                    MessageBox.Show("Select the Asset Unit of Measure!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }  

                if (Button_Save.Content.ToString()=="Update Asset")
                {
                    string category = "";
                    AssetGroup productCategory = (AssetGroup)Combobox_AssetGroup.SelectedItem;
                    category = productCategory.GroupGuid;

                    using (var db = new PosDbContext())
                    {
                        var edititem = db.AssetItem.FirstOrDefault(k => k.AssetItemGuid == pitem.AssetItemGuid);
                        if (edititem != null)
                        {
                            edititem.AssetName = Textbox_ProductName.Text;
                            edititem.AssetDescription = Textbox_Description.Text.Trim();
                            edititem.AssetGroupGuid = productCategory.GroupGuid;
                            edititem.UOM = ((AssetUOM)Combobox_AssetUOM.SelectedItem).UnitGuid;
                            edititem.IsFoodMaterial = (bool)Checkbox_IsFoodMaterial.IsChecked ? true : false; 
                            edititem.LastUpdateDate = GlobalVariables.SharedVariables.CurrentDate();
                            db.SaveChanges();
                            MessageBox.Show("Success. Item Updated.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                            ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Update Asset","Updated Asset=" +edititem.AssetItemGuid+",asset name"+edititem.AssetName);
                        }
                        else
                        {
                            MessageBox.Show("The Asset Item does not Exist!!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    this.Close();
                }
                else if (Button_Save.Content.ToString() == "Save")
                {
                    string category = "";
                    AssetGroup productCategory = (AssetGroup)Combobox_AssetGroup.SelectedItem;
                    category = productCategory.GroupGuid;
                    using (var db = new PosDbContext())
                    {
                        var ass = new AssetItem()
                        {
                            AssetItemGuid = Guid.NewGuid().ToString(),
                            AssetName = Textbox_Description.Text,
                            AssetDescription = Textbox_Description.Text.Trim(),
                            AssetGroupGuid = productCategory.GroupGuid,
                            UOM = ((AssetUOM)Combobox_AssetUOM.SelectedItem).UnitGuid,
                            IsFoodMaterial = (bool)Checkbox_IsFoodMaterial.IsChecked ? true : false,
                            RegistrationDate = GlobalVariables.SharedVariables.CurrentDate(),
                            LastUpdateDate = GlobalVariables.SharedVariables.CurrentDate()
                        };
                        db.AssetItem.Add(ass);
                        db.SaveChanges();
                        MessageBox.Show("Success. Item Saved.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Added new Asset", "Assetcode=" + ass.AssetItemGuid+",name="+ass.AssetName);
                    }
                    this.Close();
                }
                else
                {
                    
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
                 }
                if (pitem != null)
                {
                    Button_Save.Content = "Update Asset";
                    this.Title = "Edit Asset - "+ pitem.AssetName;
                    Textbox_ProductName.Text = pitem.AssetName;
                    Textbox_Description.Text = pitem.AssetDescription.ToString();
                      Combobox_AssetGroup.SelectedItem = Combobox_AssetGroup.Items.Cast<AssetGroup>().FirstOrDefault(x => x.GroupGuid == pitem.AssetGroupGuid);
                    Combobox_AssetUOM.SelectedItem = Combobox_AssetUOM.Items.Cast<AssetUOM>().FirstOrDefault(x => x.UnitGuid == pitem.UOM);
  
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
