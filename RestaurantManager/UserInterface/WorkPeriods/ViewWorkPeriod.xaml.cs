using RestaurantManager.BusinessModels.WorkPeriod;
using RestaurantManager.MailingPlugin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantManager.UserInterface.WorkPeriods
{
    /// <summary>
    /// Interaction logic for ViewWorkPeriod.xaml
    /// </summary>
    public partial class ViewWorkPeriod : Page
    {
        public ViewWorkPeriod()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshWorkPeriods();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshWorkPeriods()
        {
            try
            {
                List<WorkPeriod> workperiods = new List<WorkPeriod>();
                using (var db = new PosDbContext())
                {
                    workperiods = db.WorkPeriod.ToList();
                }
                foreach (var x in workperiods)
                {
                    x.TotalTicketsCount = TicketCount("", x.WorkperiodName, true); 
                }
                Datagrid_Workperiods.ItemsSource = workperiods;
                TextBox_TotalCount.Text = Datagrid_Workperiods.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        int TicketCount(string tstatus,string workperiod,bool CountAll)
        {
            try
            {
                int count = 0;
                if (CountAll)
                {
                    using (var db = new PosDbContext())
                    {
                        count = db.OrderMaster.Where(x => x.Workperiod == workperiod).Count();
                    }
                    return count;
                }
                
                using (var db=new PosDbContext())
                {
                    count = db.OrderMaster.Where(x => x.Workperiod == workperiod & x.OrderStatus == tstatus).Count();
                }
                    return count;
            }
            catch
            {
                return -1;
            }
        }

       
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
                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
                    }

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        { 
            
        }
    }
}
