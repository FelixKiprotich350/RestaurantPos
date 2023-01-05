using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RestaurantManager
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string keyboardlocation = @"C:\Windows\system32\osk.exe";
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            App.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            EventManager.RegisterClassHandler(typeof(TextBox), FrameworkElement.GotFocusEvent, new RoutedEventHandler(Textbox_GotFocus), true);
            EventManager.RegisterClassHandler(typeof(TextBox), FrameworkElement.LostFocusEvent, new RoutedEventHandler(Textbox_LostFocus), true);
            EventManager.RegisterClassHandler(typeof(TextBox), FrameworkElement.LostTouchCaptureEvent, new RoutedEventHandler(Textbox_LostFocus), true);
            EventManager.RegisterClassHandler(typeof(TextBox), FrameworkElement.LostKeyboardFocusEvent, new RoutedEventHandler(Textbox_LostFocus), true);

            //preloader
            //configure firstrun client info and user
            //initializedbb
        }
        private void Textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox tb = sender as TextBox;
                if (!tb.IsReadOnly)
                {
                    if (File.Exists(keyboardlocation))
                    {
                        Process p = new Process();
                        p.StartInfo.FileName = keyboardlocation; 
                        //p.StartInfo.Arguments = "node fileWithCommands.js";
                        p.Start(); 
                    }
                    else
                    {
                        MessageBox.Show("Keyboard not Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox tb = sender as TextBox;
                if (File.Exists(keyboardlocation))
                {
                    Process p = Process.GetProcesses().Where(k => k.ProcessName == "osk").FirstOrDefault();
                    if (p != null)
                    {
                        p.Kill();
                    }
                }
                else
                {
                    MessageBox.Show("Keyboard not Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch(Exception Ex)
            { 
                MessageBox.Show(Ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class TabSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            TabControl tabControl = values[0] as TabControl;
            double width = tabControl.ActualWidth / tabControl.Items.Count;
            //Subtract 1, otherwise we could overflow to two rows.
            return (width <= 1) ? 0 : (width - 1);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
