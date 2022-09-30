using RestaurantManager.BusinessModels.OrderTicket;
using RestaurantManager.BusinessModels.Vouchers;
using RestaurantManager.GlobalVariables;
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

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for DiscountsSelector.xaml
    /// </summary>
    public partial class DiscountsSelector : Window
    {
        public List<DiscountVoucher> Discountslist = new List<DiscountVoucher>(); 
        public DiscountsSelector( List<DiscountVoucher> l)
        {
            InitializeComponent();
            Discountslist = l;
        }
         

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LisTview_DiscountsList.ItemsSource = Discountslist;
        } 
        private void Button_Accept_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                if (Discountslist.Where(k => k.IsSelected).Count() > 0)
                {
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Select atleast one Item or close the Window!","Message Box",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void CheckBox_SelectAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            { 

            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
