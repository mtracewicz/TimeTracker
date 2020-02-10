using System;
using System.ServiceModel;
using System.ServiceProcess;
using System.Timers;

namespace TimerService
{
    public partial class TimeTrackingService : ServiceBase
    {

        private Loger _Loger;
        private ProcessInformations _ProcInfo;
        public ServiceHost serviceHost = null;

        public TimeTrackingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _ProcInfo = new ProcessInformations(Config.GetAppsToTrack());
            _Loger = new Loger();
            _Loger.LogWhatsTracked(Config.GetAppsToTrack());
            this.SetUpTimer();
            this.SetUpServiceHost();
        }

        protected override void OnStop()
        {
            _Loger.LogInforamtion($"Tracking ended at {DateTime.Now}");
            this.CloseServiceHost();
        }

        private void SetUpTimer()
        {
            Timer timer = new Timer(Config.GetTimerInterval());
            timer.Elapsed += new ElapsedEventHandler((sender, e) =>
            {
                try
                {
                    _ProcInfo.MonitorProcessesStatus();
                }
                catch (Exception ex)
                {
                    _Loger.LogException(ex);
                }

            });
            timer.AutoReset = true;
            timer.Start();
        }

        private void SetUpServiceHost()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }
            serviceHost = new ServiceHost(typeof(WCFLibrary.TimeTrackerServer));
            serviceHost.Open();
        }

        private void CloseServiceHost()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
}
