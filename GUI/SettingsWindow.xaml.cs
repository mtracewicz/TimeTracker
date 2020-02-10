using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly Window _MainWindow;
        private List<String> _AppsToTrack;
        private Config _Config;
        
        public SettingsWindow()
        {
            InitializeComponent();
            this.SetUp();
        }

        public SettingsWindow(Window mainWindow)
        {
            InitializeComponent();
            _MainWindow = mainWindow;
            this.SetUp();
        }

        private void SetUp()
        {
            this._Config = new Config("TimerService.exe");
            this.LoadAppsList();
            this.LoadTimerAccuracy();            
        }

        private void LoadTimerAccuracy()
        {           
            TimerAccuracyBox.Text = _Config.GetTimerInterval();
        }

        private void LoadAppsList()
        {
            _AppsToTrack = _Config.GetAppsToTrack().ToList();
            foreach (String appName in _AppsToTrack)
            {
                if (!String.IsNullOrWhiteSpace(appName))
                {
                    DockPanel dockPanel = ControlsFactory.CreateDockpanel(appName, RemoveButton_Click);
                    TrackedAppsPanel.Children.Add(dockPanel);
                }
            }
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
            _Config.SaveAppsToTrack(_AppsToTrack);
            _Config.SaveTimerInterval(TimerAccuracyBox.Text);
            MessageBox.Show("Configuration sucessfully saved!", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddAppButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "Executable Files (.exe)|*.exe"
            };
            fileDialog.ShowDialog();

            if (String.IsNullOrEmpty(fileDialog.FileName))
            {
                return;
            }

            string app = Path.GetFileName(fileDialog.FileName);
            if (!_AppsToTrack.Contains(app))
            {
                DockPanel dockPanel = ControlsFactory.CreateDockpanel(app, RemoveButton_Click);
                TrackedAppsPanel.Children.Add(dockPanel);
                _AppsToTrack.Add(app);
            }
            else
            {
                MessageBox.Show("App is already added!", "Duplicate!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
