﻿using DatabaseModels.Payments;
using DatabaseModels.WorkPeriod;
using RestaurantManager.ApplicationFiles; 
using RestaurantManager.GlobalVariables;
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

namespace RestaurantManager.UserInterface.WorkPeriods
{
    /// <summary>
    /// Interaction logic for CloseWorkPeriodSummary.xaml
    /// </summary>
    public partial class CloseWorkPeriodSummary : Window
    {
        readonly public string WPname = "";
        public CloseWorkPeriodSummary(string periodname)
        {
            InitializeComponent();
            WPname = periodname;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                WorkPeriod period = null;
                using (var db = new PosDbContext())
                {
                    period = db.WorkPeriod.Where(x => x.WorkperiodStatus == "Open" && x.WorkperiodName == WPname).FirstOrDefault();
                    if (period is null)
                    {
                        MessageBox.Show("The Work Period does not Exist!.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    if (period.WorkperiodStatus != "Open")
                    {
                        MessageBox.Show("The work period Is Closed!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                }
                Textbox_Workperiodname.Text = period.WorkperiodName;
                Textbox_Openingdate.Text = period.OpeningDate.ToString();
                Textbox_OpenedBy.Text = period.Openedby;
                //find tickets summary  
                using (var db = new PosDbContext())
                {
                    var x = db.OrderMaster.Where(y => y.Workperiod == period.WorkperiodName).ToList();
                    TextBox_PendingTickets.Text = x.Where(k => k.OrderStatus == PosEnums.OrderTicketStatuses.Pending.ToString()).Count().ToString();
                    TextBox_VoidedTickets.Text = x.Where(k => k.OrderStatus == PosEnums.OrderTicketStatuses.Voided.ToString()).Count().ToString();
                    TextBox_CompletedTickets.Text = x.Where(k => k.OrderStatus == PosEnums.OrderTicketStatuses.Completed.ToString()).Count().ToString();
                }
                //find tickets payments summary
                List<TicketPaymentMaster> tlist = new List<TicketPaymentMaster>();
                using (var db = new PosDbContext())
                {
                    tlist = db.TicketPaymentMaster.Where(k => k.WorkPeriod == period.WorkperiodName).ToList();
                    GetAccountsTotal(tlist);
                }
                using (var db = new PosDbContext())
                {
                    decimal pendingtotal = 0;
                    var wp = SharedVariables.CurrentOpenWorkPeriod().WorkperiodName;
                    var list = db.OrderMaster.AsNoTracking().Where(k => k.OrderStatus==PosEnums.OrderTicketStatuses.Pending.ToString()& k.Workperiod==wp).ToList();
                   foreach (var x in list)
                    {
                        pendingtotal += db.OrderItem.AsNoTracking().Where(i => i.OrderID == x.OrderNo).Sum(k => k.Total);
                    }
                    TextBox_PendingTotal.Text = pendingtotal.ToString("N2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

        }
        private void GetAccountsTotal(List<TicketPaymentMaster> tlist)
        {
            try
            {
                decimal mpesa = 0;
                decimal cash = 0;
                decimal cards = 0;
                decimal voucher = 0;
                decimal invoice = 0; 
                decimal unknown = 0;

                var db = new PosDbContext();
                if (tlist.Count > 0)
                {
                    var innerGroupJoinQuery = from m in tlist
                                              join t in db.TicketPaymentItem on m.TicketNo equals t.ParentSourceRef
                                              select new { m, t };


                    foreach (var x in innerGroupJoinQuery)
                    {

                        if (x.t.Method == PosEnums.TicketPaymentMethods.Cash.ToString())
                        {
                            cash += x.t.AmountUsed;
                        }
                        else if (x.t.Method == PosEnums.TicketPaymentMethods.Mpesa.ToString())
                        {
                            mpesa += x.t.AmountUsed;
                        }
                        else if (x.t.Method.ToLower().Contains(PosEnums.TicketPaymentMethods.Card.ToString().ToLower()))
                        {
                            cards += x.t.AmountUsed;
                        }
                        else if (x.t.Method == PosEnums.TicketPaymentMethods.Voucher.ToString())
                        {
                            voucher += x.t.AmountUsed;
                        }
                        else if (x.t.Method == PosEnums.TicketPaymentMethods.Invoice.ToString())
                        {
                            invoice += x.t.AmountUsed;
                        }
                        else
                        {
                            unknown += x.t.AmountUsed;
                        }
                    } 
                }
               

                Textbox_CashTotal.Text = cash.ToString();
                Textbox_Mpesa.Text = mpesa.ToString();
                TextBox_Vouchers.Text = voucher.ToString();
                Textbox_Cards.Text = cards.ToString();
                Textbox_Totals.Text = (cash + mpesa + cards ).ToString();
                if (unknown > 0)
                {
                    MessageBox.Show("The following amount cannot be accounted for!\n" + unknown.ToString("N2"), "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Button_Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                using (var db = new PosDbContext())
                {
                    if (db.OrderMaster.Where(x => x.Workperiod == Textbox_Workperiodname.Text && x.OrderStatus == PosEnums.OrderTicketStatuses.Pending.ToString()).Count() > 0)
                    {
                        MessageBox.Show("The work period contains Pending Tickets. This will be closed anyway!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                       // return;
                    }
                }
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }

        }

       
    }
}
