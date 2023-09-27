using DatabaseModels.WorkPeriod;
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

namespace RestaurantManager.UserInterface.PosReports
{
    /// <summary>
    /// Interaction logic for SelectWorkPeriod.xaml
    /// </summary>
    public partial class SelectWorkPeriod : Window
    { 
        public WorkPeriod SelectedWorkperiod = null;
        public List<WorkPeriod> AllWorkPeriods = new List<WorkPeriod>();
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
                    AllWorkPeriods = db.WorkPeriod.AsNoTracking().ToList();
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
    }
}
