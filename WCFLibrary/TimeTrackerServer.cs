using System.Collections.Generic;

namespace WCFLibrary
{
    public class TimeTrackerServer : IRecords
    {
        public List<DbRecord> GetRecordsData()
        {
            List<DbRecord> output = new List<DbRecord>();
            foreach(var rec in DataBaseLibrary.DataAccess.GetRecords())
            {
                output.Add(new DbRecord(rec));
            }
            return output;
        }
    }
}
