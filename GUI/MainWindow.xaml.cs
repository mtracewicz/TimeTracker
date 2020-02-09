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
        private readonly ServiceController _ServiceController;
        private List<RecordModel> _Records;
        public MainWindow()
        {
            InitializeComponent();
            _ServiceController = new ServiceController("TimeTrackerService");
            ServiceStatusLabel.Content = _ServiceController.Status.ToString();
            EnableStartStopButtons(_ServiceController.Status == ServiceControllerStatus.Running);
            LoadData();
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
        private void LoadData()
        {
            try
            {
                _Records = DataAccess.GetRecords().ToList();
                AppsListBox.ItemsSource = _Records;
                EntriesPanel.ItemsSource = _Records;
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            EntriesPanel.ItemsSource = _Records;
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
                EntriesPanel.ItemsSource = _Records.Where(r => r.AppName.Equals(sellected))
                    .Where(r => r.StartTime.Date.Equals(RecordDatePicker.SelectedDate));
            }
            else
            {
                EntriesPanel.ItemsSource = _Records.Where(r => r.AppName.Equals(sellected));
            }
        }

        private void RecordDatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string sellected = (AppsListBox.SelectedItem != null) ? AppsListBox.SelectedItem.ToString() : "";
            if (!sellected.Equals(""))
            {
                EntriesPanel.ItemsSource = _Records.Where(r => r.AppName.Equals(sellected)).Where(r => r.StartTime.Date.Equals(RecordDatePicker.SelectedDate));
            }
            else
            {
                EntriesPanel.ItemsSource = _Records.Where(r => r.StartTime.Date.Equals(RecordDatePicker.SelectedDate));
            }

        }
    }
}
