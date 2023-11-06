using DatabaseModels.Inventory;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantManager.UserInterface.Inventory
{
    /// <summary>
    /// Interaction logic for NewMenuProduct.xaml
    /// </summary>
    public partial class AddStockTakingNumber : Window
    {
        bool returnvalue = false;
        public AddStockTakingNumber()
        {
            InitializeComponent();
        }

        private void Button_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_Notes.Text.Trim() == "")
                {
                    MessageBox.Show("Enter the Stock Taking Number", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } if (Textbox_STTNo.Text.Trim() == "")
                {
                    MessageBox.Show("You must enter a short note.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }   
                using (var db = new PosDbContext())
                {
                    StockTakingMaster stt = new StockTakingMaster();
                    stt.ItemGuid = Guid.NewGuid().ToString();
                    stt.OpeningDate = GlobalVariables.SharedVariables.CurrentDate();
                    stt.ClosingDate = GlobalVariables.SharedVariables.CurrentDate();
                    stt.OpenedBy = GlobalVariables.SharedVariables.CurrentUser.UserName;
                    stt.ClosedBy = "N/A";
                    stt.Notes = Textbox_Notes.Text.Trim();
                    stt.STTNumber = Textbox_STTNo.Text.Trim();
                    db.StockTakingMaster.Add(stt);
                    db.SaveChanges();
                    MessageBox.Show("Success. Item Saved.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    returnvalue = true;
                }
                
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Textbox_STTNo.Text = DateTime.Now.ToString("dddMMyyyy");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = returnvalue;
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
