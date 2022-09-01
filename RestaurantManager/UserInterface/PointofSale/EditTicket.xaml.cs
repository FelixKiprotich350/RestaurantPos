﻿using System;
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

namespace RestaurantManager.UserInterface.PointofSale
{
    /// <summary>
    /// Interaction logic for EditTicket.xaml
    /// </summary>
    public partial class EditTicket : Page
    {
        public EditTicket()
        {
            InitializeComponent();
        }
        private void Button_SelectTicket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectTicketToEdit st = new SelectTicketToEdit();
                st.ShowDialog();
                //using (var b = new PosDbContext())
                //{
                //    var a = b.ProductCategory.ToList();
                //    a.ForEach(s => s.GetMenuItems(s.CategoryGuid));
                //    Categories_ListView.ItemsSource = a;
                //}
                //Datagrid_OrderItems.ItemsSource = OrderItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}