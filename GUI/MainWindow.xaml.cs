using System;
using System.ServiceProcess;
using System.Windows;
using System.Windows.Media;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ServiceController _ServiceController;
        public MainWindow()
        {
            InitializeComponent();
            _ServiceController = new ServiceController("TimeTrackerService");
            ServiceStatusLabel.Content = _ServiceController.Status.ToString();
            EnableStartStopButtons(_ServiceController.Status == ServiceControllerStatus.Running);
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
    }
}
