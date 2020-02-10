using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TimerService
{
    class Loger
    {
        private EventLog _EventLog;

        public Loger()
        {
            this.SetUpEventLog();
        }

        public void LogWhatsTracked(List<String> appsToTrack)
        {
            StringBuilder stringBuilder = new StringBuilder($"Tracking started at {DateTime.Now}\nTracking following apps:\n");
            foreach (String app in appsToTrack)
            {
                if (!String.IsNullOrWhiteSpace(app))
                {
                    stringBuilder.Append($"\t* {app}\n");
                }
            }
            _EventLog.WriteEntry(stringBuilder.ToString(), EventLogEntryType.Information);
        }

        public void LogException(Exception e)
        {
            _EventLog.WriteEntry($"Error message: {e.Message}\nStack trace:{e.StackTrace}", EventLogEntryType.Error);
        }

        public void LogInforamtion(String info)
        {
            _EventLog.WriteEntry(info, EventLogEntryType.Information );
        }

        private void SetUpEventLog()
        {
            String sourceName = Config.GetLogSourceName();
            String eventLogName = Config.GetLogName();
            if (!EventLog.SourceExists(sourceName, "."))
            {
                EventLog.CreateEventSource(sourceName, eventLogName);
            }
            _EventLog = new EventLog(eventLogName, ".", sourceName);
        }
    }
}
