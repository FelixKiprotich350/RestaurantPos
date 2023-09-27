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

namespace RestaurantManager.UserInterface.Inventory
{
    /// <summary>
    /// Interaction logic for AddnewCategory.xaml
    /// </summary>
    public partial class AddnewCategory : Window
    {
        public AddnewCategory()
        {
            InitializeComponent();
        }

        private void Button_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_Categoryname.Text.Trim() == "")
                {
                    MessageBox.Show("Enter the name of the Category!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Textbox_Categoryname.Text.Length<2)
                {
                    MessageBox.Show("The name is too short!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Combobox_department.SelectedItem==null)
                {
                    MessageBox.Show("Select the department!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Textbox_Categoryname.Focus();
        }
    }
}
