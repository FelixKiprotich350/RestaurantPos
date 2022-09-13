using RestaurantManager.BusinessModels.WorkPeriod;
using RestaurantManager.MailingPlugin;
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

namespace RestaurantManager.UserInterface.WorkPeriods
{
    /// <summary>
    /// Interaction logic for ViewWorkPeriod.xaml
    /// </summary>
    public partial class VouchersList : Page
    { 
        public VouchersList()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshVouchersList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshVouchersList()
        {
            try
            {
                List<WorkPeriod> workperiods = new List<WorkPeriod>();
                using (var db = new PosDbContext())
                {
                    workperiods = db.WorkPeriod.ToList();
                }
                 
                Datagrid_Workperiods.ItemsSource = workperiods;
                TextBox_TotalCount.Text = Datagrid_Workperiods.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    if (Datagrid_Workperiods.SelectedItem == null)
                    {
                        return;
                    }
                    WorkPeriod o = (WorkPeriod)Datagrid_Workperiods.SelectedItem;
                    if (o == null)
                    {
                        MessageBox.Show("The selected Work Period is not Known!!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    using (var db = new PosDbContext())
                    {
                        if (db.WorkPeriod.Where(x => x.WorkperiodName == o.WorkperiodName && x.WorkperiodStatus == "Open").Count() <= 0)
                        {
                            MessageBox.Show("This Work Period is already closed!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                            RefreshVouchersList();
                            return;
                        }
                    }
                    CloseWorkPeriod c = new CloseWorkPeriod();
                    c.ShowDialog();
                    if ((bool)c.DialogResult)
                    {
                        using (var db = new PosDbContext())
                        {
                            var wp = db.WorkPeriod.Where(a => a.WorkperiodName == o.WorkperiodName).First();
                            wp.WorkperiodStatus = "Closed";
                            wp.ClosedBy = ErpShared.CurrentUser.UserName;
                            wp.ClosingDate = ErpShared.CurrentDate();
                            db.SaveChanges();
                            ////   var  jjj=SendMail();

                            //if (jjj.Result == false)
                            //{
                            //    MessageBox.Show("Failed to send the Email!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            //}
                            MessageBox.Show("Successfully closed the Work Period!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            RefreshVouchersList();
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
