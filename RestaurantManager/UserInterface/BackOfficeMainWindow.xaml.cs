using DatabaseModels.Navigation;
using DatabaseModels.Security;
using RestaurantManager.GlobalVariables;
using RestaurantManager.UserInterface.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static RestaurantManager.GlobalVariables.PosEnums;

namespace RestaurantManager.UserInterface
{
    /// <summary>
    /// Interaction logic for BackOfficeMainWindow.xaml
    /// </summary>
    public partial class BackOfficeMainWindow : Window
    {
        public ObservableCollection<PermissionMaster> AllMenuitems = new ObservableCollection<PermissionMaster>();
        public ObservableCollection<Level1menu> Modules_Collection = new ObservableCollection<Level1menu>();

        bool isfirsttime = true;  
        public BackOfficeMainWindow()
        {
            InitializeComponent();
            StateChanged += MainWindowStateChangeRaised;
            TextBox_Date.Text = GlobalVariables.SharedVariables.CurrentDate().ToLongDateString();
            DispatcherTimer dt = new DispatcherTimer();
            Timer_Tick(null, new EventArgs());
            dt.Tick += new EventHandler(Timer_Tick);
            dt.Interval = new TimeSpan(0, 0, 1);
            dt.Start();
            Frame1.Content = new Accounts.AccountsDashboard2();
        }

        #region
        // Can execute
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        // State change
        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (isfirsttime)
            {
                isfirsttime = false;
                SystemCommands.MaximizeWindow(this);
            }
            if (WindowState == WindowState.Maximized)
            {
                MainWindowBorder.BorderThickness = new Thickness(8);
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainWindowBorder.BorderThickness = new Thickness(0);
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
            }
        }

