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
using System.Windows.Shapes;
using System.Collections.ObjectModel; 
using DatabaseModels.Payments;

namespace RestaurantManager.UserInterface.TicketPayments
{
    /// <summary>
    /// Interaction logic for PaymentsWindow.xaml
    /// </summary>
    public partial class PaymentsWindow : Window
    {
        public string SelectedPaymentMethod = "";
        public decimal AmountToPay = 0;
        public decimal AmountPaid = 0;
        public decimal balance = 0;
        public decimal discount = 0;
        public bool IsInitializing = true;
        public ObservableCollection<PaymentMethod> payments = new ObservableCollection<PaymentMethod>();
        public PaymentsWindow()
        {
            InitializeComponent();
            Datagrid_Payments.ItemsSource = payments;
          

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AmountToPay <= 0)
                {
                    MessageBox.Show("The amount must be greater than 0!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Close();
                }
                Stackpanel_CardGroup.Visibility = Visibility.Collapsed; 
                Btn_Complete.IsEnabled = false;
                IsInitializing = false;
                LoadBankCards();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadBankCards()
        {
            try
            {
                var list = SharedVariables.ClientInfo().AcceptedCards.Split(',').Where(x => x != "").ToList();
                list.ForEach(k => k.Trim());
                Combobos_BanksList.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #region Number Pad

        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(3);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(4);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(5);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(6);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_7_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(7);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_8_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_9_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(9);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_0_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NummberClicked(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
        private void NummberClicked(int num)
        {

            try
            {
                if(SelectedPaymentMethod == "")
                {
                    MessageBox.Show("Select Payment Method!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if(SelectedPaymentMethod == "Discount")
                {
                    if (!Textbox_DiscountOffered.IsReadOnly)
                    {
                        Textbox_DiscountOffered.Text += num;
                    }
                }
                else
                {
                    Textbox_AmountToAdd.Text += num;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedPaymentMethod != "Discount" )
                {
                    Textbox_AmountToAdd.Clear();
                }
                else
                {
                    Textbox_DiscountOffered.Clear();
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //payment methods
        private void Btn_Cash_Click(object sender, RoutedEventArgs e)
        {
            PaymentMethodSelected(PosEnums.TicketPaymentMethods.Cash.ToString());
        }

        private void Btn_Mpesa_Click(object sender, RoutedEventArgs e)
        {
            PaymentMethodSelected(PosEnums.TicketPaymentMethods.Mpesa.ToString());
        }

        private void Btn_Cards_Click(object sender, RoutedEventArgs e)
        {
            PaymentMethodSelected(PosEnums.TicketPaymentMethods.Card.ToString());
        }

        private void Btn_Voucher_Click(object sender, RoutedEventArgs e)
        {
            PaymentMethodSelected(PosEnums.TicketPaymentMethods.Voucher.ToString());
        }

        private void PaymentMethodSelected(string method)
        { 
            try
            {
                Label_SelectedModeofpayment.Content = method+" Amount";
                if (method == PosEnums.TicketPaymentMethods.Cash.ToString())
                {
                    SelectedPaymentMethod = method;
                    Stackpanel_CardGroup.Visibility = Visibility.Collapsed;
                    
                }
                else if (method == PosEnums.TicketPaymentMethods.Card.ToString())
                {
                    SelectedPaymentMethod = method;
                    Stackpanel_CardGroup.Visibility = Visibility.Visible;
                }
                else if (method == PosEnums.TicketPaymentMethods.Mpesa.ToString())
                {
                    SelectedPaymentMethod = method;
                    Stackpanel_CardGroup.Visibility = Visibility.Collapsed;
                }
                else if (method == PosEnums.TicketPaymentMethods.Voucher.ToString())
                {
                    SelectedPaymentMethod = method;
                    Stackpanel_CardGroup.Visibility = Visibility.Collapsed;
                    Label_SelectedModeofpayment.Content = "Enter Voucher Number";
                }
                else
                {
                    SelectedPaymentMethod = "";
                    Label_SelectedModeofpayment.Content = "No Payment Selected";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_AddPayment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(Textbox_AmountToAdd.Text.Trim(), out int amount))
                {
                    MessageBox.Show("Enter Valid Amount!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (SelectedPaymentMethod == "" | SelectedPaymentMethod == "Discount")
                {
                    MessageBox.Show("Select Payment Method!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                PaymentMethod pm = new PaymentMethod
                {
                    Amount = amount,
                    PaymentMethodName = SelectedPaymentMethod,
                    SecondaryRefference = "None"
                };
                if (pm.PaymentMethodName == PosEnums.TicketPaymentMethods.Card.ToString())
                {
                    if (Combobos_BanksList.Text.Trim() == "")
                    {
                        MessageBox.Show("Select CardName!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    pm.PrimaryRefference = Combobos_BanksList.Text;
                }
                else
                {
                    pm.PrimaryRefference = "None";
                }
                payments.Add(pm);
                Calculatetotal();
                Textbox_AmountToAdd.Text = "";
                SelectedPaymentMethod = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Calculatetotal()
        {
            try
            {
                AmountPaid = 0;
                if (IsInitializing)
                {
                    return;
                }
                foreach (var x in payments)
                {
                    AmountPaid += x.Amount;
                }
                if(int.TryParse(Textbox_DiscountOffered.Text.Trim(),out int disc))
                {
                    discount = disc;
                }
                else
                {
                    discount = 0;
                }
                balance = AmountPaid - (AmountToPay - discount);
                Textblock_Amountpaid.Text = AmountPaid.ToString("N2");
                Textblock_Balance.Text = balance.ToString("N2");
                if (AmountPaid + discount >= AmountToPay)
                {
                    Btn_Complete.IsEnabled = true;
                }
                else
                {
                    Btn_Complete.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Combobox_SetDiscount_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Textbox_DiscountOffered.IsReadOnly = false;
                Textbox_DiscountOffered.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Combobox_SetDiscount_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Textbox_DiscountOffered.Text = "0";
                Textbox_DiscountOffered.IsReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_Complete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Calculatetotal();
                if (AmountPaid >= AmountToPay-discount)
                {
                    DialogResult = true;

                }
                else
                {
                    MessageBox.Show("Failed! Insufficient Amount!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    DialogResult = false; 

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
        }

        private void Textbox_DiscountOffered_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedPaymentMethod = "Discount";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Textbox_DiscountOffered_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.TryParse(Textbox_DiscountOffered.Text.Trim(), out _))
                {
                    Calculatetotal();
                }
                else
                {
                    MessageBox.Show("Invalid Amount!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Textbox_DiscountOffered.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
