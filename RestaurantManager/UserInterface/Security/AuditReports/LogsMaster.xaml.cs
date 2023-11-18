using DatabaseModels.Inventory;
using DatabaseModels.SystemLogs;
using DatabaseModels.WorkPeriod;
using RestaurantManager.GlobalVariables;
using RestaurantManager.UserInterface.PosReports;
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

namespace RestaurantManager.UserInterface.Security.AuditReports
{
    /// <summary>
    /// Interaction logic for Paymentsmaster.xaml
    /// </summary>
    public partial class LogsMaster : Page
    {
        ObservableCollection<DBChangeLog> dbchangelogs = new ObservableCollection<DBChangeLog>();
        ObservableCollection<UserActivityLog> useractivitylogs = new ObservableCollection<UserActivityLog>();
        public LogsMaster()
        {
            InitializeComponent();
          
        }
      
        private void Textbox_SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Tabcontrol_LogsContainer.SelectedIndex == 0)
                {
                    TextBox t = (TextBox)sender;
                    string filter = t.Text;
                    if (Datagrid_UserActivityLogs.ItemsSource == null)
                    {
                        return;
                    }
                    ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_UserActivityLogs.ItemsSource);
                    if (filter == "")
                    {
                        cv.Filter = null;
                    }
                    else
                    {
                        cv.Filter = new Predicate<object>(UserActivityContains);
                    }

                }
                if (Tabcontrol_LogsContainer.SelectedIndex == 1)
                {
                    TextBox t = (TextBox)sender;
                    string filter = t.Text;
                    if (Datagrid_Dbchangelogs.ItemsSource == null)
                    {
                        return;
                    }
                    ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_Dbchangelogs.ItemsSource);
                    if (filter == "")
                    {
                        cv.Filter = null;
                    }
                    else
                    {
                        cv.Filter = new Predicate<object>(DbChangeLogContains);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool UserActivityContains(object de)
        {
            UserActivityLog item = de as UserActivityLog;
            return item.LogID.ToString().Contains(Textbox_SearchBox.Text.ToLower()) |
                item.Logtype.ToLower().Contains(Textbox_SearchBox.Text.ToLower()) |
                item.SystemUser.ToLower().Contains(Textbox_SearchBox.Text.ToLower()) |
                item.Description.ToLower().Contains(Textbox_SearchBox.Text.ToLower()) |
                item.Parameters.ToLower().Contains(Textbox_SearchBox.Text.ToLower());
        }

        public bool DbChangeLogContains(object de)
        {
            DBChangeLog item = de as DBChangeLog;
            return item.Id.ToString().Contains(Textbox_SearchBox.Text.ToLower()) |
                item.LogActionType.ToLower().Contains(Textbox_SearchBox.Text.ToLower()) |
                item.OldValue.ToLower().Contains(Textbox_SearchBox.Text.ToLower()) |
                item.NewValue.ToLower().Contains(Textbox_SearchBox.Text.ToLower()) |
                item.PropertyName.ToLower().Contains(Textbox_SearchBox.Text.ToLower()) |
                item.SystemUser.ToLower().Contains(Textbox_SearchBox.Text.ToLower()) |
                item.EntityName.ToLower().Contains(Textbox_SearchBox.Text.ToLower()) |
                item.PrimaryKeyValue.ToLower().Contains(Textbox_SearchBox.Text.ToLower());
        }

        void LoadLogs( DateTime? startdate, DateTime? enddate)
        {
            try
            {
                var db = new PosDbContext();
                if (Tabcontrol_LogsContainer.SelectedIndex == 0)
                {
                    var useractivity = db.UserActivityLog.AsNoTracking().ToList();
                    useractivitylogs = new ObservableCollection<UserActivityLog>(useractivity);
                    Datagrid_UserActivityLogs.ItemsSource = useractivitylogs;
                }
                else if (Tabcontrol_LogsContainer.SelectedIndex == 1)
                {
                    var dblogs = db.DBChangeLog.AsNoTracking().ToList();
                    dbchangelogs = new ObservableCollection<DBChangeLog>(dblogs);
                    Datagrid_Dbchangelogs.ItemsSource = dbchangelogs;
                }
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
         
        private void Button_Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 
                DateTime? startdate = null;
                DateTime? enddate = null; 
                if (Datepicker_Startdate.SelectedDate != null)
                {
                    startdate = Datepicker_Startdate.SelectedDate;
                }
                if (Datepicker_Enddate.SelectedDate != null)
                {
                    enddate = Datepicker_Enddate.SelectedDate;
                }
                LoadLogs(startdate, enddate);
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
         
    }
}
