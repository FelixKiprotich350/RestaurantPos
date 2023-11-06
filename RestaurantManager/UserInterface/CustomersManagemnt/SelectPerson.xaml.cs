using DatabaseModels.CRM;
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

namespace RestaurantManager.UserInterface.CustomersManagemnt
{
    /// <summary>
    /// Interaction logic for SelectTicketToUpdate.xaml
    /// </summary>
    public partial class SelectPerson : Window
    {
        public string SelectedPersonNumber = null;
        public SelectPerson()
        {
            InitializeComponent();
            SelectedPersonNumber = null;
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
                var a = db.PersonalAccount.FirstOrDefault(k => k.PersonGuid == Textbox_TicketNumber.Text.ToString());
                if (a != null)
                {
                    SelectedPersonNumber = Textbox_TicketNumber.Text.ToString();
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
                List<PersonalAccount> item = new List<PersonalAccount>();
                using (var db = new PosDbContext())
                {
                    item = db.PersonalAccount.AsNoTracking().ToList();
                }
                Datagrid_PersonsList.ItemsSource = item;
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
                    if (Datagrid_PersonsList.SelectedItem == null)
                    {
                        return;
                    }
                     
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
                if (Datagrid_PersonsList.SelectedItem != null)
                {
                    var order = (PersonalAccount)Datagrid_PersonsList.SelectedItem;
                    Textbox_TicketNumber.Text = order.PersonGuid;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