        #endregion

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                TextBox_Date.Text = TextBox_Date.Text = SharedVariables.CurrentDate().ToLongDateString() + " " + SharedVariables.CurrentDate().ToLongTimeString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window != this && window.Name != "Login_Window")
            //    { 
            //        window.Activate();
            //        return;
            //    }
            //} 
            if (Application.Current.Windows.Count > 1)
            {
                Application.Current.Windows[0].Activate();
                e.Cancel = false;
                return;
            }
            if (MessageBox.Show("Are you sure you want to EXIT ?", "Message Box", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                SharedVariables.POS_MainWindow = null;
                SharedVariables.Backend_MainWindow = this;
                if (SharedVariables.CurrentUser == null)
                { 
                    MessageBox.Show("The current LoggedIn User is Null!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                    App.Current.Shutdown();
                    return;
                }
                if (SharedVariables.ClientInfo() == null)
                {
                     MessageBox.Show("The Restaurant Profile does not Exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                    App.Current.Shutdown();
                    return;
                }
              
                TextBox_InstitutionTitle.Text = SharedVariables.ClientInfo().ClientTitle;
                //GlobalVariables.SharedVariables.Main_Window = this;
                SetupUIForUser(true);
                WindowState = WindowState.Maximized;
                Task.Delay(100);
            }
            catch (Exception ex)
            {
                string help = "\nKindly contact Technical Support Team for HELP!";
                MessageBox.Show(ex.Message + help, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
            }
        }
    
         
        private void SetupUIForUser(bool UserLoggedIn)
        {
            if (UserLoggedIn)
            {
                Textbox_LoggedinUserFullName.Text = GlobalVariables.SharedVariables.CurrentUser.UserFullName;
                SetupMenu();

            }
            else
            {
                this.Close();
            }

        }

        private void SetupMenu()
        {
            try
            {
                NavigationMenu menu = new NavigationMenu();
                var list = GlobalVariables.SharedVariables.CurrentUser.User_Permissions_final;
                foreach (var x in menu.MenuCategories)
                {
                    //if (list.Count(k=>k.ParentModule==x.GroupCode) >0)
                    //{
                    //    x.IsEnabled = true;
                    //    //x.BackgroundColor = Brushes.Transparent;
                    //    modules.Add(x);
                    //}
                    //else
                    //{
                    //    //x.BackgroundColor = Brushes.DarkGray;
                    //    modules.Add(x);
                    //}

                    var subitems = SharedVariables.CurrentUser.User_Permissions_final.Where(k => k.ParentModule == x.GroupCode & k.PermissionLevel == "1").ToList();
                    x.MenuItems = new ObservableCollection<PermissionMaster>(subitems);
                    Modules_Collection.Add(x);
                }
                ModulesListView.ItemsSource = Modules_Collection;
                ModulesListView.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StackPanel_Home_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Frame1.Content = new HomePage();
        }

        public void Button_Category_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button a = sender as Button;
                if (!a.IsEnabled)
                {
                    return;
                }
                string tag = a.Tag.ToString();
                if (tag != "")
                {
                    if (tag == "A")
                    {
                        using (var db = new PosDbContext())
                        {
                            if (db.WorkPeriod.Where(x => x.WorkperiodStatus == "Open").Count() <= 0)
                            {
                                Frame1.Content = "";
                                MessageBox.Show("No Work Period open for the sales!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }

                        }
                    }
                    //var subitems = GlobalVariables.SharedVariables.CurrentUser.User_Permissions_final.Where(x => x.ParentModule == tag && x.PermissionLevel == "1").ToList();
                    //Category_Submenu.ItemsSource = subitems;
                    //if (subitems.Count <= 0)
                    //{
                    //    Frame1.Content = "";
                    //    return;
                    //}
                    //Frame1.Content = subitems[0].PageClass;
                    //Category_Submenu.Visibility = Visibility.Visible;
                }
                else
                {
                    Frame1.Content = "";
                    MessageBox.Show(this, "The feature does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_CategoryPermissionItem_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Button a = sender as Button;
            //    string tag = a.Tag.ToString();
            //    if (tag != "")
            //    {
            //        if (pm.GetAllPermissions().Where(k => k.PermissionCode == tag).Count() > 0)
            //        {
            //            object UiItem = pm.GetAllPermissions().Where(k => k.PermissionCode == tag).First().PageClass;
            //            if (UiItem == null)
            //            {
            //                return;
            //            }
            //            if (UiItem.GetType() != Frame1.Content.GetType())
            //            {
            //                Frame1.Content = UiItem;
            //            }
            //            //else
            //            //{
            //            //    MessageBox.Show("The following is activated");
            //            //}
            //        }
            //    }
            //    else
            //    {
            //        Frame1.Content = "";
            //        MessageBox.Show(this, "The feature does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

        }
 
        private void Btn_Logout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to logout ?", "Message Box", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    GlobalVariables.SharedVariables.CurrentUser = null;
                    Login Login = new Login();
                    Login.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Label_Dashboard_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Frame1.Content = new HomePage();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyboardDevice.Modifiers == ModifierKeys.Control) && (e.Key == Key.E))
            {
                MessageBox.Show("Activated");
            }
            if ((e.KeyboardDevice.Modifiers == ModifierKeys.Control) && (e.Key == Key.D))
            {
                MessageBox.Show("DeActivated");
            }
        }

       

        /// <summary>
        /// new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView Lv = sender as ListView;
            try
            {
                if (Lv.SelectedItem == null)
                {
                    return;
                }
                //get selected item
                PermissionMaster mpi = (PermissionMaster)Lv.SelectedItem;
                Frame1.Content = mpi.PageClass;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Lv.SelectedItem = null;
            }
        }


        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            try
            {
                Expander exp = sender as Expander;
                foreach (var x in Modules_Collection)
                {
                    if (x.GroupCode != exp.Tag.ToString())
                    {
                        x.IsSelected = false;
                    }
                    else
                    {
                        x.IsSelected = true;
                        // x.MenuItems = new ObservableCollection<PermissionMaster>(new Permissions().GetAllPermissions().Where(k => k.ParentModule == x.GroupCode).ToList());
                    }
                }
                ModulesListView.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
           
        } 
    }
}
