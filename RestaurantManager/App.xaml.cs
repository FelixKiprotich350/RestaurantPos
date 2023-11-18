using MySql.Data.MySqlClient;
using RestaurantManager.ActivityLogs;
using RestaurantManager.ApplicationFiles;
using RestaurantManager.GlobalVariables;
using RestaurantManager.UserInterface.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace RestaurantManager
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string sql = "";
        private static Mutex _mutex = null;
        private readonly string keyboardlocation = @"C:\Windows\system32\osk.exe";
        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ActivityLogger.LogFileAction(GlobalVariables.PosEnums.ActivityLogType.System.ToString(), "Application Started", "N/A");
            App.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            SetUpDatabase();
            ActivityLogger.LogDBAction(GlobalVariables.PosEnums.ActivityLogType.System.ToString(), "Application Started", "N/A");
            EventManager.RegisterClassHandler(typeof(Window), Window.LoadedEvent, new RoutedEventHandler(OnWindowLoaded));
            EventManager.RegisterClassHandler(typeof(Page), Page.LoadedEvent, new RoutedEventHandler(OnPageLoaded));
            //  EventManager.RegisterClassHandler(typeof(TextBox), FrameworkElement.GotFocusEvent, new RoutedEventHandler(Textbox_GotFocus), true);
            // EventManager.RegisterClassHandler(typeof(TextBox), FrameworkElement.LostFocusEvent, new RoutedEventHandler(Textbox_LostFocus), true);
            //EventManager.RegisterClassHandler(typeof(TextBox), FrameworkElement.LostTouchCaptureEvent, new RoutedEventHandler(Textbox_LostFocus), true);
            //EventManager.RegisterClassHandler(typeof(TextBox), FrameworkElement.LostKeyboardFocusEvent, new RoutedEventHandler(Textbox_LostFocus), true);

        }
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is Window window)
            { 
                ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), $"User  opened new Window: {window.GetType().Name}", $"{window.Title}");
            }
        }
        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is Window window)
            {
                ActivityLogger.LogDBAction(PosEnums.ActivityLogType.User.ToString(), $"User  opened new Page: {window.GetType().Name}", $"{window.Title}");
            }
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "MyAppName";

            _mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew)
            {
                MessageBox.Show("The App is Already Running!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Information); 
 
                //app is already running! Exiting the application
                Application.Current.Shutdown();
            }

            base.OnStartup(e);
        }

        private void SetUpDatabase()
        {
            try
            {

                using (var db = new PosDbContext())
                {

                    db.Database.Connection.Open();
                    db.Database.Connection.Close();

                }
                ActivityLogger.LogDBAction(GlobalVariables.PosEnums.ActivityLogType.System.ToString(), "Database Connection Checked...Success", "N/A");
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message + "\n\nDo you want to configure the server now?", "DATABASE SERVER CONNECTION", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ServerConfiguration isc = new ServerConfiguration();
                    isc.ShowDialog();
                    isc.Close();
                }
                else
                {
                    App.Current.Shutdown();
                }
            }
            return;
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
                        // p.Start(); 
                    }
                    else
                    {
                        MessageBox.Show("Keyboard not Found!", "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception Ex)
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
            catch (Exception Ex)
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
    public class HalfValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                return doubleValue * 0.8;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                return doubleValue * 1.25;
            }
            return value;
        }
    }

}
