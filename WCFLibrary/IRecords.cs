using DataBaseLibrary;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WCFLibrary
{
    [ServiceContract]
    public interface IRecords
    {
        [OperationContract]
        List<DbRecord>GetRecordsData();

    }

    [DataContract]
    public class DbRecord
    {

        public DbRecord(RecordModel record)
        {
            Id = record.Id;
            AppName = record.AppName;
            StartTime = record.StartTime;
            EndTime = record.EndTime;
        }

        public DbRecord(string app, DateTime start, DateTime? end = null, int? id = null)
        {
            Id = id;
            AppName = app;
            StartTime = start;
            EndTime = end;
        }

        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public string AppName { get; set; }
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public DateTime? EndTime { get; set; }        
    }
}
