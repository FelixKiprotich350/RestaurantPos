using MySql.Data.MySqlClient;
using RestaurantPos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RestaurantPos.BusinessModels.OrderTickets
{
    public class OrderMaster
    {
        public string OrderDbuid { get; set; }
        public string TicketID { get; set; }
        public string OrderNumber { get; set; }
        public string Order_Waiter { get; set; }
        public string TicketTime { get; set; }
        public string TicketStatus { get; set; }
        public string TicketTable { get; set; }

        public List<OrderMaster> GetOrdersList()
        {
            List<OrderMaster> a = new List<OrderMaster>();
            try
            {

                MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                con.Open();
                MySqlCommand sql = new MySqlCommand("Select * from ordermaster;", con);
                MySqlDataReader R = sql.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        OrderMaster b = new OrderMaster
                        {
                            OrderDbuid = R.GetString("orderdbuid"),
                            TicketID = R.GetString("ticketid"),
                            Order_Waiter = R.GetString("waiter")
                        };
                        a.Add(b);
                    } 
                }
                else
                {
                    a.Clear();
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
               
            }
            return a;
        }
    }
}
