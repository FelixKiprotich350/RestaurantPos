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
    public partial class ServiceType : Window
    { 
        public string ItemServiceType = "";
        public int ItemQty = 1;
        public ServiceType()
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
 

        private void Button_Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                if ((bool)CheckBox_CarryOut.IsChecked)
                {
                    ItemServiceType = "Out"; 
                    CheckBox_EatIn.IsChecked = false;

                }
                else
                {
                    ItemServiceType = "In"; 
                    CheckBox_CarryOut.IsChecked = false;
                }
                ItemQty = Convert.ToInt32(TextBox_Quantity.Text);
                this.DialogResult = true;
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

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox_Quantity.Text = "1";
        }
    }
}
