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
            //Textbox_InstitutionMotto.Text = "Innovation & Excellence";
            //Textbox_InstitutionName.Text = ErpShared.InstitutionDetails.InstitutionName;
        } 
        /// <summary>
        /// Code to implement Textbox Backround/Placeholder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_Username_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //if (TextBox_Username.Text == "")
                //{
                //    // Create an ImageBrush.
                //    ImageBrush textImageBrush = new ImageBrush
                //    {
                //        ImageSource =
                //        new BitmapImage(
                //            new Uri(@"D:\Felix\TagileERP\img3.png", UriKind.Absolute)
                //        ),
                //        AlignmentX = AlignmentX.Left,
                //        Stretch = Stretch.Fill
                //    };
                //    // Use the brush to paint the button's background.
                //    TextBox_Username.Background = textImageBrush;
                //}
                //else
                //{

                //    TextBox_Username.Background = null;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
     

    }
}
