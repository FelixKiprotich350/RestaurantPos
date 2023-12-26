using DatabaseModels.WorkPeriod;
using RestaurantManager.ActivityLogs;
using RestaurantManager.GlobalVariables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace RestaurantManager.UserInterface.WorkPeriods
{
    /// <summary>
    /// Interaction logic for SelectWorkPeriod.xaml
    /// </summary>
    public partial class SelectWorkPeriod : Window
    { 
        public WorkPeriod SelectedWorkperiod = null;
        public ObservableCollection<WorkPeriod> AllWorkPeriods = new ObservableCollection<WorkPeriod>();
        public SelectWorkPeriod()
        {
            InitializeComponent(); 
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    AllWorkPeriods = new ObservableCollection<WorkPeriod>(db.WorkPeriod.AsNoTracking().OrderByDescending(k => k.OpeningDate).ToList());
                }
                Datagrid_AllWorkPeriods.ItemsSource = AllWorkPeriods;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_Select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedWorkperiod = AllWorkPeriods.FirstOrDefault(k => k.IsSelected);
                if (SelectedWorkperiod!=null)
                {
                    ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Selected WorkPeriod", "Workperiod Name:" + SelectedWorkperiod.WorkperiodName);

                    DialogResult = true;
                }

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
            finally
            {
                Close();
            }
        }

        private void Button_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedWorkperiod = null;
                ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Selected WorkPeriod", "Workperiod Name:" + "All");
                DialogResult = true;

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
            finally
            {
                Close();
            }
        }

        private void Datagrid_AllWorkPeriods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                foreach (var x in AllWorkPeriods)
                {
                    x.IsSelected = false;
                }
                if (Datagrid_AllWorkPeriods.SelectedItem is WorkPeriod wp)
                {

                    if (wp.IsSelected)
                    {
                        wp.IsSelected = false;
                    }
                    else
                    {
                        wp.IsSelected = true;
                    }
                }
                Datagrid_AllWorkPeriods.Items.Refresh();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
