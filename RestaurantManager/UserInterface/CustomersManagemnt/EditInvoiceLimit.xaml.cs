using DatabaseModels.CRM;
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

namespace RestaurantManager.UserInterface.CustomersManagemnt
{
    /// <summary>
    /// Interaction logic for NewCustomer.xaml
    /// </summary>
    public partial class EditInvoiceLimit : Window
    {
        readonly CustomerAccount EditingPersonID = null; 
        //for adding new person
        public EditInvoiceLimit()
        {
            InitializeComponent();
        }
        //for editing
        public EditInvoiceLimit(CustomerAccount personid)
        {
            InitializeComponent();
            EditingPersonID = personid;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EditingPersonID == null)
                {
                    Close();
                }
                Textbox_FullName.Text = EditingPersonID.FullName;
                Textbox_NewLimit.Text = EditingPersonID.InvoiceLimit.ToString();
                Textbox_CurrentLimit.Text = EditingPersonID.InvoiceLimit.ToString("N2");
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (decimal.TryParse(Textbox_NewLimit.Text.Trim(), out decimal newlimit))
                {
                    var db = new PosDbContext();
                    var p = db.CustomerAccount.FirstOrDefault(k => k.AccountGuid == EditingPersonID.AccountGuid);
                    p.InvoiceLimit = newlimit;
                    db.SaveChanges();
                    MessageBox.Show("Successfully Saved!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("The New Limit Amount entered is Invalid!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                Close();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
         
    
    }
}
