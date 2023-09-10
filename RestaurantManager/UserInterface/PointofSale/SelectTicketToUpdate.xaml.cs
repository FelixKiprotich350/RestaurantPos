using DatabaseModels.OrderTicket;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for SelectTicketToUpdate.xaml
    /// </summary>
    public partial class SelectTicketToUpdate : Window
    {
        public string SelectedTicketNumber = null;
        public SelectTicketToUpdate()
        {
            InitializeComponent();
            SelectedTicketNumber = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshMenuProducts();
        }

        private void Button_Continue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = new PosDbContext();
                var a = db.OrderMaster.FirstOrDefault(k => k.OrderNo == Textbox_TicketNumber.Text.ToString());
                if (a != null)
                {
                    SelectedTicketNumber = Textbox_TicketNumber.Text.ToString();
                    DialogResult = true;
                }
                else
                {
                    this.DialogResult = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }

        }

        private void RefreshMenuProducts()
        {
            try
            {
                List<OrderMaster> item = new List<OrderMaster>();
                using (var db = new PosDbContext())
                {
                    item = db.OrderMaster.AsNoTracking().Where(k=>k.OrderStatus==GlobalVariables.PosEnums.OrderTicketStatuses.Pending.ToString()).ToList();
                }
                Datagrid_TicketsList.ItemsSource = item;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Datagrid_TicketsList_MouseUp(object sender, MouseButtonEventArgs e)
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
                    if (cell.Column.DisplayIndex != 1)
                    {
                        return;
                    }
                    if (Datagrid_TicketsList.SelectedItem == null)
                    {
                        return;
                    }
                    //OrderMaster om = Datagrid_TicketsList.SelectedItem as OrderMaster;
                    //TrackTicketMaster t = new TrackTicketMaster(om);
                    //t.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Datagrid_TicketsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Datagrid_TicketsList.SelectedItem != null)
                {
                    var order = (OrderMaster)Datagrid_TicketsList.SelectedItem;
                    Textbox_TicketNumber.Text = order.OrderNo;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
