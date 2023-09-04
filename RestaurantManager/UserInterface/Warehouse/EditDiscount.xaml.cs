using DatabaseModels.Warehouse;
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

namespace RestaurantManager.UserInterface.Warehouse
{
    /// <summary>
    /// Interaction logic for EditDiscount.xaml
    /// </summary>
    public partial class EditDiscount : Window
    {
        public DiscountItem Item = null;
        public EditDiscount()
        {
            InitializeComponent();
        }


        protected override void OnSourceInitialized(EventArgs e)
        {
            WindowIconRemover.RemoveIcon(this);
        }
         

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void Btn_Change_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (Combobox_DiscStatus.SelectedItem != null)
                {
                    if (Item != null)
                    {
                        using (var db=new PosDbContext())
                        {
                            db.DiscountItem.First(k => k.ProductGuid == Item.ProductGuid).DiscStatus = Combobox_DiscStatus.Text;
                            db.SaveChanges();
                            MessageBox.Show("Updated Successfully!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("The product is not known!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Select the Status", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }
        }
    }
}
