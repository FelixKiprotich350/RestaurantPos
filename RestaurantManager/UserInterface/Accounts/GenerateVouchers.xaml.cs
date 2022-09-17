using RestaurantManager.BusinessModels.Vouchers;
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

namespace RestaurantManager.UserInterface.Accounts
{
    /// <summary>
    /// Interaction logic for GenerateVouchers.xaml
    /// </summary>
    public partial class GenerateVouchers : Page
    {
        readonly Random R = new Random();
        public GenerateVouchers()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBatchVoucher();
        }
        private void LoadBatchVoucher()
        {
            try
            {
                TextBox_VoucherType.Text = GlobalVariables.PosEnums.VoucherTypes.CashVoucher.ToString();
                using (var db = new PosDbContext())
                {
                    var data = db.VouchersBatch.ToList();
                    Datagrid_Vouchers.ItemsSource = data;
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 

        private void Button_CreateVouchers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dtime = GlobalVariables.SharedVariables.CurrentDate();
                VouchersBatch v = new VouchersBatch
                {
                    BatchGuid = Guid.NewGuid().ToString(),
                    BatchNumber = R.Next(100000, 999999).ToString(),
                    VoucherType = TextBox_VoucherType.Text,
                    CreatedBy = GlobalVariables.SharedVariables.CurrentUser.UserName,
                    VoucherAmount = Convert.ToInt32(TextBox_VoucherAmount.Text),
                    VouchersCount = Convert.ToInt32(TextBox_VoucherQuantity.Text),
                    BatchDescription = TextBox_BatchDescription.Text,
                    CreationDate = GlobalVariables.SharedVariables.CurrentDate(),
                    ExpiryDate = (DateTime)DatePicker_BatchExpiryDate.SelectedDate
                };
                List<VoucherCard> vcards = new List<VoucherCard>();

                
                List<int> UniqueList = new List<int>();
                while (UniqueList.Count<v.VouchersCount)
                {
                    int y = R.Next(10000, 99999);
                    if (!UniqueList.Contains(y))
                    {
                        UniqueList.Add(y);
                    }
                }
                foreach (var a in UniqueList)
                {
                    VoucherCard vc = new VoucherCard
                    {
                        VoucherBatchNo = v.BatchNumber,
                        VoucherType = v.VoucherType,
                        VoucherAmount = v.VoucherAmount,
                        VoucherStatus = GlobalVariables.PosEnums.VoucherStatuses.Available.ToString(),
                        CreationDate = dtime,
                        ExpiryDate = v.ExpiryDate,
                        VoucherGuid = Guid.NewGuid().ToString(),
                        VoucherNumber = v.BatchNumber + "-" + a
                    };
                   
                    vcards.Add(vc);
                }
                using (var db = new PosDbContext())
                {
                    db.VoucherCard.AddRange(vcards);
                    db.VouchersBatch.Add(v);
                    db.SaveChanges();
                }
                MessageBox.Show("Successfully Saved!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadBatchVoucher();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }  
        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadBatchVoucher();
        }
    }
}
