using DataBaseLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServiceController _ServiceController;
        private List<RecordModel> _Records;
        public MainWindow()
        {
            InitializeComponent();
            SetUpService();
            LoadData();
        }

        private void SetUpService()
        {
            _ServiceController = new ServiceController("TimeTrackerService");
            if(_ServiceController.Status != ServiceControllerStatus.Running || _ServiceController.Status != ServiceControllerStatus.StartPending)
            {
                _ServiceController.Start();
                ServiceStatusLabel.Content = "Running";
            }            
            ServiceStatusLabel.Background = Utils.GetBrushFromHex("#ff89b0ae");
            EnableStartStopButtons(true);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _ServiceController.Start();
            ServiceStatusLabel.Content = "Running";
            ServiceStatusLabel.Background = Utils.GetBrushFromHex("#ff89b0ae");
            EnableStartStopButtons(true);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

            _ServiceController.Stop();
            ServiceStatusLabel.Content = "Stoped";
            ServiceStatusLabel.Background = Utils.GetBrushFromHex("#fff03e47");
            EnableStartStopButtons(false);
        }

        private void EnableStartStopButtons(bool running)
        {
            StartButton.IsEnabled = !running;
            StopButton.IsEnabled = running;
            SettingsButton.IsEnabled = !running;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow(this);
            this.IsEnabled = false;
            settings.Show();
            this.Hide();
        }
        private async void LoadData()
        {
            try
            {
                using (TimeTrackerServerReference.RecordsClient recordsClient = new TimeTrackerServerReference.RecordsClient())
                {
                    _Records = Utils.Convert(await recordsClient.GetRecordsDataAsync());
                }
                AppsListBox.ItemsSource = _Records;
                EntriesPanel.ItemsSource = _Records;
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void AppsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string sellected = (AppsListBox.SelectedItem != null) ? AppsListBox.SelectedItem.ToString() : "";
            if (RecordDatePicker.SelectedDate != null)
            {
                EntriesPanel.ItemsSource = GetRecordModels(sellected, RecordDatePicker.SelectedDate);
            }
            else
            {
                EntriesPanel.ItemsSource = GetRecordModelsForName(sellected);
            }
        }

        private void RecordDatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string sellected = (AppsListBox.SelectedItem != null) ? AppsListBox.SelectedItem.ToString() : "";
            if (!String.IsNullOrEmpty(sellected))
            {
                EntriesPanel.ItemsSource = GetRecordModels(sellected,RecordDatePicker.SelectedDate);
            }
            else
            {
                EntriesPanel.ItemsSource = GetRecordModelsForDate(RecordDatePicker.SelectedDate);
            }
        }

        private IEnumerable<RecordModel> GetRecordModelsForName(string appName)
        {
            return _Records.Where(r => r.AppName.Equals(appName));
        }

        private IEnumerable<RecordModel> GetRecordModelsForDate(DateTime? startTime)
        {
            if(startTime != null)
            {
                return _Records.Where(r => r.StartTime.Date.Equals(startTime));
            }
            else
            {
                return _Records;
            }
            
        }

        private IEnumerable<RecordModel> GetRecordModels(string appName, DateTime? startTime)
        {
            if(startTime == null)
            {
                return GetRecordModelsForName(appName);
            }
            else
            {
                return _Records.Where(r => r.AppName.Equals(appName)).Where(r => r.StartTime.Date.Equals(startTime));
            }            
        }
    }
}
