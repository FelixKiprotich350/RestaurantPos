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
    /// Interaction logic for TablesEntities.xaml
    /// </summary>
    public partial class TablesEntities : Page
    {
        public TablesEntities()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshMenuProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TableEntity t = new TableEntity
                {
                    TableGuid = Guid.NewGuid().ToString(),
                    TableName = Textbox_UserFullName.Text,
                    TableStatus = "Available",
                    IsDeleted = false,
                    RegistrationDate = GlobalVariables.SharedVariables.CurrentDate()
                };
                using (var db = new PosDbContext())
                {
                    db.TableEntity.Add(t);
                    db.SaveChanges();
                    MessageBox.Show("Success. Table Saved.", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshMenuProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshMenuProducts()
        {
            try
            {
                using (var db = new PosDbContext())
                {
                    Datagrid_Tables.ItemsSource = db.TableEntity.ToList();
                }
                TextBox_TotalCount.Text = Datagrid_Tables.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       
    }
}
