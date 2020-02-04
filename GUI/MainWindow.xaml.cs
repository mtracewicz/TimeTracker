using System.Drawing;
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
            EnableStartStopButtons(true);
            ServiceStatusLabel.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#ff89b0ae");
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

            _ServiceController.Stop();
            ServiceStatusLabel.Content = "Stoped";
            ServiceStatusLabel.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#fff03e47");
            EnableStartStopButtons(false);
        }

        private void EnableStartStopButtons(bool running)
        {
            if (running)
            {
                StartButton.IsEnabled = false;
                StopButton.IsEnabled = true;
                SettingsButton.IsEnabled = false;
            }
            else
            {
                StartButton.IsEnabled = true;
                StopButton.IsEnabled = false;
                SettingsButton.IsEnabled = true;
            }
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
