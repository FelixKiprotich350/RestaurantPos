﻿using DatabaseModels.CRM;
using DatabaseModels.Payroll;
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

namespace RestaurantManager.UserInterface.HR
{
    /// <summary>
    /// Interaction logic for EmployeeProfile.xaml
    /// </summary>
    public partial class EmployeeProfile : Window
    {
        EmployeeAccount EmpAcc = null;
        public EmployeeProfile()
        {
            InitializeComponent();
        }
        public EmployeeProfile(EmployeeAccount employee)
        {
            InitializeComponent();
            EmpAcc = employee;
        }

        private void Checkbox_DefaultBirthDate_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void Checkbox_DefaultBirthDate_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
