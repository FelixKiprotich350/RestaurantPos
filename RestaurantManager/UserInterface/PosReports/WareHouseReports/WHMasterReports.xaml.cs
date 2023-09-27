using RestaurantManager.UserInterface.PosReports.WareHouseReports;
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

namespace RestaurantManager.UserInterface.WareHouseReports
{
    /// <summary>
    /// Interaction logic for MasterReports.xaml
    /// </summary>
    public partial class WHMasterReports : Page
    { 
        public WHMasterReports()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Listview_ReportHeads.SelectedIndex = 0;
            Frame_ReportArea.Content = new StockCountReport();
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
                    Frame_ReportArea.Content = new StockCountReport();
                }
                else
                {
                    Frame_ReportArea.Content = "Reports Coming....";
                }  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
