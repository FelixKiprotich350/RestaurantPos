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
    /// Interaction logic for CreateWorkPeriod.xaml
    /// </summary>
    public partial class CreateWorkPeriod : Window
    {
        public CreateWorkPeriod()
        {
            InitializeComponent();
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_PeriodName.Text.Trim() == "")
                {
                    MessageBox.Show("Enter the Work Period Name.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Textbox_Description.Text.Trim() == "")
                {
                    MessageBox.Show("Enter the Description.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
