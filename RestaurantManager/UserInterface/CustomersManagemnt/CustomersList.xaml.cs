using System; 
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
using System.Windows.Controls.Primitives;
using RestaurantManager.ApplicationFiles;

namespace RestaurantManager.UserInterface.CustomersManagemnt
{
    /// <summary>
    /// Interaction logic for CustomersList.xaml
    /// </summary>
    public partial class CustomersList : Page
    {
        public CustomersList()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCustomers();
        }
        private void LoadCustomers()
        {
            try
            { 
                using (var db = new PosDbContext())
                {
                    var data = db.Customer.ToList();
                    Datagrid_CustomersList.ItemsSource = data;
                } 
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_NewCustomer_Click(object sender, RoutedEventArgs e)
        {
            NewCustomer cust = new NewCustomer();
            cust.ShowDialog();
                    }

        private void Datagrid_CustomersList_MouseUp(object sender, MouseButtonEventArgs e)
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
                    if (Datagrid_CustomersList.CurrentCell.Column.Header.ToString() == "BirthDate")
                    {
                       // MessageBox.Show("Birthdate");
                    }
                    CustomerDetails cd = new CustomerDetails();
                    cd.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
