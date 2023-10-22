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
using DatabaseModels.CRM;

namespace RestaurantManager.UserInterface.CustomersManagemnt
{
    /// <summary>
    /// Interaction logic for CustomersList.xaml
    /// </summary>
    public partial class PersonsList : Page
    {
        public PersonsList()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPersons();
        }
        private void LoadPersons()
        {
            try
            { 
                using (var db = new PosDbContext())
                {
                    Datagrid_CustomersList.ItemsSource = null;
                    var data = db.PersonalAccount.AsNoTracking().OrderBy(k=>k.AccountNo).ToList();
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
            NewPersonalAccount cust = new NewPersonalAccount();
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
                    if (Datagrid_CustomersList.CurrentCell.Column.DisplayIndex==0)
                    {
                        PersonalAccount p = (PersonalAccount)Datagrid_CustomersList.SelectedItem;
                        NewPersonalAccount NP = new NewPersonalAccount(p.AccountNo);

                        NP.ShowDialog();
                        LoadPersons();
                    }

                    

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Refreshpersons_Click(object sender, RoutedEventArgs e)
        {
            LoadPersons();
        }
    }
}
