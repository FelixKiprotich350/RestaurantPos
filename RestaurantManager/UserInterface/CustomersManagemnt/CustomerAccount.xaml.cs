using RestaurantManager.ApplicationFiles;
using RestaurantManager.BusinessModels.CustomersManagement;
using RestaurantManager.BusinessModels.Vouchers;
using RestaurantManager.UserInterface.PointofSale;
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
using static RestaurantManager.GlobalVariables.PosEnums;

namespace RestaurantManager.UserInterface.CustomersManagemnt
{
    /// <summary>
    /// Interaction logic for CustomerAccount.xaml
    /// </summary>
    public partial class CustomerAccount : Page
    {
        //readonly Random R = new Random();
        public CustomerAccount()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Grid_CustomerData.Visibility = Visibility.Hidden;
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_RedeemPoints_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RedeeemSettings rs = new RedeeemSettings(Textbox_PhoneNo.Text);
                if (!(bool)rs.ShowDialog())
                {
                    return;
                }
                DateTime dtime = GlobalVariables.SharedVariables.CurrentDate();
               
             
                VoucherCard vc = new VoucherCard
                {
                    //VoucherBatchNo = v.BatchNumber,
                    //VoucherType = v.VoucherType,
                    //VoucherAmount = v.VoucherAmount,
                    //VoucherStatus = GlobalVariables.PosEnums.VoucherStatuses.Available.ToString(),
                    //CreationDate = dtime,
                    //ExpiryDate = v.StartDate,
                    //VoucherGuid = Guid.NewGuid().ToString(),
                    //VoucherNumber = v.BatchNumber + "-" + UniqueList[0]
                };
                BusinessModels.CustomersManagement.CustomerAccount ca = new BusinessModels.CustomersManagement.CustomerAccount
                {
                    AccActionGuid = Guid.NewGuid().ToString(),
                    TransactionNo = GlobalVariables.SharedVariables.CurrentDate().ToString("yyMMddHHmmssffff"),
                    CustomerPhoneNo = Textbox_PhoneNo.Text,
                    Credit = rs.RedeemPoints,
                    Debit = 0,
                    ApprovedBy = GlobalVariables.SharedVariables.CurrentUser.UserName,
                    ActionDate = GlobalVariables.SharedVariables.CurrentDate(),
                    TransactionType= CustomerAccountActionType.Redeem.ToString()
                };
                using (var db = new PosDbContext())
                {
                    db.CustomerPointsAccount.Add(ca);
                    db.VoucherCard.Add(vc); 
                    db.SaveChanges();
                }
                MessageBox.Show("Successfully Redeemed!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                GetHistory(Textbox_PhoneNo.Text);
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectCustomerName s = new SelectCustomerName();
                if ((bool)s.ShowDialog())
                {
                    Grid_CustomerData.Visibility = Visibility.Visible;
                    Customer x = s.SelectedCustomer;
                    Textbox_FullName.Text = x.CustomerName;
                    Textbox_PhoneNo.Text = x.PhoneNumber;
                    Textbox_Gender.Text = x.Gender;
                    Textbox_BirthDate.Text = x.BirthDate.ToShortDateString();
                    Textbox_RegistrationDate.Text = x.RegistrationDate.ToShortDateString();
                    GetHistory(x.PhoneNumber);
                }
                else
                {
                    Grid_CustomerData.Visibility = Visibility.Hidden; ;
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void GetHistory(string phoneno)
        {
            try
            {
                int total = 0;
                int credit = 0;
                using (var db = new PosDbContext())
                {
                    var list = db.CustomerPointsAccount.Where(x => x.CustomerPhoneNo == phoneno).ToList();
                    foreach(var x in list)
                    {
                        total += x.Debit;
                        credit += x.Credit;
                    }
                    Textbox_LoyaltyPoints.Text = total.ToString();
                    Textbox_BalanceLoyaltyPoints.Text = (total-credit).ToString();
                    Datagrid_CustomersList.ItemsSource = list;
                } 
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetForm()
        {
            Textbox_FullName.Text = "";
            Textbox_PhoneNo.Text = "";
            Textbox_Gender.Text = "";
            Textbox_BirthDate.Text = "";
            Textbox_RegistrationDate.Text = "";
            Textbox_BalanceLoyaltyPoints.Text = "";
            Textbox_LoyaltyPoints.Text = "";
            Datagrid_CustomersList.ItemsSource = null;
            Datagrid_CustomersList.Items.Clear();
            Grid_CustomerData.Visibility = Visibility.Hidden;
        }
        
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }
    }
}
