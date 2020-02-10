using DataBaseLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TimerService
{
    public class ProcessInformations
    {

        private readonly List<String> _AppsToTrack;
        public List<RecordModel> CurrentlyTracked { get; set; }

        public ProcessInformations(IEnumerable<String> appToTrack)
        {
            _AppsToTrack = appToTrack.ToList();
            CurrentlyTracked = new List<RecordModel>();
        }

        public bool CheckIfAppProccessIsRunning(String appName)
        {
            return (Process.GetProcessesByName(appName).Length > 0 || Process.GetProcessesByName(Path.GetFileNameWithoutExtension(appName)).Length > 0);
        }

        public DateTime FindFirstStartedProcess(String appName)
        {
            if (!CheckIfAppProccessIsRunning(appName))
            {
                throw new ArgumentException($"There is no process for {appName}");
            }
            
            var processes = Process.GetProcessesByName(appName);
            if(!(processes.Length > 0))
            {
                processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(appName));
            }

            DateTime firstStartTime = DateTime.Now;
            foreach (Process process in processes)
            {
                firstStartTime = (process.StartTime <= firstStartTime) ? process.StartTime : firstStartTime;
            }
            return firstStartTime;
        }

        public void MonitorProcessesStatus()
        {
            foreach (String app in _AppsToTrack)
            {
                switch (CheckProcessStatus(app))
                {
                    case "Exited":
                        UpdateTrackedProcess(GetRecordModelsForApp(app).First(), DateTime.Now);
                        break;
                    case "Running":
                        this.AddTrackedProcess(app, FindFirstStartedProcess(app));
                        break;
                    default:
                        break;
                }
            }
        }

        public string CheckProcessStatus(String app)
        {
            IEnumerable<RecordModel> trackedApp = GetRecordModelsForApp(app);
            if (trackedApp.Any())
            {
                if (!CheckIfAppProccessIsRunning(app))
                {
                    return "Exited";
                }
            }
            else
            {
                if (CheckIfAppProccessIsRunning(app))
                {
                    return "Running";
                }
            }
            return "NotStarted";
        }

        private IEnumerable<RecordModel> GetRecordModelsForApp(string app)
        {
            return CurrentlyTracked.Where(r => r.AppName.Equals(app));
        }

        private void AddTrackedProcess(string appName, DateTime startTime)
        {
            RecordModel record = new RecordModel(appName, startTime);
            DataAccess.AddRecord(ref record);
            CurrentlyTracked.Add(record);
        }

        private void UpdateTrackedProcess(RecordModel record, DateTime endTime)
        {
            record.EndTime = endTime;
            DataAccess.UpdateRecord(record);
            CurrentlyTracked.Remove(record);
        }
    }
}
