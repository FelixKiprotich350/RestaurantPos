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
    public partial class AccountsDashboard : Page
    {
        public AccountsDashboard()
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
                List<TicketPaymentMaster> tlist = new List<TicketPaymentMaster>();
                using (var b = new PosDbContext())
                {
                    b.TicketPaymentMaster.AsNoTracking();
                    tlist = b.TicketPaymentMaster.ToList();

                }  
                if ((bool)Checkbox_ByUser.IsChecked)
                {
                    if (Combobox_UserName.SelectedItem != null)
                    {
                         tlist.RemoveAll(b => b.PosUser != ((PosUser)Combobox_UserName.SelectedItem).UserName);
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

        private void GetAccountsTotal(List<TicketPaymentMaster> tlist)
        {
            try
            {
                decimal mpesa = 0;
                decimal cash = 0;
                decimal cards = 0;
                decimal voucher = 0;
                decimal cashbalance = 0;
                decimal unknown = 0;

                var db = new PosDbContext();
                if (tlist.Count <= 0)
                {
                    return;
                }
                var innerGroupJoinQuery = from m in tlist
                                          join t in db.TicketPaymentItem on m.TicketNo equals t.ParentOrderNo
                                          select new { m, t };


                foreach (var x in innerGroupJoinQuery)
                {

                    if (x.t.Method == PosEnums.TicketPaymentMethods.Cash.ToString())
                    {
                        cash += x.t.AmountPaid;
                    }
                    else if (x.t.Method == PosEnums.TicketPaymentMethods.Mpesa.ToString())
                    {
                        mpesa += x.t.AmountPaid;
                    }
                    else if (x.t.Method.ToLower().Contains(PosEnums.TicketPaymentMethods.Card.ToString().ToLower()))
                    {
                        cards += x.t.AmountPaid;
                    }
                    else if (x.t.Method == PosEnums.TicketPaymentMethods.Voucher.ToString())
                    {
                        voucher += x.t.AmountPaid;
                    }
                    else
                    {
                        unknown += x.t.AmountPaid;
                    }
                }
                foreach (var y in tlist)
                {
                    cashbalance += y.TicketBalanceReturned;
                }

                Textbox_CashTotal.Text = (cash-cashbalance).ToString();
                Textbox_Mpesa.Text = mpesa.ToString();
                TextBox_Vouchers.Text = voucher.ToString();
                Textbox_Cards.Text = cards.ToString();
                Textbox_Totals.Text = (cash + mpesa + cards-cashbalance).ToString();
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
