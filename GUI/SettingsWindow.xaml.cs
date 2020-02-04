using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private Window _MainWindow;
        public SettingsWindow()
        {
            InitializeComponent();
            this.LoadAppsList();
        }

        public SettingsWindow(Window mainWindow)
        {
            InitializeComponent();
            _MainWindow = mainWindow;
            this.LoadAppsList();
        }

        private void LoadAppsList()
        {
            var apps = GetListOfInstalledApps();
            foreach(String app in apps)
            {
                Button button = new Button
                {
                    Content = app,
                    Margin = new Thickness(2.5, 2.5, 2.5, 2.5),
                    Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#ff555b6e"),
                };
                button.Click += Button_Click;
                if(this.CheckIfAppInConfig(app))
                {
                    TrackedApps.Children.Add(button);
                }
                else
                {
                    InstalledApps.Children.Add(button);
                }                
            }            
        }

        private bool CheckIfAppInConfig(String app)
        {

            var config = ConfigurationManager.OpenExeConfiguration("TimerService.exe");
            return config.AppSettings.Settings["AppsToTrack"].Value.Contains(app);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InstalledApps.Children.Remove(sender as UIElement);
            TrackedApps.Children.Add(sender as UIElement);
            (sender as Button).Click += RemoveButton_Click;
            (sender as Button).Click -= Button_Click;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            TrackedApps.Children.Remove(sender as UIElement);
            InstalledApps.Children.Add(sender as UIElement);
            (sender as Button).Click -= Button_Click;
            (sender as Button).Click += RemoveButton_Click;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_MainWindow != null)
            {
                _MainWindow.IsEnabled = true;
                _MainWindow.Show();
            }
        }
        private List<String> GetListOfInstalledApps()
        {
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            List<String> installedApps = new List<string>();
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {
                            String displayName = (sk.GetValue("DisplayName") as String);
                            if (displayName != null)
                            {
                                installedApps.Add(displayName);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            return installedApps;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration("TimerService.exe");
            StringBuilder builder = new StringBuilder();
            foreach (var el in TrackedApps.Children)
            {
                builder.Append($"{(el as Button).Content},");
            }
            config.AppSettings.Settings["AppsToTrack"].Value = builder.ToString();
            config.Save();
            MessageBox.Show("Configuration sucessfully saved!","Settings",MessageBoxButton.OK,MessageBoxImage.Information);
        }
    }
}
