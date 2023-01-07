using RestaurantManager.ApplicationFiles;
using RestaurantManager.BusinessModels.Payments;
using RestaurantManager.BusinessModels.WorkPeriod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for AccountTicketsPayments.xaml
    /// </summary>
    public partial class AccountTicketsPayments : Page
    {
        public AccountTicketsPayments()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshPayments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshPayments()
        {
            try
            {
                List< TicketPaymentMaster> item = new List<TicketPaymentMaster>();
                using (var db = new PosDbContext())
                {
                    item = db.TicketPaymentMaster.ToList();
                }
                //foreach (var x in item)
                //{
                //    x.TotalTicketsCount = TicketCount("", x.WorkperiodName, true);
                //}
                Datagrid_PaymentsView.ItemsSource = item;
                TextBox_TotalCount.Text = Datagrid_PaymentsView.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //int TicketCount(string tstatus, string workperiod, bool CountAll)
        //{
        //    try
        //    {
        //        int count = 0;
        //        if (CountAll)
        //        {
        //            using (var db = new PosDbContext())
        //            {
        //                count = db.OrderMaster.Where(x => x.Workperiod == workperiod).Count();
        //            }
        //            return count;
        //        }

        //        using (var db = new PosDbContext())
        //        {
        //            count = db.OrderMaster.Where(x => x.Workperiod == workperiod & x.OrderStatus == tstatus).Count();
        //        }
        //        return count;
        //    }
        //    catch
        //    {
        //        return -1;
        //    }
        //}

        private void Datagrid_Workperiods_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DependencyObject dep = (DependencyObject)e.OriginalSource;

                // iteratively traverse the visual tree
                while ((dep != null) & !(dep is DataGridCell) & !(dep is DataGridColumnHeader))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }

                if (dep == null)
                {
                    return;
                }
                if (dep is DataGridCell)
                {
                    if (Datagrid_PaymentsView.SelectedItem == null)
                    {
                        return;
                    }
                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshPayments();
        }
    }
}
