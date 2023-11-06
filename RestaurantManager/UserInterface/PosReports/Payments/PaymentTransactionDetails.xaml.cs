using DatabaseModels.Payments;
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

namespace RestaurantManager.UserInterface.PosReports.Payments
{
    /// <summary>
    /// Interaction logic for PaymentTransactionDetails.xaml
    /// </summary>
    public partial class PaymentTransactionDetails : Window
    {
        readonly TicketPaymentMaster masterpayment = null;
        public PaymentTransactionDetails(TicketPaymentMaster master)
        {
            InitializeComponent();
            masterpayment = master;
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Textbox_TransactionNumber.Text = masterpayment.TransNo;
                Textbox_Workperiodd.Text = masterpayment.WorkPeriod; 
                Textbox_Date.Text = masterpayment.PaymentDate.ToLongDateString();
                Textbox_postedby.Text = masterpayment.PosUser;
                Textbox_amountcharged.Text = masterpayment.TotalAmountCharged.ToString("N2");
                Textbox_TotalPaid.Text = masterpayment.TotalAmountPaid.ToString("N2");
                Textbox_change.Text = masterpayment.TicketBalanceReturned.ToString("N2");
                var db = new PosDbContext();
                var items = db.TicketPaymentItem.AsNoTracking().Where(k => k.MasterTransNo == masterpayment.TransNo).ToList();
                Datagrid_PaymentItems.ItemsSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Reprint_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
