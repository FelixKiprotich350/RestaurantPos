using DatabaseModels.Inventory;
using DatabaseModels.Payments;
using DatabaseModels.WorkPeriod;
using RestaurantManager.ActivityLogs;
using RestaurantManager.GlobalVariables;
using RestaurantManager.UserInterface.WorkPeriods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace RestaurantManager.UserInterface.PosReports.Payments
{
    /// <summary>
    /// Interaction logic for Paymentsmaster.xaml
    /// </summary>
    public partial class Paymentsmaster : Page
    {
        ObservableCollection<TicketPaymentItem> payments = new ObservableCollection<TicketPaymentItem>();
        public Paymentsmaster()
        {
            InitializeComponent();
            Datagrid_PaymentItems.ItemsSource = payments;
        }
         
        private void Datagrid_ProductItems_MouseUp(object sender, MouseButtonEventArgs e)
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
                if (dep is DataGridCell cell)
                {
                    if (cell.Column.DisplayIndex == 1)
                    {
                        if (Datagrid_PaymentItems.SelectedItem == null)
                        {
                            return;
                        } 
                        if(Datagrid_PaymentItems.SelectedItem is TicketPaymentMaster master)
                        {
                            PaymentTransactionDetails P = new PaymentTransactionDetails(master);

                            P.ShowDialog();
                        }
                        
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Textbox_SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox t = (TextBox)sender;
                string filter = t.Text;
                if (Datagrid_PaymentItems.ItemsSource == null)
                {
                    return;
                }
                ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_PaymentItems.ItemsSource);
                if (filter == "")
                {
                    cv.Filter = null;
                }
                else
                {
                    cv.Filter = new Predicate<object>(Contains);
                }
                //if (filter == "")
                //    cv.Filter = null;
                //else
                //{
                //    cv.Filter = o =>
                //    {
                //        MenuProductItem p = o as MenuProductItem;
                //        return p.ProductName.ToLower().Contains(filter.ToLower()) || p.AvailabilityStatus.ToString().ToLower().Contains(filter.ToLower());
                //    };
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool Contains(object de)
        {
            TicketPaymentItem item = de as TicketPaymentItem;
            return item.MasterTransNo.ToLower().Contains(Textbox_SearchBox.Text.ToLower()) |
                item.ParentSourceRef.ToLower().Contains(Textbox_SearchBox.Text.ToLower()) |
                item.Method.ToLower().Contains(Textbox_SearchBox.Text.ToLower()) |
                item.Workperiod.ToLower().Contains(Textbox_SearchBox.Text.ToLower());

        }
         
        void LoadPayments(WorkPeriod wp, DateTime? startdate, DateTime? enddate)
        {
            try
            {
                decimal total = 0;
                decimal cash = 0;
                decimal mpesa = 0;
                decimal cards = 0;
                decimal invoice = 0;
                var db = new PosDbContext();
                var final = db.TicketPaymentItem.AsNoTracking().ToList();
                if (wp != null)
                {
                    final.RemoveAll(w => w.Workperiod != wp.WorkperiodName);
                }
                if (startdate != null)
                {
                    final.RemoveAll(w => w.PaymentDate <startdate);
                }
                if (enddate != null)
                {
                    final.RemoveAll(w => w.PaymentDate > enddate);
                }
                payments = new ObservableCollection<TicketPaymentItem>(final);
                var forsum = final;
                total = forsum.Sum(t => t.AmountUsed);
                cash = forsum.Where(k=>k.Method==PosEnums.TicketPaymentMethods.Cash.ToString()).Sum(t => t.AmountUsed);
                mpesa = forsum.Where(k=>k.Method==PosEnums.TicketPaymentMethods.Mpesa.ToString()).Sum(t => t.AmountUsed);
                cards = forsum.Where(k=>k.Method==PosEnums.TicketPaymentMethods.Card.ToString()).Sum(t => t.AmountUsed);
                invoice = forsum.Where(k=>k.Method==PosEnums.TicketPaymentMethods.Invoice.ToString()).Sum(t => t.AmountUsed);
                TextBox_Total.Text = total.ToString();
                TextBox_Cards.Text = cards.ToString();
                TextBox_Mpesa.Text = mpesa.ToString();
                TextBox_Cash.Text = cash.ToString();
                TextBox_Invoice.Text = invoice.ToString();
                Datagrid_PaymentItems.ItemsSource = payments;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
         
        private void Button_ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //new DateTime(2023, 10, 28, 17, 34, 11)
                var db = new PosDbContext();
                  
                var completed = db.TicketPaymentItem.Where(k=>k.Method==GlobalVariables.PosEnums.TicketPaymentMethods.Invoice.ToString()).ToList();
                var trans = db.InvoicesMaster.ToList();
                
                MessageBox.Show("done");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_SelectWorkPeriod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectWorkPeriod w = new SelectWorkPeriod();
                if ((bool)w.ShowDialog())
                {
                    if (w.SelectedWorkperiod != null)
                    {
                        Button_SelectWorkPeriod.Tag = w.SelectedWorkperiod;
                        Button_SelectWorkPeriod.Content = w.SelectedWorkperiod.WorkperiodName;
                    }
                    else
                    {
                        Button_SelectWorkPeriod.Tag = null;
                        Button_SelectWorkPeriod.Content = "All";
                    }
                }
                else
                {
                    Button_SelectWorkPeriod.Tag = null;
                    Button_SelectWorkPeriod.Content = "All";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 
                DateTime? startdate = null;
                DateTime? enddate = null;
                WorkPeriod wperiod = null;
                if (Button_SelectWorkPeriod.Content.ToString() == "All")
                {
                    wperiod = (WorkPeriod)Button_SelectWorkPeriod.Tag;
                }
                else
                {
                    wperiod = (WorkPeriod)Button_SelectWorkPeriod.Tag;
                }
                if (Datepicker_Startdate.SelectedDate != null)
                {
                    startdate = Datepicker_Startdate.SelectedDate;
                }
                if (Datepicker_Enddate.SelectedDate != null)
                {
                    enddate = Datepicker_Enddate.SelectedDate;
                }
                LoadPayments(wperiod, startdate, enddate);
                ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(),"Viewed Payments Report ","Filtered by workperiod ="+wperiod?.WorkperiodName+",startdate="+startdate?.ToString()+", enddate="+enddate?.ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Datagrid_PaymentItems_MouseUp(object sender, MouseButtonEventArgs e)
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
                if (dep is DataGridCell cell)
                {
                    if (Datagrid_PaymentItems.SelectedItem == null)
                    {
                        return;
                    }
                    if (cell.Column.DisplayIndex == 0)
                    {
                       
                        if (Datagrid_PaymentItems.SelectedItem is TicketPaymentItem item)
                        {

                            var master = new PosDbContext().TicketPaymentMaster.AsNoTracking().FirstOrDefault(k => k.TransNo == item.MasterTransNo);
                            if (master!=null)
                            {
                                PaymentTransactionDetails P = new PaymentTransactionDetails(master);

                                P.ShowDialog();
                                ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Viewed Payment Details ", "Reference number="+master.TransNo+",RecordID="+master.PaymentMasterGuid);

                            }
                        }

                    }
                    else if (cell.Column.DisplayIndex == 1)
                    {
                       
                        if (Datagrid_PaymentItems.SelectedItem is TicketPaymentItem item)
                        {

                            var master = new PosDbContext().OrderMaster.AsNoTracking().FirstOrDefault(k => k.OrderNo == item.ParentSourceRef);
                            if (master!=null)
                            {
                                TicketDetails P = new TicketDetails(master);

                                P.ShowDialog();
                                ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Viewed Order Details ", "Reference number=" + master.OrderNo + ",RecordID=" + master.OrderGuid);

                            }
                        }

                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
