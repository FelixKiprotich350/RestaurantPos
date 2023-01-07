using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace RestaurantManager.ApplicationFiles
{
    /// <summary>
    /// Interaction logic for InitialServerConfiguration.xaml
    /// </summary>
    public partial class InitialServerConfiguration : Window
    {
        public InitialServerConfiguration()
        {
            InitializeComponent();
        } 
        private void Button_Test_Click(object sender, RoutedEventArgs e)
        {
          
            //test if dataase exixtss
            try
            {
                AppStaticvalues.DbServer = Textbox_ServerIP.Text;
                AppStaticvalues.DbUser = Textbox_ServerUserID.Text;
                AppStaticvalues.DbPassword = Textbox_ServerPassword.Text;
                AppStaticvalues.DbPort = Textbox_ServerPort.Text;
                using (var db = new ApplicationFiles.PosDbContext())
                {
                    Debug.WriteLine(db.Database.Connection.ConnectionString);
       
                    if (!db.Database.Exists())
                    {
                        db.Database.CreateIfNotExists();
                    }
                    db.Database.Connection.Open();
                    db.Database.Connection.Close();
                    Properties.Settings.Default.String1= Textbox_ServerIP.Text;
                    Properties.Settings.Default.String2 = Textbox_ServerUserID.Text;
                    Properties.Settings.Default.String3 = Textbox_ServerPassword.Text;
                    Properties.Settings.Default.String4= Textbox_ServerPort.Text;
                    Properties.Settings.Default.Save(); 
                    MessageBox.Show("DATABASE CONNECTION STATUS : SUCCESS", "DATABASE SERVER CONNECTION", MessageBoxButton.OK, MessageBoxImage.Information);

                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DATABASE SERVER CONNECTION", MessageBoxButton.OK, MessageBoxImage.Error);
                
                AppStaticvalues.DbServer = "";
                AppStaticvalues.DbPort = "";
                AppStaticvalues.DbUser = "";
                AppStaticvalues.DbPassword = "";
                Close();
            }
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        { 
            this.Close();
        }
    }
}
