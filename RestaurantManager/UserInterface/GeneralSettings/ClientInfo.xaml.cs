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
                if(!decimal.TryParse(TextBox_TaxPercentage.Text,out decimal tax))
                {
                    MessageBox.Show("Enter correct Percentage Value", "Messsage Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                using (var b = new PosDbContext())
                {

                    ClientInfoDetails c = b.ClientInfo.FirstOrDefault();
                    if(c is null)
                    {
                        ClientInfoDetails d = new ClientInfoDetails();
                        d.ClientGuid = Guid.NewGuid().ToString(); 
                        d.ClientTitle = TextBox_ClientTitle.Text;
                        d.PhysicalAddress = TextBox_PhysicalAddress.Text;
                        d.Email = TextBox_Email.Text;
                        d.Phone = TextBox_Telephone.Text;
                        d.ClientKRAPIN = TextBox_PinNumber.Text;
                        d.TaxPercentage = tax;
                        d.ReceiptNote1 = TextBox_ReceiptNote1.Text;
                        d.ReceiptNote2 = TextBox_ReceiptNote2.Text;
                        d.ReceiptNote3 = TextBox_ThankYouNote.Text;
                        b.ClientInfo.Add(d);
                        b.SaveChanges(); 
                        MessageBox.Show("Success.", "Messsage Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        c.ClientTitle = TextBox_ClientTitle.Text;
                        c.PhysicalAddress = TextBox_PhysicalAddress.Text;
                        c.Email = TextBox_Email.Text;
                        c.Phone = TextBox_Telephone.Text;
                        c.ClientKRAPIN = TextBox_PinNumber.Text;
                        c.TaxPercentage = tax;
                        c.ReceiptNote1 = TextBox_ReceiptNote1.Text;
                        c.ReceiptNote2 = TextBox_ReceiptNote2.Text;
                        c.ReceiptNote3 = TextBox_ThankYouNote.Text;
                        b.SaveChanges();
                        MessageBox.Show("Success.", "Messsage Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    } 
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
                        MessageBox.Show("You have not created any profile!","Message Box",MessageBoxButton.OK,MessageBoxImage.Warning);
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
                    TextBox_ThankYouNote.Text = b.ClientInfo.ToList()[0].ReceiptNote3;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
