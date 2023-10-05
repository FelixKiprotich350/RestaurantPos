using RestaurantManager.ApplicationFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RestaurantManager.UserInterface.CustomersManagemnt
{
    /// <summary>
    /// Interaction logic for RedeeemSettings.xaml
    /// </summary>
    public partial class RedeeemSettings : Window
    {
        public int RedeemPoints = 0;
        private readonly string custno = null;
        public RedeeemSettings(string cust)
        {
            InitializeComponent();
            custno = cust;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                int debit = 0;
                int credit = 0;
                using (var db = new PosDbContext())
                {
                    //var list = db.CustomerPointsAccount.Where(y => y.CustomerAccNo == custno).ToList();
                    //if (list.Count > 0)
                    //{
                    //    foreach (var x in list)
                    //    {
                    //        debit += x.Debit;
                    //        credit += x.Credit;
                    //    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("No History exists for this Customer!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    //    DialogResult = false;
                    //}
                }
                TextBox_AvailablePoints.Text = (debit - credit).ToString();
                TextBox_BalancePoints.Text = (debit - credit).ToString();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
        }

        private void Button_Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(TextBox_AvailablePoints.Text, out int availablepoints))
                {
                    MessageBox.Show("The available Points is not allowed!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                if (!int.TryParse(TextBox_PointstoRedeem.Text, out int _redeempoints))
                {
                    MessageBox.Show("The Amount entered is not allowed!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                if (_redeempoints > availablepoints)
                {
                    MessageBox.Show("The Amount entered is greater than available Points!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                RedeemPoints = _redeempoints;
                this.DialogResult = true;
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                this.DialogResult = false ;
            }
        }

        private void TextBox_AvailablePoints_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!int.TryParse(TextBox_AvailablePoints.Text, out int availablepoints))
                {
                    MessageBox.Show("The available Points is not allowed!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = false;
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_PointstoRedeem_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       
    }
}
