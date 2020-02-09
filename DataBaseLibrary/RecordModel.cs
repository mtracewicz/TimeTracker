using System;

namespace DataBaseLibrary
{
    public class RecordModel
    {
        public int? Id { get; set; }
        public string AppName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? RunTime
        {
            get
            {
                if (EndTime != null)
                {
                    return EndTime - StartTime;
                }
                else
                {
                    return null;
                }
            }
        }

        public RecordModel(string app, DateTime start, DateTime? end = null, int? id = null)
        {
            Id = id;
            AppName = app;
            StartTime = start;
            EndTime = end;
        }

        public override string ToString()
        {
            return AppName;
        }
    }
}
