using DatabaseModels.Security;
using RestaurantManager.ActivityLogs;
using RestaurantManager.ApplicationFiles; 
using RestaurantManager.GlobalVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RestaurantManager.UserInterface.Security
{
    /// <summary>
    /// Interaction logic for PromptAdminPin.xaml
    /// </summary>
    public partial class PromptAdminPin : Window
    {
        public string ApprovingAdmin = "";
        public PromptAdminPin(string  message)
        {
            InitializeComponent();
            Textbloc_ActionDescription.Text = message;
        }
        public void Button_Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (PasswordBox_UserPin.Password != "")
                {
                    PosUser user = null;
                    using (var db = new PosDbContext())
                    {
                        if (db.PosUser.Where(a => a.UserPIN.ToString() == PasswordBox_UserPin.Password.Trim()).Count() > 0)
                        {
                            user = db.PosUser.Where(a => a.UserPIN.ToString() == PasswordBox_UserPin.Password.Trim()).First();
                            if (user.UserRole == PosEnums.UserAccountsRoles.Admin.ToString())
                            {
                                ApprovingAdmin = user.UserName;
                                DialogResult = true;
                                ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), "Approve specific action as Admin", Textbloc_ActionDescription.Text);

                            }
                            else
                            {
                                this.DialogResult = false;
                            }
                        }
                        else
                        {
                            this.DialogResult = false;
                        }
                         
                    }
                     
                  //  Close();

                }
                else
                {
                    MessageBox.Show(this, "Enter User PIN!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PasswordBox_UserPin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {

                Regex regex = new Regex("[0-9]+");
                if (!regex.IsMatch(e.Text))
                {
                    e.Handled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Number Pad

        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 3;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 4;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 5;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 6;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_7_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 7;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_8_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 8;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_9_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 9;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_0_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PasswordBox_UserPin.Password += 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PasswordBox_UserPin.Focus();
        } 
    }
}
