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
    /// Interaction logic for EditOrderItem.xaml
    /// </summary>
    public partial class EditTicketRemoveItem : Window
    {
        public string ReturningAction = "";
        public string ItemServiceType = "";
        public int ReturningQuantity=1;
        public EditTicketRemoveItem(string productname)
        {
            InitializeComponent();
            Textbox_ItemName.Text = productname;
        } 

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_VoidItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_Description.Text.Length <= 15)
                {
                    MessageBox.Show("The description is too short!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                ReturningAction = "Delete";
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       
    }
}
