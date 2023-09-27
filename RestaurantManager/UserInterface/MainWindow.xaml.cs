using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RestaurantManager.UserInterface;
using RestaurantManager.UserInterface.Security;
using RestaurantManager.GlobalVariables;
using DatabaseModels.Navigation;
using DatabaseModels.Warehouse;
using DatabaseModels.Security;
using System.Collections.ObjectModel;
using static RestaurantManager.GlobalVariables.PosEnums;

namespace RestaurantManager.UserInterface
{
    /// <summary>
    /// MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    { 
        public ObservableCollection<PermissionMaster> AllMenuitems =new ObservableCollection<PermissionMaster>();
        public ObservableCollection<Level1menu> Modules_Collection =new ObservableCollection<Level1menu>();

        public MainWindow()
        {
            InitializeComponent();
            TextBox_Date.Text = GlobalVariables.SharedVariables.CurrentDate().ToLongDateString();  
            Frame1.Content = new HomePage();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SharedVariables.POS_MainWindow = null;
                SharedVariables.Backend_MainWindow = this;
                if (SharedVariables.CurrentUser == null)
                {
                    StackPanel_SwitchPanels.Visibility = Visibility.Collapsed;
                    MessageBox.Show("The current LoggedIn User is Null!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                    App.Current.Shutdown();
                    return;
                }
                if (SharedVariables.ClientInfo() == null )
                {
                    StackPanel_SwitchPanels.Visibility = Visibility.Collapsed;
                    MessageBox.Show("The Restaurant Profile does not Exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                    App.Current.Shutdown();
                    return;
                }
                if (SharedVariables.CurrentUser.IsPosUser)
                {
                    Textbox_SwitchTo.Text = SwitchMainWindow.SalesPoint.ToString();
                    StackPanel_SwitchPanels.Visibility = Visibility.Visible;

                } 
                TextBox_InstitutionTitle.Text = SharedVariables.ClientInfo().ClientTitle;
                //GlobalVariables.SharedVariables.Main_Window = this;
                SetupUIForUser(true);

            }
            catch (Exception ex)
            {
                string help = "\nKindly contact Technical Support Team for HELP!";
                MessageBox.Show(ex.Message + help, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
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
                var list=GlobalVariables.SharedVariables.CurrentUser.User_Permissions_final;
                foreach(var x in menu.MenuCategories)
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

        private void Image_Close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to EXIT ?", "Message Box", MessageBoxButton.YesNo, MessageBoxImage.Question,MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                this.Close();
            }
            
        }

        private void Image_Minimize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Btn_Logout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to logout ?","Message Box",MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.No)==MessageBoxResult.Yes)
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window != this&&window.Name!= "Login_Window")
                {
                    //window.Close(); 
                }
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
                        x.MenuItems = new ObservableCollection<PermissionMaster>(new Permissions().GetAllPermissions().Where(k => k.ParentModule == x.GroupCode).ToList());
                    }
                }
                ModulesListView.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StackPanel_SwitchPanels_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (Textbox_SwitchTo.Text.ToString() == SwitchMainWindow.SalesPoint.ToString())
                {
                    POSMainContainer bom = new POSMainContainer();
                    bom.Show();
                    SharedVariables.POS_MainWindow = bom;
                    SharedVariables.Backend_MainWindow = null;
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}