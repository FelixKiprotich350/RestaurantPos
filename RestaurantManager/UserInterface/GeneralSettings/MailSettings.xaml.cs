using RestaurantManager.ApplicationFiles;
using RestaurantManager.BusinessModels.GeneralSettings;
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

namespace RestaurantManager.UserInterface.GeneralSettings
{
    /// <summary>
    /// Interaction logic for MailSettings.xaml
    /// </summary>
    public partial class MailSettings : Page
    {
        public MailSettings()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Textbox_DisplayName.IsReadOnly = true;
                Textbox_SenderAddress.IsReadOnly = true;
                Textbox_DestinationAddress.IsReadOnly = true;
                Textbox_MailingAddress.IsReadOnly = true;
                Passwordbox_AppPassword.IsEnabled = false ;
                LoadCredentials(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadCredentials(bool isfirst)
        {
            try
            {
                using(var db =new PosDbContext())
                {
                    var pos = db.MailingProfile.ToList().FirstOrDefault();
                    if (pos != null)
                    {
                        Textbox_ProfileName.Text = pos.ProfileName;
                        Textbox_SenderAddress.Text = pos.SenderAddress;
                        Textbox_MailingAddress.Text = pos.SenderAddress;
                        Textbox_DisplayName.Text = pos.DisplayName;
                        Textbox_DestinationAddress.Text = pos.DestinationAddress;
                    }
                    else
                    {
                        MailingProfile m = new MailingProfile
                        {
                            RowGuid = Guid.NewGuid().ToString(),
                            ProfileName = "Default",
                            DestinationAddress = "default",
                            SenderAddress = "default",
                            MailingAddress = "default",
                            DisplayName = "default",
                            AppPassword = "default"
                        };
                        db.MailingProfile.Add(m);
                        db.SaveChanges();
                        if (isfirst)
                        {
                            LoadCredentials(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckBox_EditMode_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Textbox_DisplayName.IsReadOnly = false;
                Textbox_SenderAddress.IsReadOnly = false;
                Textbox_DestinationAddress.IsReadOnly = false;
                Textbox_MailingAddress.IsReadOnly = false;
                Passwordbox_AppPassword.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckBox_EditMode_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Textbox_DisplayName.IsReadOnly = true;
                Textbox_SenderAddress.IsReadOnly = true;
                Textbox_DestinationAddress.IsReadOnly = true;
                Textbox_MailingAddress.IsReadOnly = true;
                Passwordbox_AppPassword.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((bool)CheckBox_EditMode.IsChecked)
                {
                    using (var db = new PosDbContext())
                    {
                        MailingProfile m = db.MailingProfile.Where(x => x.ProfileName == Textbox_ProfileName.Text).FirstOrDefault();
                        if (m != null)
                        {
                            m.DisplayName = Textbox_DisplayName.Text;
                            m.DestinationAddress = Textbox_DestinationAddress.Text;
                            m.SenderAddress = Textbox_SenderAddress.Text;
                            m.MailingAddress = Textbox_MailingAddress.Text;
                            m.AppPassword = Passwordbox_AppPassword.Password;
                            db.SaveChanges();
                            MessageBox.Show("Successfully Saved!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("The Profile does not exist!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadCredentials(true);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Read Mode Enabled! Check the Edit Mode to save!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Test_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
