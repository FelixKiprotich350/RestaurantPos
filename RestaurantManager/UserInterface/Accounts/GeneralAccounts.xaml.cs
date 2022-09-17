using RestaurantManager.BusinessModels.Payments;
using RestaurantManager.BusinessModels.Security;
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

namespace RestaurantManager.UserInterface.Accounts
{
    /// <summary>
    /// Interaction logic for Receivables.xaml
    /// </summary>
    public partial class GeneralAccounts : Page
    {
        public GeneralAccounts()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                 
                    using (var b = new PosDbContext())
                    {
                       Combobox_UserName.ItemsSource = b.PosUser.ToList();
                         
                    }
                GetAccountsTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearTotals();
                List<TicketPaymentItem> tlist = new List<TicketPaymentItem>();
                using (var b = new PosDbContext())
                {
                    tlist = b.TicketPaymentItem.ToList();

                }
                if ((bool)Checkbox_ByUser.IsChecked)
                {
                    if (Combobox_UserName.SelectedItem != null)
                    {
                         tlist.RemoveAll(b => b.ReceivingUsername != ((PosUser)Combobox_UserName.SelectedItem).UserName);
                    }
                    else
                    {
                        MessageBox.Show("Select UserName!","Message Box",MessageBoxButton.OK);
                        return;
                    }
                }
                if ((bool)Checkbox_PaymentByDate.IsChecked)
                {
                    if ((bool)RadioButton_SingleDay.IsChecked)
                    {
                        DateTime sday = (DateTime)DatePicker_SingleDay.SelectedDate;
                        if (sday == null)
                        {
                            MessageBox.Show("Select Date Parameter!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        if (sday.ToString().Trim() == "")
                        {
                            MessageBox.Show("Select Date Parameter!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        tlist.RemoveAll(b => b.PaymentDate.ToShortDateString() != sday.ToShortDateString());

                    }
                    else if ((bool)Radiobutton_Period.IsChecked)
                    {
                        DateTime from_day = (DateTime)Datepicker_From.SelectedDate;
                        DateTime to_day = (DateTime)Datepicker_To.SelectedDate;
                        if (from_day == null | to_day == null)
                        {
                            MessageBox.Show("Select the Start Date and End Date of the Period!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        if (from_day.ToShortDateString().Trim() == "" | to_day.ToShortDateString().Trim() == "")
                        {
                            MessageBox.Show("Select the Start Date and End Date of the Period!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        tlist.RemoveAll(b => b.PaymentDate.Date < from_day.Date && b.PaymentDate > to_day.Date);
                    }
                    else
                    {
                        MessageBox.Show("Select Date Parameter!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                GetAccountsTotal(tlist);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearTotals()
        {
            Textbox_Cards.Text="0.00";
            Textbox_Mpesa.Text="0.00";
            TextBox_Vouchers.Text="0.00";
            Textbox_CashTotal.Text="0.00";
            Textbox_Totals.Text="0.00";
        }

        private void GetAccountsTotal(List<TicketPaymentItem> tlist)
        {
            try
            {
                decimal mpesa = 0;
                decimal cash = 0;
                decimal cards = 0;
                decimal voucher = 0;
                decimal unknown = 0; 
                
                if (tlist.Count <= 0)
                {
                    return;
                }
                foreach (TicketPaymentItem t in tlist)
                {
                    if (t.Method == PosEnums.TicketPaymentMethods.Cash.ToString())
                    {
                        cash += t.AmountPaid;
                    }
                    else if (t.Method == PosEnums.TicketPaymentMethods.Mpesa.ToString())
                    {
                        mpesa += t.AmountPaid;
                    }
                    else if (t.Method.ToLower().Contains(PosEnums.TicketPaymentMethods.Card.ToString().ToLower()))
                    {
                        cards += t.AmountPaid;
                    }
                    else if (t.Method == PosEnums.TicketPaymentMethods.Voucher.ToString())
                    {
                        voucher += t.AmountPaid;
                    }
                    else
                    {
                        unknown += t.AmountPaid;
                    }
                }
                Textbox_CashTotal.Text = cash.ToString();
                Textbox_Mpesa.Text = mpesa.ToString();
                TextBox_Vouchers.Text = voucher.ToString();
                Textbox_Cards.Text = cards.ToString();
                Textbox_Totals.Text = (cash + mpesa + cards).ToString();
                if (unknown > 0)
                {
                    MessageBox.Show("The following amount cannot be accounted for!\n" + unknown.ToString("N2"), "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GetAccountsTotal( )
        {
            try
            {
                decimal mpesa = 0;
                decimal cash = 0;
                decimal cards = 0;
                decimal voucher = 0;
                decimal unknown = 0;
                using (var db = new PosDbContext())
                { 
                    var innerGroupJoinQuery = from m in db.TicketPaymentMaster
                                              join t in db.TicketPaymentItem on m.TicketNo equals t.ParentOrderNo 
                                              select new { m,t };
                    
                    foreach (var x in innerGroupJoinQuery)
                    {
                        if (x.t.Method == "Cash")
                        {
                            cash += x.t.AmountPaid;
                        }
                        
                    } 
                }
                
               
                Textbox_CashTotal.Text = cash.ToString();
                Textbox_Mpesa.Text = mpesa.ToString();
                TextBox_Vouchers.Text = voucher.ToString();
                Textbox_Cards.Text = cards.ToString();
                Textbox_Totals.Text = (cash + mpesa + cards).ToString();
                if (unknown > 0)
                {
                    MessageBox.Show("The following amount cannot be accounted for!\n" + unknown.ToString("N2"), "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Cash_Click(object sender, RoutedEventArgs e)
        { 

        }

        private void Button_Voucher_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Cards_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Mpesa_Click(object sender, RoutedEventArgs e)
        {

        }
 
        private void RadioButton_SingleDay_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                DatePicker_SingleDay.SelectedDate = null;
                Datepicker_From.SelectedDate = null;
                Datepicker_To.SelectedDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Radiobutton_Period_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                DatePicker_SingleDay.SelectedDate = null;
                Datepicker_From.SelectedDate = null;
                Datepicker_To.SelectedDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
 
        private void Checkbox_PaymentByDate_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                DatePicker_SingleDay.SelectedDate = null;
                Datepicker_From.SelectedDate = null;
                Datepicker_To.SelectedDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
 
        private void Checkbox_ByUser_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Combobox_UserName.SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
