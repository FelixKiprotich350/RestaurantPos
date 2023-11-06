using DatabaseModels.CRM;
using RestaurantManager.ApplicationFiles; 
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

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for SelectCustomerName.xaml
    /// </summary>
    public partial class SelectEmployee : Window
    {
        public EmployeeAccount SelectedCustomer = null;
        public SelectEmployee()
        {
            InitializeComponent(); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                 
                using (var db = new PosDbContext())
                {
                    var data = db.EmployeeAccount.ToList();
                    foreach (var x in data)
                    {
                        var p = db.PersonalAccount.AsNoTracking().FirstOrDefault(k => k.AccountNo == x.PersonAccNo);
                        x.FullName = p.FullName;
                        x.Gender = p.Gender;
                        x.PhoneNo = p.PhoneNumber;
                        x.PersonAccNo = p.AccountNo;
                    }
                    Listview_Employees.ItemsSource = data;
                } 

            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            SelectedCustomer = null;
            this.Close();
        }

        private void Button_Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedCustomer!=null)
                { 
                    this.DialogResult = true;
                }
                else
                {
                    Close();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
        }

        private void Listview_Customers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Listview_Employees.SelectedItem != null)
                {
                    EmployeeAccount cust = (EmployeeAccount)Listview_Employees.SelectedItem;
                    Textbox_SelectedCustomerPhone.Text = cust.FullName + " - " + cust.PersonAccNo;
                    Textbox_SelectedCustomerPhone.Tag = cust.PhoneNo;
                    SelectedCustomer = cust;
                }
                
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
            Listview_Employees.SelectedItem = null;
        }
    }
}
