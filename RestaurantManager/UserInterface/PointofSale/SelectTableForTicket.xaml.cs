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

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for SelectTableForTicket.xaml
    /// </summary>
    public partial class SelectTableForTicket : Window
    {
        public string SelectedTable = "Table";
        public SelectTableForTicket()
        {
            InitializeComponent();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedTable = "";
                this.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_TableSelected.Text.Trim() != "")
                {
                    SelectedTable = Textbox_TableSelected.Text;
                    this.DialogResult = true;
                }
               
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db=new PosDbContext())
                {
                    TablesList.ItemsSource = db.TableEntity.ToList();

                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Textbox_TableSelected.Text = ((Button)sender).Content.ToString();
        }
    }
}
