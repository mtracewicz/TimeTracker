using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace TimerService
{
    public partial class TimeTrackingService : ServiceBase
    {

        private EventLog _EventLog;
        private List<String> _AppsToTrack;
        public TimeTrackingService()
        {
            InitializeComponent();
            this.ServiceName = "TimeTrackerService";
        }

        protected override void OnStart(string[] args)
        {
            this.SetUpEventLog();
            _AppsToTrack = ConfigurationManager.AppSettings.Get("AppsToTrack").Split(',').ToList();
            try
            {
                StringBuilder stringBuilder = new StringBuilder($"Tracking started at {DateTime.Now}\nTracking following apps:\n");
                foreach(String app in _AppsToTrack)
                {
                    if (!String.IsNullOrWhiteSpace(app))
                    {
                        stringBuilder.Append($"\t* {app}\n");
                    }                    
                }                
                _EventLog.WriteEntry(stringBuilder.ToString(), EventLogEntryType.Information);
                foreach (String app in _AppsToTrack)
                {
                    var processes = Process.GetProcessesByName(app);
                    DateTime? firstStartTime = null;
                    foreach (Process process in processes)
                    {
                        if (firstStartTime == null)
                        {
                            firstStartTime = process.StartTime;
                        }
                        else
                        {
                            firstStartTime = (process.StartTime <= firstStartTime) ? process.StartTime : firstStartTime;
                        }
                        this.AddLogingOnProcessExit(process);
                    }
                    if (firstStartTime != null)
                    {
                        _EventLog.WriteEntry($"Process:{app} started at {firstStartTime}", EventLogEntryType.Information);
                    }
                }
            }
            catch (Exception e)
            {
                _EventLog.WriteEntry($"Error message: {e.Message}", EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            _EventLog.WriteEntry($"Tracking ended at {DateTime.Now}");
        }

        private void SetUpEventLog()
        {
            String sourceName = ConfigurationManager.AppSettings.Get("LogSourceName");
            String eventLogName = ConfigurationManager.AppSettings.Get("LogName");
            if (!EventLog.SourceExists(sourceName, "."))
            {
                EventLog.CreateEventSource(sourceName, eventLogName);
            }
            _EventLog = new EventLog(eventLogName, ".", sourceName);
        }

        private void AddLogingOnProcessExit(Process process)
        {
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler((object sender, EventArgs e) =>
            {
                if (Process.GetProcessesByName((sender as Process).ProcessName).Length == 0)
                {
                    _EventLog.WriteEntry($"Process:{process.ProcessName} ended at {process.ExitTime}", EventLogEntryType.Information);
                }
            });
        }
    }
}
