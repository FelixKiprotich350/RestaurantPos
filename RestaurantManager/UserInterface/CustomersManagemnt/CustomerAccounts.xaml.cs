using DatabaseModels.CRM;
using DatabaseModels.Vouchers;
using RestaurantManager.ApplicationFiles;
using RestaurantManager.GlobalVariables;
using RestaurantManager.UserInterface.PointofSale;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class CustomerAccounts : Page
    {
        //readonly Random R = new Random();
        public CustomerAccounts()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadCustomers();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadCustomers()
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    var data = db.CustomerAccount.ToList();
                    Datagrid_CustomersList.ItemsSource = data;
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Textbox_SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //try
            //{
            //    TextBox t = (TextBox)sender;
            //    string filter = t.Text;
            //    if (Datagrid_CustomersList.ItemsSource == null)
            //    {
            //        return;
            //    }
            //    ICollectionView cv = CollectionViewSource.GetDefaultView(Datagrid_CustomersList.ItemsSource);
            //    if (filter == "")
            //    {
            //        cv.Filter = null;
            //    }
            //    else
            //    {
            //        cv.Filter = new Predicate<object>(Contains);
            //    }
            //    //if (filter == "")
            //    //    cv.Filter = null;
            //    //else
            //    //{
            //    //    cv.Filter = o =>
            //    //    {
            //    //        MenuProductItem p = o as MenuProductItem;
            //    //        return p.ProductName.ToLower().Contains(filter.ToLower()) || p.AvailabilityStatus.ToString().ToLower().Contains(filter.ToLower());
            //    //    };
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }
 
        private void Button_RedeemPoints_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    RedeeemSettings rs = new RedeeemSettings(Textbox_PhoneNo.Text);
            //    if (!(bool)rs.ShowDialog())
            //    {
            //        return;
            //    }
            //    DateTime dtime = GlobalVariables.SharedVariables.CurrentDate();
               
             
            //    VoucherCard vc = new VoucherCard
            //    {
            //        //VoucherBatchNo = v.BatchNumber,
            //        //VoucherType = v.VoucherType,
            //        //VoucherAmount = v.VoucherAmount,
            //        //VoucherStatus = GlobalVariables.PosEnums.VoucherStatuses.Available.ToString(),
            //        //CreationDate = dtime,
            //        //ExpiryDate = v.StartDate,
            //        //VoucherGuid = Guid.NewGuid().ToString(),
            //        //VoucherNumber = v.BatchNumber + "-" + UniqueList[0]
            //    };
            //    DatabaseModels.CRM.CustomerPointsAccount ca = new DatabaseModels.CRM.CustomerPointsAccount
            //    {
            //        AccActionGuid = Guid.NewGuid().ToString(),
            //        TransactionNo = GlobalVariables.SharedVariables.CurrentDate().ToString("yyMMddHHmmssffff"),
            //        CustomerPhoneNo = Textbox_PhoneNo.Text,
            //        Credit = rs.RedeemPoints,
            //        Debit = 0,
            //        ApprovedBy = GlobalVariables.SharedVariables.CurrentUser.UserName,
            //        ActionDate = GlobalVariables.SharedVariables.CurrentDate(),
            //        TransactionType= CustomerAccountActionType.Redeem.ToString()
            //    };
            //    using (var db = new PosDbContext())
            //    {
            //        db.CustomerPointsAccount.Add(ca);
            //        db.VoucherCard.Add(vc); 
            //        db.SaveChanges();
            //    }
            //    MessageBox.Show("Successfully Redeemed!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
            //    GetHistory(Textbox_PhoneNo.Text);
            //}
            //catch (Exception exception1)
            //{
            //    MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void Button_Select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 
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
                    //var list = db.CustomerPointsAccount.Where(x => x.CustomerPhoneNo == phoneno).ToList();
                    //foreach(var x in list)
                    //{
                    //    total += x.Debit;
                    //    credit += x.Credit;
                    //}
                    
                    //Datagrid_CustomersList.ItemsSource = list;
                } 
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_NewCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectCustomertoadd sc = new SelectCustomertoadd();
                if ((bool)sc.ShowDialog())
                {
                    PersonalAccount p = (PersonalAccount)sc.Textbox_SelectedCustomerPhone.Tag;
                    if (p != null)
                    {
                        CustomerAccount ca = new CustomerAccount()
                        {
                            AccountGuid = Guid.NewGuid().ToString(),
                            AccountStatus = PosEnums.PersonAccountStatus.Active.ToString(),
                            PersonAccNo = p.AccountNo,
                            TotalPoints = 0,
                            InvoiceLimit = 0,
                            LastUpdatedBy = SharedVariables.CurrentUser.UserName,
                            LastUpdateDate = SharedVariables.CurrentDate(),
                            RegDate = SharedVariables.CurrentDate(),
                        };

                        var db = new PosDbContext();
                        db.CustomerAccount.Add(ca);
                        db.SaveChanges();
                        MessageBox.Show("Customer Added Successfully", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
