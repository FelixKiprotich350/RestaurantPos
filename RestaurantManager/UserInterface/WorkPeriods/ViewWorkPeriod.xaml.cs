using RestaurantManager.BusinessModels.WorkPeriod;
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
                using (var db = new PosDbContext())
                {
                    Datagrid_Workperiods.ItemsSource = db.WorkPeriod.ToList();
                }
                TextBox_TotalCount.Text = Datagrid_Workperiods.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_NewWorkPeriod_Click(object sender, RoutedEventArgs e)
        { 
            try
            {
                CreateWorkPeriod create = new CreateWorkPeriod(); 
                if (create.ShowDialog() == false)
                {
                    return;
                }
                WorkPeriod w = new WorkPeriod
                {
                    WorkPeriodGuid = Guid.NewGuid().ToString(),
                    WorkperiodName = create.Textbox_PeriodName.Text,
                    WorkperiodDescription = create.Textbox_Description.Text,
                    Openedby = ErpShared.CurrentUser.UserPIN.ToString(),
                    ClosedBy = "",
                    WorkperiodStatus = "Open",
                    OpeningDate = ErpShared.CurrentDate(),
                    ClosingDate = ErpShared.CurrentDate()
                };
                using (var db = new PosDbContext())
                {
                    db.WorkPeriod.Add(w);
                    db.SaveChanges();
                    MessageBox.Show("Success. Item Saved.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshWorkPeriods();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Datagrid_Workperiods_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseWorkPeriod c = new CloseWorkPeriod();
            c.ShowDialog();
        }
    }
}
