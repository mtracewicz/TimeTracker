using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.IO;

namespace GUI
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly Window _MainWindow;
        private List<String> _AppsToTrack;
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
            _AppsToTrack = GetAppsInConfig().ToList();
            foreach (String appName in _AppsToTrack)
            {
                if (!String.IsNullOrWhiteSpace(appName))
                {
                    DockPanel dockPanel = ControlsFactory.CreateDockpanel(appName, RemoveButton_Click);
                    TrackedAppsPanel.Children.Add(dockPanel);
                }
            }
        }

        private IEnumerable<string> GetAppsInConfig()
        {
            var config = ConfigurationManager.OpenExeConfiguration("TimerService.exe");
            return config.AppSettings.Settings["AppsToTrack"].Value.Split(',');
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            TrackedAppsPanel.Children.Remove((sender as Button).Parent as UIElement);
            _AppsToTrack.Remove((((sender as Button).Parent as DockPanel).Children[1] as Label).Content as String);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_MainWindow != null)
            {
                _MainWindow.IsEnabled = true;
                _MainWindow.Show();
            }
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration("TimerService.exe");
            StringBuilder builder = new StringBuilder();
            foreach (var app in _AppsToTrack)
            {
                builder.Append($"{app},");
            }
            config.AppSettings.Settings["AppsToTrack"].Value = builder.ToString();
            config.Save();
            MessageBox.Show("Configuration sucessfully saved!", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddAppButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "Executable Files (.exe)|*.exe"
            };
            fileDialog.ShowDialog();
            if (!_AppsToTrack.Contains(fileDialog.FileName))
            {
                DockPanel dockPanel = ControlsFactory.CreateDockpanel(fileDialog.FileName, RemoveButton_Click);
                TrackedAppsPanel.Children.Add(dockPanel);
                _AppsToTrack.Add(fileDialog.FileName);
            }
            else 
            {
                MessageBox.Show("App is already added!", "Duplicate!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
