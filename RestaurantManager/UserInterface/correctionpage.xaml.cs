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

namespace RestaurantManager.UserInterface
{
    /// <summary>
    /// Interaction logic for correctionpage.xaml
    /// </summary>
    public partial class correctionpage : Page
    {
        public correctionpage()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //List<string> items = new List<string>();
                //MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(GlobalVariables.SharedVariables.GetdevMysqlDbConnectionString());
                //con.Open();
                //var data = new MySql.Data.MySqlClient.MySqlCommand("select * from posdbv2.ticketpaymentitem",con).ExecuteReader();
                //while (data.Read())
                //{
                //    items.Add(data.GetString("PaymentGuid"));
                //}
                //data.Close(); 
                ////MessageBox.Show("loaded");
                ////con.Open();
                //int c = 0;
                //foreach(var x in items)
                //{
                //    c++;
                //    var st = new MySql.Data.MySqlClient.MySqlCommand("update posdbv2.ticketpaymentitem set PaymentGuid=" + c.ToString() + " where PaymentGuid=" + "'"+x+"'", con);
                //    var resp = st.ExecuteNonQuery();
                
                //}
                //MessageBox.Show("Done");
                //con.Close();
                //get ticket items
                //var db = new PosDbContext();
                //int c = 0;
                //var original = db.TicketPaymentItem.ToList();
                //foreach (var x in original)
                //{
                //    c++;
                //    x.PaymentGuid = c.ToString() ;
                //}
                //db.SaveChanges(); 
                //Datagrid_TicketItems.ItemsSource = original; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
