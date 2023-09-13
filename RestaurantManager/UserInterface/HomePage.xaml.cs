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
using DatabaseModels.Navigation;
using RestaurantManager.GlobalVariables;
using RestaurantManager.UserInterface.Security;

namespace RestaurantManager.UserInterface
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    { 
        public HomePage()
        {
            InitializeComponent(); 
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Button_PoS.Tag = "A";
                Button_WorkPeriod.Tag = "B";
                Button_Accounts.Tag = "C";
                Button_MenuProducts.Tag = "D";
                Button_Security.Tag = "F";
                Button_Reports.Tag = "E";
                NavigationMenu menu = new NavigationMenu();
                List<Level1menu> modules = new List<Level1menu>();
                var list = GlobalVariables.SharedVariables.CurrentUser.User_Permissions_final;
                foreach (var x in menu.MenuCategories)
                {
                    if (list.Count(k => k.ParentModule == x.GroupCode) > 0)
                    {
                        x.IsEnabled = true;
                        //x.BackgroundColor = Brushes.Transparent;
                        modules.Add(x);
                    }
                    else
                    {
                        //x.BackgroundColor = Brushes.DarkGray;
                        modules.Add(x);
                    }
                }
                if (modules.FirstOrDefault(k => k.GroupCode == "A"&&k.IsEnabled) != null)
                {
                    Button_PoS.IsEnabled = true;
                }
                if (modules.FirstOrDefault(k => k.GroupCode == "B"&&k.IsEnabled) != null)
                {
                    Button_WorkPeriod.IsEnabled = true;
                }
                if (modules.FirstOrDefault(k => k.GroupCode == "C"&&k.IsEnabled) != null)
                {
                    Button_Accounts.IsEnabled = true;
                }
                if (modules.FirstOrDefault(k => k.GroupCode == "D"&&k.IsEnabled) != null)
                {
                    Button_MenuProducts.IsEnabled = true;
                }
                if (modules.FirstOrDefault(k => k.GroupCode == "F"&&k.IsEnabled) != null)
                {
                    Button_Security.IsEnabled = true;
                }
                if (modules.FirstOrDefault(k => k.GroupCode == "E"&&k.IsEnabled) != null)
                {
                    Button_Reports.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 
      
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button x = (Button)sender;
                if (x.IsEnabled)
                {
                    if (SharedVariables.POS_MainWindow != null)
                    {
                        //SharedVariables.POS_MainWindow.Button_Category_Click(sender, e);
                        return;
                    } 
                    if (SharedVariables.Backend_MainWindow != null)
                    {
                        //SharedVariables.Backend_MainWindow.Button_Category_Click(sender, e);
                        return;
                    } 
                }
               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
        private void Button_PoS_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
///// <summary>
///// Code to implement Textbox Backround/Placeholder
///// </summary>
///// <param name="sender"></param>
///// <param name="e"></param>
//private void TextBox_Username_TextChanged(object sender, TextChangedEventArgs e)
//{
//    try
//    {
//        //if (TextBox_Username.Text == "")
//        //{
//        //    // Create an ImageBrush.
//        //    ImageBrush textImageBrush = new ImageBrush
//        //    {
//        //        ImageSource =
//        //        new BitmapImage(
//        //            new Uri(@"D:\Felix\TagileERP\img3.png", UriKind.Absolute)
//        //        ),
//        //        AlignmentX = AlignmentX.Left,
//        //        Stretch = Stretch.Fill
//        //    };
//        //    // Use the brush to paint the button's background.
//        //    TextBox_Username.Background = textImageBrush;
//        //}
//        //else
//        //{

//        //    TextBox_Username.Background = null;
//        //}
//    }
//    catch (Exception ex)
//    {
//        MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
//    }
//}
