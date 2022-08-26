using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RestaurantPos.BusinessModels.Administration
{
    public class InstitutionModelProperty
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public List<InstitutionModelProperty> GetInstitutionProperties()
        {
            List<InstitutionModelProperty> a = new List<InstitutionModelProperty>();
            try
            {
                a.Clear();
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
                        a.Add(s);
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
                a.Clear();
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return a;
        }
    }
}
