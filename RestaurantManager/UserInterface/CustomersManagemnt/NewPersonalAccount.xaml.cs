using DatabaseModels.CRM;
using RestaurantManager.ActivityLogs;
using RestaurantManager.ApplicationFiles;
using RestaurantManager.GlobalVariables;
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

namespace RestaurantManager.UserInterface.CustomersManagemnt
{
    /// <summary>
    /// Interaction logic for NewCustomer.xaml
    /// </summary>
    public partial class NewPersonalAccount : Window
    {
        readonly string EditingPersonID = "";
        PersonalAccount Person = null;
        //for adding new person
        public NewPersonalAccount()
        {
            InitializeComponent();
        }
        //for editing
        public NewPersonalAccount(string personid)
        {
            InitializeComponent();
            EditingPersonID = personid;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EditingPersonID == "")
                {
                    this.Title = "Add New Person";
                }
                else
                {
                    this.Title = "Edit Personal Details";
                    var db = new PosDbContext();
                    Person= db.PersonalAccount.AsNoTracking().FirstOrDefault(k=>k.AccountNo==EditingPersonID);
                    
                    if (Person == null)
                    {
                        MessageBox.Show("The Selected Account Does not Exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    this.Title = "Editing "+ Person.FullName +" Details";
                    Textbox_Email.Text = Person.Email;
                    Textbox_FullName.Text = Person.FullName;
                    Textbox_PhoneNo.Text = Person.PhoneNumber;
                    Textbox_National.Text = Person.NationalID;
                    Combobox_Gender.SelectedItem= Person.Gender;
                    DatePicker_BirthDate.SelectedDate = Person.BirthDate;
                    Button_Save.Content = "Update";

                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Button_Save.Content.ToString() == "Update")
                {
                    var db = new PosDbContext();
                    var p= db.PersonalAccount.FirstOrDefault(k => k.AccountNo == EditingPersonID);
                    p.FullName = Textbox_FullName.Text;
                    p.PhoneNumber = Textbox_PhoneNo.Text;
                    p.NationalID = Textbox_National.Text.Trim();
                    p.Gender = Combobox_Gender.SelectedItem.ToString();
                    p.Email = Textbox_Email.Text.Trim().ToString();
                    p.BirthDate = (DateTime)DatePicker_BirthDate.SelectedDate;
                    db.SaveChanges();
                    MessageBox.Show("Successfully Saved!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "updated personal account", "account no=" + p.AccountNo);
                }
                else if (Button_Save.Content.ToString() == "Save")
                {
                    string accno = "ACC-" + GlobalVariables.SharedVariables.GeneratePersonAccNo();
                    PersonalAccount c = new PersonalAccount
                    {
                        PersonGuid = Guid.NewGuid().ToString(),
                        FullName = Textbox_FullName.Text,
                        AccountNo = accno,
                        InvoiceLimit = 0,
                        AccountStatus = GlobalVariables.PosEnums.PersonAccountStatus.Active.ToString(),
                        PhoneNumber = Textbox_PhoneNo.Text,
                        NationalID = Textbox_National.Text.Trim(),
                        Gender = Combobox_Gender.SelectedItem.ToString(),
                        Email = Textbox_Email.Text.Trim().ToString(),
                        RegistrationDate = GlobalVariables.SharedVariables.CurrentDate(),
                        BirthDate = (DateTime)DatePicker_BirthDate.SelectedDate,
                        UpdateDate = GlobalVariables.SharedVariables.CurrentDate()
                    };
                    using (var db = new PosDbContext())
                    {
                        db.PersonalAccount.Add(c);
                        db.SaveChanges();
                        ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Added new personal account", "account no=" + c.AccountNo);
                    }
                    MessageBox.Show("Successfully Saved!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                
                Close();
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Checkbox_DefaultBirthDate_Checked(object sender, RoutedEventArgs e)
        {
            DatePicker_BirthDate.SelectedDate = DateTime.MinValue;
        }

        private void Checkbox_DefaultBirthDate_Unchecked(object sender, RoutedEventArgs e)
        {
            DatePicker_BirthDate.SelectedDate = null;
        }

    
    }
}
