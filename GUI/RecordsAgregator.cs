using DataBaseLibrary;
using ReportsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI
{
    class RecordsAgregator
    {
        public IEnumerable<RecordModel> GetRecordModelsForName(string appName, IEnumerable<RecordModel> records)
        {
            return records.Where(r => r.AppName.Equals(appName));
        }

        public IEnumerable<RecordModel> GetRecordModelsForDate(DateTime? startTime, IEnumerable<RecordModel> records)
        {
            if (startTime != null)
            {
                return records.Where(r => r.StartTime.Date.Equals(startTime));
            }
            else
            {
                return records;
            }

        }

        public IEnumerable<RecordModel> GetRecordModels(string appName, DateTime? startTime, IEnumerable<RecordModel> records)
        {
            if (startTime == null)
            {
                return GetRecordModelsForName(appName, records);
            }
            else
            {
                return records.Where(r => r.AppName.Equals(appName)).Where(r => r.StartTime.Date.Equals(startTime));
            }
        }

        public IEnumerable<ReportEntryModel> CreateReport(string appName, DateTime startTime, IEnumerable<RecordModel> records)
        {
            return records.Where(r => r.AppName.Equals(appName))
                .Where(r => r.StartTime.Date.Equals(startTime.Date))
                .GroupBy(r => r.AppName)
                .Select(r => new ReportEntryModel { Name = r.Key, TotalTime = new TimeSpan(r.Sum(rec => rec.RunTime.Value.Ticks)) });
        }
    }
}
