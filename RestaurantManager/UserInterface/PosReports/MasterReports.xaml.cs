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
    /// Interaction logic for MasterReports.xaml
    /// </summary>
    public partial class MasterReports : Page
    { 
        public MasterReports()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalVariables.SharedVariables.Main_Window.Category_Submenu.Visibility = Visibility.Collapsed;
            Listview_ReportHeads.SelectedIndex = 0;
            Frame_ReportArea.Content = new SalesReport();
        }

        private void Listview_ReportHeads_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Listview_ReportHeads.SelectedItem == null)
                {
                    Listview_ReportHeads.SelectedIndex = 0;
                    return;
                }
                ListViewItem Lv = Listview_ReportHeads.SelectedItem as ListViewItem;
                if(Listview_ReportHeads.SelectedIndex==0)
                {
                    Frame_ReportArea.Content = new SalesReport();
                }
                else if (Listview_ReportHeads.SelectedIndex == 1)
                {
                    Frame_ReportArea.Content = new TicketsReports();
                }
                else if (Listview_ReportHeads.SelectedIndex == 2)
                {
                    Frame_ReportArea.Content = new PaymentsReport();
                }
                else if (Listview_ReportHeads.SelectedIndex == 3)
                {
                    Frame_ReportArea.Content = new UsersReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
