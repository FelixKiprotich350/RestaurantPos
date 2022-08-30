using RestaurantManager.BusinessModels.GeneralSettings;
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

namespace RestaurantManager.UserInterface.GeneralSettings
{
    /// <summary>
    /// Interaction logic for ClientInfo.xaml
    /// </summary>
    public partial class ClientInfo : Page
    {
        public ClientInfo()
        {
            InitializeComponent();
        }

        private void Button_SaveClientInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TextBox_ClientTitle.Tag == null)
                {
                    return;
                }
                using (var b = new PosDbContext())
                {
                    
                        ClientInfoDetails c = b.ClientInfo.Where(a => a.ClientGuid == TextBox_ClientTitle.Tag.ToString()).First();
                    c.ClientTitle = TextBox_ClientTitle.Text;
                    c.PhysicalAddress = TextBox_PhysicalAddress.Text;
                    c.Email = TextBox_Email.Text;
                    c.Phone = TextBox_Telephone.Text; 
                    c.ClientKRAPIN = TextBox_PinNumber.Text;  
                    c.ReceiptNote1 = TextBox_ReceiptNote1.Text;  
                    c.ReceiptNote2 = TextBox_ReceiptNote2.Text;   
                    c.ThankYouNote = TextBox_ThankYouNote.Text; 
                    b.SaveChanges();
                    MessageBox.Show("Success. Updated", "Messsage Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var b = new PosDbContext())
                {
                    if (b.ClientInfo.Count() <= 0)
                    {
                        return;
                    }
                    TextBox_ClientTitle.Text = b.ClientInfo.ToList()[0].ClientTitle;
                    TextBox_ClientTitle.Tag = b.ClientInfo.ToList()[0].ClientGuid;
                    TextBox_PhysicalAddress.Text = b.ClientInfo.ToList()[0].PhysicalAddress;
                    TextBox_Email.Text = b.ClientInfo.ToList()[0].Email;
                    TextBox_Telephone.Text = b.ClientInfo.ToList()[0].Phone;
                    TextBox_PinNumber.Text = b.ClientInfo.ToList()[0].ClientKRAPIN;
                    TextBox_ReceiptNote1.Text = b.ClientInfo.ToList()[0].ReceiptNote1;
                    TextBox_ReceiptNote2.Text = b.ClientInfo.ToList()[0].ReceiptNote2;
                    TextBox_ThankYouNote.Text = b.ClientInfo.ToList()[0].ThankYouNote;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
