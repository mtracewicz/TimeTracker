using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace GUI
{
    class Config
    {
        private readonly Configuration configuration;

        public Config(String configFilePath)
        {
            configuration = ConfigurationManager.OpenExeConfiguration(configFilePath);
        }

        public IEnumerable<string> GetAppsToTrack()
        {
            return configuration.AppSettings.Settings["AppsToTrack"].Value.Split(',');
        }

        public void SaveAppsToTrack(IEnumerable<String> appsToTrack)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var app in appsToTrack)
            {
                if (String.IsNullOrEmpty(app))
                {
                    builder.Append($"{app},");
                }                
            }
            configuration.AppSettings.Settings["AppsToTrack"].Value = builder.ToString();
            configuration.Save();
        }

        public string GetTimerInterval()
        {
            string value = configuration.AppSettings.Settings["TimerInterval"].Value??"60000";            
            return (int.Parse(value) / 1000.0).ToString();
        }

        public void SaveTimerInterval(string time)
        {
            configuration.AppSettings.Settings["TimerInterval"].Value = Utils.ParseTime(time);
            configuration.Save();
        }
    }
}
