using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace TimerService
{
    class Config
    {
        public static List<String> GetAppsToTrack()
        {
            return ConfigurationManager.AppSettings.Get("AppsToTrack").Split(',').ToList()??new List<string>();
        }

        public static int GetTimerInterval()
        {
            if (int.TryParse(ConfigurationManager.AppSettings.Get("TimerInterval"), out int interval))
            {
                return interval;
            }
            else
            {
                return 60000;
            }

        }

        public static String GetLogName()
        {

            return ConfigurationManager.AppSettings.Get("LogName")??"TimeTrackerLog";
        }

        public static String GetLogSourceName()
        {
            return ConfigurationManager.AppSettings.Get("LogSourceName")??"TimeTrackerLogSource";
        }
    }
}
