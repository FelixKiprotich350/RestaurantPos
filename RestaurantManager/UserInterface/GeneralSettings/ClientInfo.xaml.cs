using RestaurantManager.ApplicationFiles;
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
                        ClientInfoDetails d = new ClientInfoDetails
                        {
                            ClientGuid = Guid.NewGuid().ToString(),
                            ClientTitle = TextBox_ClientTitle.Text,
                            PhysicalAddress = TextBox_PhysicalAddress.Text,
                            Email = TextBox_Email.Text,
                            Phone = TextBox_Telephone.Text,
                            ClientKRAPIN = TextBox_PinNumber.Text,
                            TaxPercentage = tax,
                            ReceiptNote1 = TextBox_ReceiptNote1.Text,
                            ReceiptNote2 = TextBox_ReceiptNote2.Text,
                            ReceiptNote3 = TextBox_ReceiptNote3.Text,
                            AcceptedCards = TextBox_AcceptedCards.Text.Trim()
                        };
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
                        c.ReceiptNote3 = TextBox_ReceiptNote3.Text;
                        c.AcceptedCards = TextBox_AcceptedCards.Text.Trim();
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
                    TextBox_ReceiptNote3.Text = b.ClientInfo.ToList()[0].ReceiptNote3;
                    TextBox_TaxPercentage.Text = b.ClientInfo.ToList()[0].TaxPercentage.ToString();
                    TextBox_AcceptedCards.Text = b.ClientInfo.ToList()[0].AcceptedCards;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
