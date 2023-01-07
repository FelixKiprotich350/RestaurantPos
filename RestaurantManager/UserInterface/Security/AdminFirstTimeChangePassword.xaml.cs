using RestaurantManager.ApplicationFiles;
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

namespace RestaurantManager.UserInterface.Security
{
    /// <summary>
    /// Interaction logic for AdminFirstTimeChangePassword.xaml
    /// </summary>
    public partial class AdminFirstTimeChangePassword : Window
    {
        public AdminFirstTimeChangePassword()
        {
            InitializeComponent();
        }

        private void Button_Change_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var b = new PosDbContext())
                {
                    //var a = b.PosUser.Where(c => c.UserServing == GlobalVariables.SharedMethodsVariables.CurrentUser.UserName && c.OrderStatus == "Pending").ToList();
                    //Datagrid_Tickets.ItemsSource = a;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
