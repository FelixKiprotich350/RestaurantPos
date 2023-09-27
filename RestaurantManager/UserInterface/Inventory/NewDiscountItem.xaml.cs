using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using DatabaseModels.Warehouse;
using RestaurantManager.ApplicationFiles; 

namespace RestaurantManager.UserInterface.Inventory
{
    /// <summary>
    /// Interaction logic for NewDiscountItem.xaml
    /// </summary>
    public partial class NewDiscountItem : Window
    {
        public ObservableCollection<MenuProductItem> Selected_Items = new ObservableCollection<MenuProductItem>();
        public ObservableCollection<MenuProductItem> failed_items = new ObservableCollection<MenuProductItem>();
        public ObservableCollection<DiscountItem> Final_Selected_Items = new ObservableCollection<DiscountItem>();
        public NewDiscountItem()
        {
            InitializeComponent();
            Datagrid_DiscountProductItems.ItemsSource = Selected_Items;
            Grid_GiftItemSelector.Visibility = Visibility.Collapsed;
            Grid_PriceDiscSelector.Visibility = Visibility.Collapsed;
            Label_GiftedItem.Tag = null;
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close  ();
        }

        private void Button_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Combobox_DiscType.SelectedItem == null )
                {
                    MessageBox.Show("Select Discount Type and Its Value!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Combobox_DiscType.Text == "PricePercentage")
                {
                    if (!decimal.TryParse(Textbox_Pricediscount.Text.Trim(), out _))
                    {
                        MessageBox.Show("Enter Discount Percentage!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else if (Combobox_DiscType.Text == "GiftItem")
                {
                    if (Label_GiftedItem.Tag is null)
                    {
                        MessageBox.Show("Select a Gift Item!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                if ((bool)CheckBox_RepeatWeekly.IsChecked)
                {
                    if (Combobox_WeeklyDays.SelectedItem == null)
                    {
                        MessageBox.Show("Select Weekly Offer Day!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                {
                    Combobox_WeeklyDays.SelectedItem = null;
                }
                if (Datepicker_Startdate.SelectedDate == null)
                {
                    MessageBox.Show("Select the StartDate!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Datepicker_Enddate.SelectedDate == null)
                {
                    MessageBox.Show("Select the EndDate!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Selected_Items.Count <= 0)
                {
                    MessageBox.Show("You must Select atleast one Item to Discount!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string disctype = Combobox_DiscType.Text;
                using (var db = new PosDbContext())
                {
                   foreach(var x in Selected_Items)
                    {
                        DiscountItem d = new DiscountItem
                        {
                            DiscRowID = Guid.NewGuid().ToString(),
                            ProductGuid = x.ProductGuid,
                            ProductName = x.ProductName,
                            DiscType = disctype,
                            DiscStatus = "Active",
                            StartDate = (DateTime)Datepicker_Startdate.SelectedDate,
                            EndDate = (DateTime)Datepicker_Enddate.SelectedDate,
                            IsRepetitive = (bool)CheckBox_RepeatWeekly.IsChecked,
                            OfferDay = "None"
                        };
                        if ((bool)CheckBox_RepeatWeekly.IsChecked)
                        {
                            d.OfferDay = Combobox_WeeklyDays.Text;
                        }
                        if (disctype== "PricePercentage")
                        {
                            d.DiscPercentage = decimal.Parse(Textbox_Pricediscount.Text.Trim());
                            d.AttachedProduct = "None";
                        }
                        else if (disctype== "GiftItem")
                        {
                            d.DiscPercentage = 0;
                            MenuProductItem gift = (MenuProductItem)Label_GiftedItem.Tag;
                            d.AttachedProduct = gift.ProductGuid;
                        } 
                        Final_Selected_Items.Add(d);
                    }
                    foreach (var x in Selected_Items)
                    {
                        x.IsSelected = false;
                    }
                    db.DiscountItem.AddRange(Final_Selected_Items.ToArray());
                    db.SaveChanges();
                    MessageBox.Show("Items Discounted Successfully!","Message Box",MessageBoxButton.OK,MessageBoxImage.Information);
                    Close();
                } 
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                failed_items.Clear();
                //add items
                var db = new PosDbContext();
                SelectProduct sp = new SelectProduct();
                if ((bool)sp.ShowDialog())
                {
                    foreach (var x in sp.Returnlist)
                    {
                        if (db.DiscountItem.AsNoTracking().Where(l => l.ProductGuid == x.ProductGuid && l.DiscStatus == "Active").Count() <= 0)
                        {
                            if (Selected_Items.FirstOrDefault(k=>k.ProductGuid==x.ProductGuid) == null)
                            {
                                Selected_Items.Add(x);
                            }

                        }
                        else
                        {
                            failed_items.Add(x);
                        }
                    }
                    foreach (var x in Selected_Items)
                    {
                        x.IsSelected = false;
                    }
                }
                if (failed_items.Count > 0)
                {
                    MessageBox.Show("Total of [" + failed_items.Count.ToString() + "] ITEMS cannot be added because they have an active discount!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
 
        private void Combobox_DiscType_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (Combobox_DiscType.Text == "PricePercentage")
                {
                    Grid_PriceDiscSelector.Visibility = Visibility.Visible;
                    Grid_GiftItemSelector.Visibility = Visibility.Collapsed;
                }
                else if (Combobox_DiscType.Text == "GiftItem")
                {
                    Grid_PriceDiscSelector.Visibility = Visibility.Collapsed;
                    Grid_GiftItemSelector.Visibility = Visibility.Visible;
                }
                else
                {
                    Grid_PriceDiscSelector.Visibility = Visibility.Collapsed;
                    Grid_GiftItemSelector.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_SelectGiftItem_Click(object sender, RoutedEventArgs e)
        {
            SelectProduct sp = new SelectProduct
            {
                Title = "Select a Gift Item"
            };
            if ((bool)sp.ShowDialog())
            {
                var x = sp.Returnlist[0];
                Label_GiftedItem.Tag = x;
                Label_GiftedItem.Content = x.ProductName;
            }
            else
            {
                Label_GiftedItem.Tag = null;
                Label_GiftedItem.Content = "";
            }
        }
    }
}
