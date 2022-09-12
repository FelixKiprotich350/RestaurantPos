using RestaurantManager.BusinessModels.WorkPeriod;
using RestaurantManager.MailingPlugin;
using System;
using System.Collections.Generic;
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

namespace RestaurantManager.UserInterface.WorkPeriods
{
    /// <summary>
    /// Interaction logic for ViewWorkPeriod.xaml
    /// </summary>
    public partial class ViewWorkPeriod : Page
    {
        readonly POSMail mail = new POSMail();
        public ViewWorkPeriod()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshWorkPeriods();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshWorkPeriods()
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    Datagrid_Workperiods.ItemsSource = db.WorkPeriod.ToList();
                }
                TextBox_TotalCount.Text = Datagrid_Workperiods.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_NewWorkPeriod_Click(object sender, RoutedEventArgs e)
        { 
            try
            {
                using (var db = new PosDbContext())
                {
                    if (db.WorkPeriod.Where(x => x.WorkperiodStatus == "Open").Count() > 0)
                    {
                        MessageBox.Show("You must Close the currently Open WorkPeriod to create another one!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        RefreshWorkPeriods();
                        return;
                    }

                }
                CreateWorkPeriod create = new CreateWorkPeriod(); 
                if (create.ShowDialog() == false)
                {
                    return;
                }
                WorkPeriod w = new WorkPeriod
                {
                    WorkPeriodGuid = Guid.NewGuid().ToString(),
                    WorkperiodName = create.Textbox_PeriodName.Text,
                    WorkperiodDescription = create.Textbox_Description.Text,
                    Openedby = ErpShared.CurrentUser.UserPIN.ToString(),
                    ClosedBy = "",
                    WorkperiodStatus = "Open",
                    OpeningDate = ErpShared.CurrentDate(),
                    ClosingDate = ErpShared.CurrentDate()
                };
                
                using (var db = new PosDbContext())
                {
                    db.WorkPeriod.Add(w);
                    db.SaveChanges();
                    var ej = SendMail();
                    MessageBox.Show("Success. Item Saved.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshWorkPeriods();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
 
        private void Datagrid_Workperiods_MouseUp(object sender, MouseButtonEventArgs e)
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
                if (dep is DataGridCell)
                {
                    if (Datagrid_Workperiods.SelectedItem == null)
                    {
                        return;
                    }
                    WorkPeriod o = (WorkPeriod)Datagrid_Workperiods.SelectedItem;
                    if (o == null)
                    {
                        MessageBox.Show("The selected Work Period is not Known!!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    using (var db = new PosDbContext())
                    {
                        if (db.WorkPeriod.Where(x => x.WorkperiodName == o.WorkperiodName && x.WorkperiodStatus == "Open").Count() <= 0)
                        {
                            MessageBox.Show("This Work Period cannot be closed!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                            RefreshWorkPeriods();
                            return;
                        }
                    }
                    CloseWorkPeriod c = new CloseWorkPeriod();
                    c.ShowDialog();
                    if ((bool)c.DialogResult)
                    {
                        using (var db = new PosDbContext())
                        {
                            var wp = db.WorkPeriod.Where(a => a.WorkperiodName == o.WorkperiodName).First();
                            wp.WorkperiodStatus = "Closed";
                            wp.ClosedBy = ErpShared.CurrentUser.UserName;
                            wp.ClosingDate = ErpShared.CurrentDate();
                            db.SaveChanges();
                            var  jjj=SendMail();

                            //if (jjj.Result == false)
                            //{
                            //    MessageBox.Show("Failed to send the Email!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            //}
                            MessageBox.Show("Successfully closed the Work Period!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            RefreshWorkPeriods();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
                    }
        private async Task<bool> SendMail()
        {
            var b = await mail.SendReadyMeail("portxyz100@gmail.com", "fkiprotich845@gmail.com", "integrating async", "subject email", false);
            return b;
        }
    }
}
