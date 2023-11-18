using DatabaseModels.CRM; 
using DatabaseModels.Inventory;
using DatabaseModels.Payroll;
using DatabaseModels.Vouchers;
using RestaurantManager.ActivityLogs;
using RestaurantManager.ApplicationFiles;
using RestaurantManager.GlobalVariables;
using RestaurantManager.UserInterface.CustomersManagemnt; 
using RestaurantManager.UserInterface.PointofSale;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static RestaurantManager.GlobalVariables.PosEnums;

namespace RestaurantManager.UserInterface.HR
{
    /// <summary>
    /// Interaction logic for CustomerAccount.xaml
    /// </summary>
    public partial class Employee : Page
    {
        //readonly Random R = new Random();
        public Employee()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadEmployees();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadEmployees()
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    var data = db.EmployeeAccount.AsNoTracking().ToList();
                    foreach (var x in data)
                    {
                        var p = db.PersonalAccount.AsNoTracking().FirstOrDefault(k => k.AccountNo == x.EmployeeNo);
                        x.OtherNames = p.FullName;
                        x.Gender = p.Gender;
                        x.PhoneNumber = p.PhoneNumber;
                        x.EmployeeNo = p.AccountNo;
                    }
                    Datagrid_EmployeeList.ItemsSource = data;
                }
                ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Viewed employee List", "On Date=" + SharedVariables.CurrentDate().ToString());

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


        private void Button_NewEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectPerson sc = new SelectPerson();
                if ((bool)sc.ShowDialog())
                {
                    var db = new PosDbContext();
                    PersonalAccount p = db.PersonalAccount.AsNoTracking().FirstOrDefault(k => k.PersonGuid == sc.SelectedPersonNumber);
                    if (p != null)
                    {
                        EmployeeAccount ca = new EmployeeAccount()
                        {
                            //AccountGuid = Guid.NewGuid().ToString(),
                            //AccountStatus = PosEnums.PersonAccountStatus.Active.ToString(),
                            //PersonAccNo = p.AccountNo,
                            //MonthlySalary = 0,
                            //InvoiceLimit = 0,
                            //LastUpdatedBy = SharedVariables.CurrentUser.UserName,
                            //LastUpdateDate = SharedVariables.CurrentDate(),
                            //RegDate = SharedVariables.CurrentDate(),
                        };

                        db.EmployeeAccount.Add(ca);
                        db.SaveChanges();
                        MessageBox.Show("Employee Added Successfully", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Added new employee", "Employee No=" + ca.EmployeeNo+", ID="+ca.NationalID);
                        LoadEmployees();
                    }
                }

            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadEmployees();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                // iteratively traverse the visual tree
                while ((dep != null) & !(dep is DataGridCell) & !(dep is DataGridColumnHeader))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }
                if (dep == null)
                {
                    return;
                }
                if (dep is DataGridCell cell)
                {
                    if (cell.Column.DisplayIndex == 4)
                    {
                        if (Datagrid_EmployeeList.SelectedItem == null)
                        {
                            return;
                        }
                        EmployeeAccount m = (EmployeeAccount)Datagrid_EmployeeList.SelectedItem;
                        EmployeeProfile ed = new EmployeeProfile(m);
                        ed.ShowDialog();
                        ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Viewed Employee Profile", "Employee No="+m.EmployeeNo );

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
