using MySql.Data.MySqlClient;
using RestaurantPos.BusinessModels.Security;
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
using System.Windows.Shapes;
using RestaurantPos.BusinessModels.Navigation; 

namespace RestaurantPos.UserInterface.Security
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            ErpShared.CurrentUser = null;
        }

        public void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Textbox_Username.Text != "" && Password_Box.Password != "")
                { 
                    ERPUser user = new ERPUser();
                    MySqlConnection con = new MySqlConnection(ErpShared.DbConnectionstring);
                    con.Open();
                    MySqlCommand com = new MySqlCommand("select * from erpusers where username=@username and password=@password and userstate=@userstate", con);
                    com.Parameters.AddWithValue("@userstate", "Active");
                    com.Parameters.AddWithValue("@username", Textbox_Username.Text);
                    com.Parameters.AddWithValue("@password", Password_Box.Password);
                    MySqlDataReader dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        List<string> raw_roles = new List<string>();
                        List<Level2menu> All_Function_roles = new List<Level2menu>();
                        while (dr.Read())
                        {
                            user.UserName = dr["username"].ToString();
                            user.UserFullName = dr["userfullname"].ToString();
                            user.UserStatus = dr["userstate"].ToString();
                            user.UserRoles = new List<Level2menu>();
                            raw_roles = dr["roles"].ToString().Split(',').ToList<string>();
                        }
                        dr.Close();
                        com.Dispose();
                        com = new MySqlCommand("select * from masterroles", con);
                        MySqlDataReader dr1 = com.ExecuteReader();
                        while (dr1.Read())
                        { 
                             All_Function_roles.Add(new Level2menu() 
                             { 
                                 Category = dr1["rolecategory"].ToString(), 
                                 SubmenuItem = dr1["menuname"].ToString(), 
                                 Itemcode = dr1["roleid"].ToString(), 
                                 Iconkind = "plus" 
                             }); 
                        }
                        dr1.Close();
                        com.Dispose();
                        //evaluate user specific roles
                        foreach (Level2menu x in All_Function_roles)
                        {
                            //if (raw_roles.Contains(x.Itemcode))
                            //{
                            user.UserRoles.Add(x);
                            //}
                        }
                        ErpShared.CurrentUser = user;
                        this.DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show(this, "Incorrect Password!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    con.Close();
                }
                else
                {
                    MessageBox.Show(this, "Enter Username and Password!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
