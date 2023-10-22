using DatabaseModels.OrderTicket;
using DatabaseModels.WorkPeriod;
using RestaurantManager.ApplicationFiles; 
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

namespace RestaurantManager.UserInterface.PosReports
{
    /// <summary>
    /// Interaction logic for TicketsReports.xaml
    /// </summary>
    public partial class TicketsReports : Page
    {
        List<OrderMaster> MainList = new List<OrderMaster>();
        List<OrderItem> MainList_VoidedItems = new List<OrderItem>();
        public TicketsReports()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TabControl_TicketsTabControl.SelectedIndex = 0;
        }

        private void Button_ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button_SelectWorkPeriod.Content = "All";
                Datepicker_Startdate.SelectedDate = null;
                Datepicker_Enddate.SelectedDate = null;
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
                int item = TabControl_TicketsTabControl.SelectedIndex;
                ClearLists();
                DateTime? startdate = null;
                DateTime? enddate = null;
                WorkPeriod wperiod = null;
                if (Button_SelectWorkPeriod.Content.ToString() == "All")
                {
                    if (Button_SelectWorkPeriod.Tag != null)
                    {
                        if (Button_SelectWorkPeriod.Tag.GetType() == typeof(WorkPeriod))
                        {
                            wperiod = (WorkPeriod)Button_SelectWorkPeriod.Tag;
                        }
                    }

                }
                if (Datepicker_Startdate.SelectedDate != null)
                {
                    startdate = Datepicker_Startdate.SelectedDate;
                }
                if (Datepicker_Enddate.SelectedDate != null)
                {
                    enddate = Datepicker_Enddate.SelectedDate;
                }
                LoadItems(wperiod, startdate, enddate);
                TabControl_TicketsTabControl.SelectedIndex = -1;
                TabControl_TicketsTabControl.SelectedIndex = item;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearLists()
        {
            try
            {
                MainList.Clear();
                MainList_VoidedItems.Clear();
                //grids
                Datagrid_CompletedTickets.ItemsSource = null;
                Datagrid_CompletedTickets.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadItems(WorkPeriod wp, DateTime? startdate, DateTime? enddate)
        {
            try
            {
                var db = new PosDbContext(); 
                var t = db.OrderMaster.AsNoTracking().ToList();
                MainList_VoidedItems = db.OrderItem.AsNoTracking().ToList();
                if (wp != null)
                {
                    t = t.Where(a => a.Workperiod == wp.WorkperiodName).ToList();
                }
                if (startdate != null && enddate != null)
                {
                    t = t.Where(a => a.PaymentDate >= startdate && a.PaymentDate <= enddate).ToList();
                }
                MainList = t;
                MessageBox.Show("Loading Done!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_SelectWorkPeriod_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void TabControl_TicketsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int position = TabControl_TicketsTabControl.SelectedIndex;
                if (position == -1)
                {
                    return;
                }
                if (position == 0)
                {
                    var completed = MainList.Where(k => k.OrderStatus == PosEnums.OrderTicketStatuses.Completed.ToString());
                    if (completed.Count() > 0)
                    {
                        Label_CompletedTicketsCount_Count.Content = completed.Count().ToString();
                        Datagrid_CompletedTickets.ItemsSource = MainList;
                    }
                }
                else if (position == 1)
                {
                    var merged = MainList.Where(k => k.OrderStatus == "Merged");
                    if (merged.Count() > 0)
                    {
                        Label_CompletedTicketsCount_Count.Content = merged.Count().ToString();
                        Datagrid_MergedTickets.ItemsSource = merged;
                    }
                }
                else if (position == 2)
                {
                    var voided = MainList_VoidedItems;
                    if (voided.Count() > 0)
                    {
                        Label_VoidedItemsCount_Count.Content = voided.Count().ToString();
                        Datagrid_VoidedItemsTickets.ItemsSource = voided;
                    }
                }
                else if (position == 3)
                {
                    var voideditems = MainList.Where(k => k.OrderStatus == PosEnums.OrderTicketStatuses.Voided.ToString());
                    if (voideditems.Count() > 0)
                    {
                        Label_VoidedTicketsCount_Count.Content = voideditems.Count().ToString();
                        Datagrid_VoidedTickets.ItemsSource = voideditems;
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
