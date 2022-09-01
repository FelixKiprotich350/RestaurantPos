using System;
using System.Data;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RestaurantManager.UserInterface;
using RestaurantManager.BusinessModels.Security;
using RestaurantManager.UserInterface.Security;  
using RestaurantManager.BusinessModels.Navigation;
using MaterialDesignColors;
using MaterialDesignThemes; 
using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using RestaurantManager.UserInterface.PointofSale;
using System.Diagnostics;

namespace RestaurantManager
{
    /// <summary>
    /// MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly PermissionMaster pm = new PermissionMaster();

        public MainWindow()
        {
            InitializeComponent();
            TextBox_Date.Text = ErpShared.CurrentDate().ToLongDateString();  
            Frame1.Content = new HomePage();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
             
                //TextBox_InstitutionTitle.Text = ErpShared.ClientInfo.ClientTitle;
                ErpShared.Main_Window = this;
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
                SetupMenu();  
                Textbox_LoggedinUserName.Text = ErpShared.CurrentUser.UserFullName;
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
                ModulesListView.ItemsSource = menu.MenuCategories; 
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

        private void Button_Category_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button a = sender as Button;
                string tag = a.Tag.ToString();
                if (tag != "")
                {
                    var subitems = ErpShared.CurrentUser.User_Permissions_final.Where(x => x.ParentModule == tag).ToList();
                    Category_Submenu.ItemsSource = subitems;
                    if (subitems.Count <= 0)
                    {
                        Frame1.Content = "";
                        return;
                    }
                    Frame1.Content = subitems[0].PageClass;
                    Category_Submenu.Visibility = Visibility.Visible;
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
            try
            {
                Button a = sender as Button;
                string tag = a.Tag.ToString();
                if (tag != "")
                {
                    if (pm.GetAllPermissions().Where(k => k.PermissionGuid == tag).Count() > 0)
                    {
                        object UiItem = pm.GetAllPermissions().Where(k => k.PermissionGuid == tag).First().PageClass;
                        if (UiItem == null)
                        {
                            return;
                        }
                        if (UiItem.GetType() != Frame1.Content.GetType())
                        {
                            Frame1.Content = UiItem;
                        }
                        else
                        {
                            MessageBox.Show("The following is activated");
                        }
                    }
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
                    ErpShared.CurrentUser = null;
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
            Category_Submenu.ItemsSource = null;
        }
 
    }
}