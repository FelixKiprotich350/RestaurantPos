using RestaurantManager.BusinessModels.WorkPeriod;
using RestaurantManager.MailingPlugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using WinForms = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using RestaurantManager.ApplicationFiles;

namespace RestaurantManager.UserInterface.WorkPeriods
{
    /// <summary>
    /// Interaction logic for ManageWorkPeriod.xaml
    /// </summary>
    public partial class ManageWorkPeriod : Page
    {
        readonly POSMail mail = new POSMail();
        public ManageWorkPeriod()
        {
            InitializeComponent();
        }

        private  void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GroupBox_ClosePeriod.IsEnabled = false;
            GroupBox_OpenPeriod.IsEnabled = false;
            TextBox_CurrentWorkPeriodStatus.Tag = null;
            TogglePeriodTask();
            DispatcherTimer t = new DispatcherTimer();
            t.Tick += new EventHandler(UpdateWorkPeriodStatus);
            t.Interval = new TimeSpan(0, 0, 1);
            t.Start(); 
             
        }

        private void UpdateWorkPeriodStatus(object sender, EventArgs eventArgs)
        {
            TogglePeriodTask();
        }

        private void TogglePeriodTask()
        {
            try
            {
                if (!GroupBox_OpenPeriod.IsEnabled)
                {
                    Textbox_OpenningNote.Text = "";
                    Textbox_NewWorkperiodname.Text = "";
                }
                if (!GroupBox_ClosePeriod.IsEnabled)
                {
                    Textbox_PeriodName.Text = "";
                    Textbox_OpenedBy.Text = "";
                    Textbox_OpeningDate.Text = "";
                    Textbox_OpenningNote_close.Text = "";
                }



                var db = new PosDbContext();
                if (db.WorkPeriod.Where(x => x.WorkperiodStatus == GlobalVariables.PosEnums.WorkPeriodStatuses.Open.ToString()).Count() > 0)
                {
                    TextBox_CurrentWorkPeriodStatus.Text = GlobalVariables.PosEnums.WorkPeriodStatuses.Open.ToString();
                    WorkPeriod w = db.WorkPeriod.Where(x => x.WorkperiodStatus == GlobalVariables.PosEnums.WorkPeriodStatuses.Open.ToString()).First();
                    TextBox_CurrentWorkPeriodStatus.Tag = w;
                    if (!GroupBox_ClosePeriod.IsEnabled)
                    {
                        GroupBox_ClosePeriod.IsEnabled = true;
                    }
                    GroupBox_OpenPeriod.IsEnabled = false;
                    Textbox_PeriodName.Text = w.WorkperiodName;
                    Textbox_OpenedBy.Text = w.Openedby;
                    Textbox_OpeningDate.Text = w.OpeningDate.ToString();
                    Textbox_OpenningNote_close.Text = w.OpeningNote;
                    //
                }
                else
                {

                    TextBox_CurrentWorkPeriodStatus.Text = GlobalVariables.PosEnums.WorkPeriodStatuses.Closed.ToString();
                    GroupBox_ClosePeriod.IsEnabled = false;
                    if (!GroupBox_OpenPeriod.IsEnabled)
                    {
                        GroupBox_OpenPeriod.IsEnabled = true;
                    }

                    TextBox_CurrentWorkPeriodStatus.Tag = null;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                GroupBox_ClosePeriod.IsEnabled = false;
                GroupBox_OpenPeriod.IsEnabled = false;
                TextBox_CurrentWorkPeriodStatus.Tag = null;
            }
        }

        private void Button_CreateWorkPeriod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_NewWorkperiodname.Text.Trim() == "")
                {
                    MessageBox.Show("Enter the Work Period Name.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Textbox_OpenningNote.Text.Trim() == "")
                {
                    MessageBox.Show("Enter the Period Opening Note.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var db = new PosDbContext())
                {
                    if (db.WorkPeriod.Where(x => x.WorkperiodStatus == "Open").Count() > 0)
                    {
                        MessageBox.Show("You must Close the currently Open WorkPeriod to create another one!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        TogglePeriodTask();
                        return;
                    }

                }
                WorkPeriod w = new WorkPeriod
                {
                    WorkPeriodGuid = Guid.NewGuid().ToString(),
                    WorkperiodName = "WP-" + Textbox_NewWorkperiodname.Text,
                    OpeningNote = Textbox_OpenningNote.Text,
                    Openedby = GlobalVariables.SharedVariables.CurrentUser.UserName.ToString(),
                    ClosedBy = "",
                    WorkperiodStatus = "Open",
                    OpeningDate = GlobalVariables.SharedVariables.CurrentDate(),
                    ClosingDate = GlobalVariables.SharedVariables.CurrentDate(),
                    ClosingNote = "Openning"
                };

                using (var db = new PosDbContext())
                {
                    db.WorkPeriod.Add(w);
                    db.SaveChanges();
                    //var x = SendMail();
                    MessageBox.Show("Success. Item Saved.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    TogglePeriodTask();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private async void Button_ClosePeriod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime closetime = GlobalVariables.SharedVariables.CurrentDate();
                if (TextBox_CurrentWorkPeriodStatus.Tag == null)
                {
                    MessageBox.Show("Try Again!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!(TextBox_CurrentWorkPeriodStatus.Tag is WorkPeriod))
                {
                    MessageBox.Show("The Open Work Period is not Known!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                WorkPeriod o = (WorkPeriod)TextBox_CurrentWorkPeriodStatus.Tag;
                if (o == null)
                {
                    MessageBox.Show("The selected Work Period is null!!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                using (var db = new PosDbContext())
                {
                    if (db.WorkPeriod.Where(x => x.WorkperiodName == o.WorkperiodName && x.WorkperiodStatus == "Open").Count() <= 0)
                    {
                        MessageBox.Show("This Work Period is already closed!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }

                CloseWorkPeriodSummary summary = new CloseWorkPeriodSummary(o.WorkperiodName);
                if ((bool)summary.ShowDialog())
                {
                    
                    using (var db = new PosDbContext())
                    {
                        var wp = db.WorkPeriod.Where(a => a.WorkperiodName == o.WorkperiodName).First();
                        wp.WorkperiodStatus = "Closed";
                        wp.ClosingNote = "Note";
                        wp.ClosedBy = GlobalVariables.SharedVariables.CurrentUser.UserName;
                        wp.ClosingDate = closetime;
                        db.SaveChanges();
                        WPClosureMessage mess = new WPClosureMessage
                        {
                            WorkPeriodName=wp.WorkperiodName,
                            OpenedBy = wp.Openedby,
                            OpeningTime = wp.OpeningDate.ToString(),
                            VoidedTickets = summary.TextBox_VoidedTickets.Text,
                            PendingTickets = summary.TextBox_PendingTickets.Text,
                            CompletedTickets = summary.TextBox_CompletedTickets.Text,
                            Cash = summary.Textbox_CashTotal.Text,
                            Mpesa = summary.Textbox_Mpesa.Text,
                            Vouchers = summary.TextBox_Vouchers.Text,
                            Cards = summary.Textbox_Cards.Text,
                            Total = summary.Textbox_Totals.Text,
                            ClosedBy = GlobalVariables.SharedVariables.CurrentUser.UserName,
                            ClosingTime = closetime.ToString(),
                            OpeningNote = wp.OpeningNote,
                            ClosingNote ="Closed",
                            
                        };
                        string message = GetDoc(mess);
                        var xm = await mail.SendWPClosureMail( message, "Work Period Closure", true);
                        if (!xm)
                        {
                            MessageBox.Show("Failed to send the Mail. Consult the Administrator Immediately!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        MessageBox.Show("Successfully closed the Work Period!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetDoc(WPClosureMessage m)
        {
            try
            {
                if(m is null)
                {
                    return null;
                }
                string htmlstring = File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"PeriodClosure.html"));
                htmlstring = htmlstring.Replace("~Pnamevalue~", m.WorkPeriodName);
                htmlstring = htmlstring.Replace("~Item1~", "Open Date");
                htmlstring = htmlstring.Replace("~Item2~", "Opened By");
                htmlstring = htmlstring.Replace("~Item3~", "Closing Date");
                htmlstring = htmlstring.Replace("~Item4~", "Closed By");
                htmlstring = htmlstring.Replace("~Item5~", "Pending Tickets");
                htmlstring = htmlstring.Replace("~Item6~", "Voided Tickets");
                htmlstring = htmlstring.Replace("~Item7~", "Completed Tickets");
                htmlstring = htmlstring.Replace("~Item8~", "Cash ");
                htmlstring = htmlstring.Replace("~Item9~", "Mpesa ");
                htmlstring = htmlstring.Replace("~Item10~", "Cards ");
                htmlstring = htmlstring.Replace("~Item11~", "Vouchers ");
                htmlstring = htmlstring.Replace("~Item12~", "Summary Total");
                htmlstring = htmlstring.Replace("~Item13~", "Opening Note");
                htmlstring = htmlstring.Replace("~Item14~", "Closing Note");
                htmlstring = htmlstring.Replace("~Value1~", m.OpeningTime);
                htmlstring = htmlstring.Replace("~Value2~", m.OpenedBy);
                htmlstring = htmlstring.Replace("~Value3~", m.ClosingTime);
                htmlstring = htmlstring.Replace("~Value4~", m.ClosedBy);
                htmlstring = htmlstring.Replace("~Value5~", m.PendingTickets);
                htmlstring = htmlstring.Replace("~Value6~", m.VoidedTickets);
                htmlstring = htmlstring.Replace("~Value7~", m.CompletedTickets);
                htmlstring = htmlstring.Replace("~Value8~", m.Cash);
                htmlstring = htmlstring.Replace("~Value9~", m.Mpesa);
                htmlstring = htmlstring.Replace("~Value10~", m.Cards);
                htmlstring = htmlstring.Replace("~Value11~", m.Vouchers);
                htmlstring = htmlstring.Replace("~Value12~", m.Total);
                htmlstring = htmlstring.Replace("~Value13~", m.OpeningNote);
                htmlstring = htmlstring.Replace("~Value14~", m.ClosingNote);
                return htmlstring;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
