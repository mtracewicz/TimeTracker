using DataBaseLibrary;
using Microsoft.Win32;
using ReportsLibrary;
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
        private readonly RecordsAgregator _RecordsAgregator;

        public MainWindow()
        {
            InitializeComponent();
            _RecordsAgregator = new RecordsAgregator();
            SetUpService();
            LoadData();
        }

        private void SetUpService()
        {
            _ServiceController = new ServiceController("TimeTrackerService");
            if (!(_ServiceController.Status == ServiceControllerStatus.Running || _ServiceController.Status == ServiceControllerStatus.StartPending))
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
                AppsListBox.ItemsSource = _Records.Select(r=>r.AppName).Distinct();
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
                EntriesPanel.ItemsSource = _RecordsAgregator.GetRecordModels(sellected, RecordDatePicker.SelectedDate, _Records);
            }
            else
            {
                EntriesPanel.ItemsSource = _RecordsAgregator.GetRecordModelsForName(sellected, _Records);
            }
        }

        private void RecordDatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string sellected = (AppsListBox.SelectedItem != null) ? AppsListBox.SelectedItem.ToString() : "";
            if (!String.IsNullOrEmpty(sellected))
            {
                EntriesPanel.ItemsSource = _RecordsAgregator.GetRecordModels(sellected, RecordDatePicker.SelectedDate, _Records);
            }
            else
            {
                EntriesPanel.ItemsSource = _RecordsAgregator.GetRecordModelsForDate(RecordDatePicker.SelectedDate, _Records);
            }
        }


        private async void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "csv files (*.csv)|*.csv",
                FilterIndex = 2
            };
            saveFileDialog.ShowDialog();
            if (!String.IsNullOrEmpty(saveFileDialog.FileName))
            {
                try
                {
                    IEnumerable<ReportEntryModel> reportEntries;
                    if (AppsListBox.SelectedItem != null)
                    {
                        reportEntries = _RecordsAgregator.CreateReport(AppsListBox.SelectedItem.ToString(), RecordDatePicker.SelectedDate ?? DateTime.Now, _Records);
                    }
                    else if (AppsListBox.Items.Count > 0)
                    {
                        reportEntries = _RecordsAgregator.CreateReport(AppsListBox.Items[0].ToString(), RecordDatePicker.SelectedDate ?? DateTime.Now, _Records);
                    }
                    else
                    {
                        MessageBox.Show($"Error while saving file!\nNo records", "Report generation", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    await ReportGenerator.WriteReportToFileAsync(saveFileDialog.FileName, reportEntries);
                    MessageBox.Show("File saved!", "Report generation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while saving file!\n{ex.Message}", "Report generation", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
