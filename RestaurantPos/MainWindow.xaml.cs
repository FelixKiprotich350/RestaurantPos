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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RestaurantPos.UserInterface;
using RestaurantPos.BusinessModels.Security;
using RestaurantPos.BusinessModels.OrderTickets;
using RestaurantPos.UserInterface.Security; 
using RestaurantPos.BusinessModels.Administration;
using RestaurantPos.BusinessModels.Navigation;
using MaterialDesignColors;
using MaterialDesignThemes; 
using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using TagileERP.UserInterface;

namespace RestaurantPos
{
    /// <summary>
    /// MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    { 
        readonly OrderMaster om = new OrderMaster();
        ObservableCollection<OrderMaster> ol = new ObservableCollection<OrderMaster>();
        public MainWindow()
        {
            InitializeComponent();
            TextBox_Date.Text = ErpShared.CurrentDate().ToLongDateString();
            //Login l = new Login();
            //l.Textbox_Username.Text = "a";
            //l.Password_Box.Password = "123";
            //l.Button_Login_Click(new object(), new RoutedEventArgs());
            ////bool? response = l.ShowDialog();
            ////if (response != null && response == true)
            ////{
            ////    if (ErpShared.CurrentUser != null)
            ////    {
            ////        SetupMenu();
            ////    }
            ////    else
            ////    {
            ////        this.Close();
            ////    }
            ////}
            ////else
            ////{
            ////    this.Close();
            ////} 
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*
            try
            {
                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from institutioninfo;", con);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        InstitutionModelProperty s = new InstitutionModelProperty
                        {
                            ItemId = Convert.ToInt32(R.GetString("itemid")),
                            ItemName = R.GetString("itemname"),
                            Value1 = R.GetString("value1"),
                            Value2 = R.GetString("value2"),
                            Value3 = R.GetString("value3")
                        };
                        List<InstitutionModelProperty> im = imp.GetInstitutionProperties();
                        MyInstitution myi = new MyInstitution
                        {
                            InstitutionName = im.Where(b => b.ItemId == 1).First().Value1
                        };
                        ErpShared.InstitutionDetails = myi;
                    }

                }
                else
                {
                    ///do something
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                if (MessageBox.Show("Do you want to configure the server now?", "Server Message Box", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    InitialServerConfiguration isc = new InitialServerConfiguration();
                   bool? feed= isc.ShowDialog();
                    if ((bool)feed)
                    {
                        Window_Loaded(this,new RoutedEventArgs());
                    }
                }
            }
            SetupUIForUser(false); 
            */
            //  Frame1.Content = new HomePage();
            OrderMaster m = new OrderMaster();
            ol = new ObservableCollection<OrderMaster>(m.GetOrdersList());
            Listview_OrderMaster.ItemsSource = ol;
        }

        
        private void Label_Login_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ErpShared.CurrentUser == null)
            {
                Login l = new Login();
                l.Textbox_Username.Text = "a";
                l.Password_Box.Password = "123";
                // l.Button_Login_Click(new object(), new RoutedEventArgs());
                bool? response = l.ShowDialog();
                if (response != null && response == true)
                {
                    if (ErpShared.CurrentUser == null)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            } 
        }
 
 
         
        
        private void Listview_OrderMaster_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView l = sender as ListView;
            if (l.SelectedItem != null)
            {
                Pos p = new Pos();
                p.ShowDialog();
            }
            Listview_OrderMaster.SelectedItem = null;
        }
    }
}