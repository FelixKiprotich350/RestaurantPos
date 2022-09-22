using RestaurantManager.BusinessModels.CustomersManagement;
using RestaurantManager.BusinessModels.OrderTicket;
using RestaurantManager.BusinessModels.Payments;
using RestaurantManager.BusinessModels.WorkPeriod;
using RestaurantManager.GlobalVariables;
using RestaurantManager.UserInterface.TicketPayments;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for CheckoutTicket.xaml
    /// </summary>
    public partial class CheckoutTicket : Page
    {
        public CheckoutTicket()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshTicketList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_RefreshTicketsList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshTicketList();
                MessageBox.Show("Done. Refresh Complete", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshTicketList()
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    LisTview_TicketsList.ItemsSource = db.OrderMaster.Where(p => p.OrderStatus == "Pending").ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_CheckoutTicket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var db1 = new PosDbContext();
                DateTime Tdate = GlobalVariables.SharedVariables.CurrentDate();
                WorkPeriod wp = db1.WorkPeriod.Where(h => h.WorkperiodStatus == PosEnums.WorkPeriodStatuses.Open.ToString()).FirstOrDefault();
                OrderMaster om = db1.OrderMaster.Where(h => h.OrderNo == TextBlock_TicketNo.Text).FirstOrDefault();
                if (wp==null)
                {
                    MessageBox.Show("The work Period is Closed!", "Message Box", MessageBoxButton.OK);
                    return;
                }  if (om==null)
                {
                    MessageBox.Show("The Ticket does not Exist. Select a ticket to Check it out!", "Message Box", MessageBoxButton.OK);
                    return;
                }  
                int total = 0;
                var a = Datagrid_TicketItems.Items.Cast<OrderItem>().ToList();
                foreach (OrderItem x in a)
                {
                    total += (int)x.Price * x.Quantity;
                } 

                //get payments
                List<PaymentMethod> pm = new List<PaymentMethod>();
                PaymentsUI T = new PaymentsUI(total, pm);
                if (T.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    pm = T.Payments;
                }
                if (pm.Count <= 0)
                {
                    MessageBox.Show("No payments Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);

                    return;
                }
                TicketPaymentMaster tpm = new TicketPaymentMaster
                {
                    PaymentMasterGuid = Guid.NewGuid().ToString(),
                    TicketNo = TextBlock_TicketNo.Text,
                    PosUser =  SharedVariables.CurrentUser.UserName.ToString(),
                    TotalAmountPaid = pm.Sum(b => b.Amount),
                    TotalAmountCharged = total,
                    TicketBalanceReturned = T.BalanceRetrned,
                    PaymentDate = Tdate,
                    WorkPeriod=wp.WorkperiodName
                };
                List<TicketPaymentItem> tpi = new List<TicketPaymentItem>();
                foreach (PaymentMethod p in pm)
                {
                    TicketPaymentItem t = new TicketPaymentItem
                    {
                        PaymentGuid = Guid.NewGuid().ToString(),
                        ParentOrderNo = TextBlock_TicketNo.Text,
                        ParentPaymasterGuid = tpm.PaymentMasterGuid,
                        PrimaryRefference = p.PrimaryRefference,
                        Method = p.PaymentMethodName,
                        AmountPaid = p.Amount,
                        PaymentDate = Tdate,
                        ReceivingUsername = GlobalVariables.SharedVariables.CurrentUser.UserName,
                        SecondaryRefference = p.SecondaryRefference
                    };
                    tpi.Add(t);
                     
                }

                //customer service
                Customer cust = GetCustomer(om.CustomerRefference);
                CustomerAccount ca = null;
                if (cust != null)
                {
                    ca = new CustomerAccount
                    {
                        AccActionGuid = Guid.NewGuid().ToString(),
                        TransactionNo = SharedVariables.CurrentDate().ToString("yyMMddHHmmssffff"),
                        CustomerPhoneNo = cust.PhoneNumber,
                        ApprovedBy = SharedVariables.CurrentUser.UserName,
                        ActionDate = SharedVariables.CurrentDate(),
                        Credit = 0,
                        TransactionType = PosEnums.CustomerAccountActionType.PointsAward.ToString()
                    };
                    if (GetLoyaltyPoints(total) > 0)
                    {
                        ca.Debit = GetLoyaltyPoints(total);
                    }
                    else
                    {
                        
                        ca = null;
                    }
                }

                //saving data
                using (var db=new PosDbContext())
                {
                    using (DbContextTransaction tr = db.Database.BeginTransaction())
                    {
                        foreach(var item in tpi)
                        {
                            if(item.Method == GlobalVariables.PosEnums.TicketPaymentMethods.Voucher.ToString())
                            {
                                db.VoucherCard.Where(i => i.VoucherNumber == item.PrimaryRefference).First().VoucherStatus = GlobalVariables.PosEnums.VoucherStatuses.Redeemed.ToString();
                            }
                        }
                        db.SaveChanges();
                        var result = db.OrderMaster.FirstOrDefault(b => b.OrderNo == tpm.TicketNo);
                        if (result != null)
                        {
                            result.OrderStatus = GlobalVariables.PosEnums.OrderTicketStatuses.Completed.ToString();
                            result.PaymentDate = GlobalVariables.SharedVariables.CurrentDate();
                            int x = db.SaveChanges();
                            if (x != 1)
                            {
                                tr.Rollback();
                            }
                        }

                        db.TicketPaymentItem.AddRange(tpi);
                        db.SaveChanges();
                        db.TicketPaymentMaster.Add(tpm);
                        if (ca != null)
                        {
                            db.CustomerPointsAccount.Add(ca);
                        }
                        db.SaveChanges();
                        tr.Commit();
                        MessageBox.Show("Successfuly Saved", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        Button_Discard_Click(new object(), new RoutedEventArgs());
                        RefreshTicketList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private Customer GetCustomer(string custno)
        {
            try
            {
                if (custno == null)
                {
                    return null;
                }
                var db = new PosDbContext();
                Customer cust = db.Customer.Where(x => x.PhoneNumber == custno).FirstOrDefault();
                if (cust != null)
                {
                   return cust;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public int GetLoyaltyPoints(int PurchaseAmount)
        {
            int points;
            try
            {
                //using (var db=new PosDbContext())
                //{

                //}
                points = PurchaseAmount / 100;
            }
            catch(Exception ex)
            {
                points = -1;
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return points;
        }

        private void LisTview_TicketsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListView Lv = sender as ListView;
                if (Lv.SelectedItem == null)
                {
                    return;
                }
                OrderMaster om = (OrderMaster)Lv.SelectedItem;
                LoadTicketDetails(om);
                Lv.SelectedItem = null;
                Button_CheckoutTicket.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTicketDetails(OrderMaster order)
        {
            try
            {
                //get ticket items
                using (var db = new PosDbContext())
                {
                    var a = db.OrderItem.Where(o => o.OrderID == order.OrderNo).ToList();
                    Datagrid_TicketItems.ItemsSource = a;
                }
                //find total
                decimal total = 0;
                var b = Datagrid_TicketItems.Items.Cast<OrderItem>().ToList();
                foreach (OrderItem x in b)
                {
                    total += x.Price * x.Quantity;
                }
                TextBox_TotalAmount.Text = total.ToString(); 
                TextBlock_TicketNo.Text = order.OrderNo;
                TextBlock_TicketDate.Text = order.OrderDate.ToString();
                TextBlock_ItemsCount.Text = b.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_Discard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox_TotalAmount.Text = "0.00";
                TextBlock_TicketNo.Text = "";
                TextBlock_TicketDate.Text = "";
                TextBlock_ItemsCount.Text = "0";
                Datagrid_TicketItems.ItemsSource = null;
                Button_CheckoutTicket.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
    }
}
