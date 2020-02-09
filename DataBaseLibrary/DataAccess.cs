using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataBaseLibrary
{
    public class DataAccess
    {
        public static IEnumerable<RecordModel> GetRecords()
        {
            TimeTrackerDataContext dc = new TimeTrackerDataContext();

            var data = from r in dc.Records select new RecordModel(r.AppName, r.StartTime, r.EndTime, r.ID);
            return data.AsEnumerable();

        }

        public static void AddRecord(ref RecordModel record)
        {
            TimeTrackerDataContext dc = new TimeTrackerDataContext();
            Record dbRecord = new Record
            {
                AppName = record.AppName,
                StartTime = record.StartTime,
                EndTime = record.EndTime
            };
            int lastId = (from r in dc.Records orderby r.ID descending select r.ID).SingleOrDefault();
            dbRecord.ID = lastId + 1;
            dc.Records.InsertOnSubmit(dbRecord);
            dc.SubmitChanges();
        }

        public static void UpdateRecord(RecordModel record)
        {
            TimeTrackerDataContext dc = new TimeTrackerDataContext();
            var dane = from r in dc.Records
                       where (r.ID == record.Id)
                       select r;
            var dbRecord = dane.First();
            dbRecord.EndTime = record.EndTime;
            dc.SubmitChanges();
        }
    }
}
