﻿using MySql.Data.MySqlClient;
using RestaurantManager.BusinessModels.Security;
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
using RestaurantManager.BusinessModels.Navigation;
using System.Text.RegularExpressions;
using System.Diagnostics;
using RestaurantManager.ApplicationFiles;
using RestaurantManager.GlobalVariables;
using RestaurantManager.BusinessModels.GeneralSettings;

namespace RestaurantManager.UserInterface.Security
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        readonly PermissionMaster Pm = new PermissionMaster();
        public Login()
        {
           
            InitializeComponent();
            SharedVariables.CurrentUser = null;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            { 
                using (var a = new PosDbContext())
                {
                    if (!a.Database.Exists())
                    {
                        MessageBox.Show("THE DATABASE DOES NOT EXIST!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                        App.Current.Shutdown();
                    }
                }
                FirstAdminUser();
                FirstCompanyProfile();
                try
                {
                    using (var a = new PosDbContext())
                    {
                        if (a.ClientInfo.Count() <= 0)
                        {

                        }
                        if (a.UserRoles.Where(x => x.RoleName == PosEnums.UserAccountsRoles.Admin.ToString()).Count() <= 0)
                        {
                            UserRole r = new UserRole
                            {
                                RoleName = PosEnums.UserAccountsRoles.Admin.ToString(),
                                RoleGuid = Guid.NewGuid().ToString(),
                                RoleIsDeleted = "False",
                                RoleDescription = "Systemm Administrator",
                                RoleStatus = "Active",
                                LastUpdateDate = SharedVariables.CurrentDate(),
                                RegistrationDate = SharedVariables.CurrentDate(),
                                RolePermissions = "All"
                            };
                            a.UserRoles.Add(r);
                            a.SaveChanges();
                        }
                    }
                    PasswordBox_UserPin.Focus();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);

                App.Current.Shutdown();
            }
        }

        public void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                if (PasswordBox_UserPin.Password != "" )
                {
                    PosUser user = null;
                    using (var db = new PosDbContext())
                    {
                        if (db.PosUser.Where(a => a.UserPIN.ToString() == PasswordBox_UserPin.Password.Trim()).Count() > 0)
                        {
                            user = db.PosUser.Where(a => a.UserPIN.ToString() == PasswordBox_UserPin.Password.Trim()).First();
                        }
                        else
                        {
                            MessageBox.Show(this, "The User PIN is Wrong!!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            PasswordBox_UserPin.Password = "";
                            return;
                        }
                        if (user.UserWorkingStatus.ToLower() != "active")
                        {
                            MessageBox.Show(this, "The User Status is not Active.\nAsk the administrator to enable your activenes!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            PasswordBox_UserPin.Password = "";
                            user = null;
                            return;
                        }
                        if (user.UserIsDeleted)
                        {
                            MessageBox.Show(this, "The UserPIN has been deleted. Enter Different PIN!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Warning);
                            PasswordBox_UserPin.Password = "";
                            user = null;
                            return;
                        }
                    }
                    MainWindow m = new MainWindow();
                    List<string> raw = new List<string>();
                    //raw permissions
                    UserRole r = null;
                    using (var db = new PosDbContext())
                    {
                        if (db.UserRoles.Where(a => a.RoleName.ToString() == user.UserRole).Count() > 0)
                        {
                            r = db.UserRoles.Where(a => a.RoleName.ToString() == user.UserRole).First();
                        }
                        else
                        {
                            return;
                        }
                    }
                    raw = r.RolePermissions.Split(',').Where(a => a.Trim() != "").ToList();
                    //final permissions
                    if (user.UserRole == PosEnums.UserAccountsRoles.Admin.ToString())
                    {
                        user.User_Permissions_final = new List<PermissionMaster>();
                        user.User_Permissions_final.AddRange(Pm.GetAllPermissions());
                    }
                    else if (raw.Count > 0)
                    {
                        user.User_Permissions_final = new List<PermissionMaster>();
                        foreach (var a in raw)
                        {
                            if (Pm.GetAllPermissions().Where(b => b.PermissionCode == a).Count() > 0)
                            {
                                user.User_Permissions_final.Add(Pm.GetAllPermissions().Where(b => b.PermissionCode == a).First());
                            }
                        }
                    }
                    else
                    {
                        user.User_Permissions_final = new List<PermissionMaster>();
                    }
                    //using (var db = new PosDbContext())
                    //{
                    //    if (db.ClientInfo.Count() > 0)
                    //    {
                    //        GlobalVariables.SharedVariables.ClientInfo = db.ClientInfo.First();
                    //    }
                    //}
                    SharedVariables.CurrentUser = user;
                    m.Show();
                    Close();
                    
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

        private void FirstAdminUser()
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    if (db.PosUser.Count() <= 0)
                    {

                        PosUser user = new PosUser()
                        {
                            UserGuid = Guid.NewGuid().ToString(),
                            UserFullName = "Admin-Default",
                            UserPIN = 1234,
                            UserName = "Admin",
                            UserIsDeleted = false,
                            UserRole = PosEnums.UserAccountsRoles.Admin.ToString(),
                            UserWorkingStatus = PosEnums.UserAccountStatuses.Active.ToString(),
                            UserRights = ",",
                            LastLoginDate = SharedVariables.CurrentDate(),
                            RegistrationDate = SharedVariables.CurrentDate()
                        };
                        db.PosUser.Add(user);
                        db.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                App.Current.Shutdown();
            }
        }

        private void FirstCompanyProfile()
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    if (db.ClientInfo.Count() <= 0)
                    {

                        ClientInfoDetails client = new ClientInfoDetails()
                        {
                            ClientGuid = Guid.NewGuid().ToString(),
                            ClientTitle = "Restaurant Title",
                            ClientKRAPIN = "Tax PIN",
                            PhysicalAddress = "Admin",
                            AcceptedCards = ",",
                            Email = "Client Email",
                            Phone = "Phone Number",
                            TaxPercentage = 1,
                            ReceiptNote1 = "Note",
                            ReceiptNote2 = "Note",
                            ReceiptNote3 = "Note"
                        };
                        db.ClientInfo.Add(client);
                        db.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                App.Current.Shutdown();
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
         
    }
}
