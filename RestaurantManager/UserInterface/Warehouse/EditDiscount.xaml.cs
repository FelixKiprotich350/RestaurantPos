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

namespace RestaurantManager.UserInterface.Warehouse
{
    /// <summary>
    /// Interaction logic for EditDiscount.xaml
    /// </summary>
    public partial class EditDiscount : Window
    {
        public EditDiscount()
        {
            InitializeComponent();
        }


        protected override void OnSourceInitialized(EventArgs e)
        {
            WindowIconRemover.RemoveIcon(this);
        }

        private void OnSourceInitialized(object sender, EventArgs e)
        {

        }
    }
}
