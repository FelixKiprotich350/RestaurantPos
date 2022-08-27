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
using System.Windows.Shapes;

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for EditOrderItem.xaml
    /// </summary>
    public partial class EditOrderItemQuantity : Window
    {
        public string ReturningAction = "";
        public string ItemServiceType = "";
        public int ReturningQuantity=1;
        public EditOrderItemQuantity()
        {
            InitializeComponent();
        }

       

        private void Buton_Add_Click(object sender, RoutedEventArgs e)
        {
            TextBox_Quantity.Text = (Convert.ToInt32(TextBox_Quantity.Text) + 1).ToString();
        }

        private void Buton_Subtract_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int a = Convert.ToInt32(TextBox_Quantity.Text);
                if (a <= 1)
                {
                    return;
                }
                TextBox_Quantity.Text = (a - 1).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReturningAction = "Update";
                ReturningQuantity = Convert.ToInt32(TextBox_Quantity.Text);
                if ((bool)CheckBox_CarryOut.IsChecked)
                {
                    ItemServiceType = "Out";
                }
                else
                {
                    ItemServiceType = "In";
                }
                this.Close();
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
                if (MessageBox.Show("Are you sure you want to remove this item ?","Message Box",MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    ReturningAction = "Delete";
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckBox_CarryOut_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox_EatIn.IsChecked = false;
        }

        private void CheckBox_EatIn_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox_CarryOut.IsChecked = false;
        }
    }
}
