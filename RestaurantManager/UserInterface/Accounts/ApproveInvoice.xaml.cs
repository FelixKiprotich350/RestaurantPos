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
    public partial class ApproveInvoice : Window
    { 
        /// <summary>
        /// approved=1
        /// cancelled =0
        /// </summary>
        public int IsApproved = -1; 
        public ApproveInvoice()
        {
            InitializeComponent();
        }
         

        private void Buton_Add_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            IsApproved = 1;
            Close();
        }

        private void Buton_Subtract_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            IsApproved = 0;
            Close();
        }
 
  
    }
}
