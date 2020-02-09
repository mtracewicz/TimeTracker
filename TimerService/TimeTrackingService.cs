using DataBaseLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace TimerService
{
    public partial class TimeTrackingService : ServiceBase
    {

        private EventLog _EventLog;
        private List<String> _AppsToTrack;
        private List<RecordModel> _CurrentlyTracked;

        public TimeTrackingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _AppsToTrack = ConfigurationManager.AppSettings.Get("AppsToTrack").Split(',').ToList();
            _CurrentlyTracked = new List<RecordModel>();
            this.SetUpEventLog();
            this.LogWhatsTracked();
            this.SetUpTimer();
        }

        protected override void OnStop()
        {
            _EventLog.WriteEntry($"Tracking ended at {DateTime.Now}", EventLogEntryType.Information);
        }

        private void CheckProcessesStatus()
        {
            try
            {
                foreach (String app in _AppsToTrack)
                {
                    RecordModel record;
                    if (_CurrentlyTracked.Where(r => r.AppName.Equals(app)).Any())
                    {
                        if (!this.CheckIfAppProccessIsRunning(app))
                        {
                            record = _CurrentlyTracked.Where(r => r.AppName.Equals(app)).First();
                            record.EndTime = DateTime.Now;
                            DataAccess.UpdateRecord(record);
                            _EventLog.WriteEntry(record.ToString());
                        }

                    }
                    else
                    {
                        if (this.CheckIfAppProccessIsRunning(app))
                        {
                            var processes = Process.GetProcessesByName(app);
                            DateTime firstStartTime = DateTime.Now;
                            foreach (Process process in processes)
                            {
                                firstStartTime = (process.StartTime <= firstStartTime) ? process.StartTime : firstStartTime;
                            }
                            record = new RecordModel(app, firstStartTime);
                            DataAccess.AddRecord(ref record);
                            _CurrentlyTracked.Add(record);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _EventLog.WriteEntry($"Error message: {e.Message}- Stack trace:{e.StackTrace}", EventLogEntryType.Error);
            }
        }

        private bool CheckIfAppProccessIsRunning(String appName)
        {
            return Process.GetProcessesByName(appName).Length > 0;
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

        private void SetUpTimer()
        {
            Timer timer = new Timer(60000);
            timer.Elapsed += new ElapsedEventHandler((sender, e) =>
            {
                CheckProcessesStatus();
            });
            timer.AutoReset = false;
            timer.Start();
        }

        private void LogWhatsTracked()
        {
            StringBuilder stringBuilder = new StringBuilder($"Tracking started at {DateTime.Now}\nTracking following apps:\n");
            foreach (String app in _AppsToTrack)
            {
                if (!String.IsNullOrWhiteSpace(app))
                {
                    stringBuilder.Append($"\t* {app}\n");
                }
            }
            _EventLog.WriteEntry(stringBuilder.ToString(), EventLogEntryType.Information);
        }
    }
}
