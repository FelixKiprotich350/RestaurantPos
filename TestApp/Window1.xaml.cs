using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using DatabaseModels.Payroll;
using DatabaseModels.Inventory;
using DatabaseModels.OrderTicket;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        } 
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                dg.ItemsSource = null;
                var result = ApiClient.GetResponse("api/ordermaster");
                if (result != null)
                {
                    if (result.IsSuccessStatusCode)
                    {
                        string content = await result.Content.ReadAsStringAsync();
                        List<OrderMaster> employees = JsonConvert.DeserializeObject<List<OrderMaster>>(content);
                        
                        MessageBox.Show(employees.Count.ToString());
                        dg.AutoGenerateColumns = true;
                        dg.ItemsSource = employees;
                    }
                    else
                    {
                        // Handle error
                        MessageBox.Show($"Error: {result.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void btn_post_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string jsonBody = JsonConvert.SerializeObject(new EmployeeAccount() {
                    EmployeeNo="123",
                    EmpRecordID=1,
                    AccountStatus="active",
                    Gender="M"
                });

                var result = ApiClient.PostResponse("api/employee",new StringContent(jsonBody,Encoding.UTF8, "application/json"));
                if (result != null)
                {
                    if (result.IsSuccessStatusCode)
                    {
                        string content = await result.Content.ReadAsStringAsync();
                         
                        MessageBox.Show("insertted");
                    }
                    else
                    {
                        // Handle error
                        MessageBox.Show($"Error: {result.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
