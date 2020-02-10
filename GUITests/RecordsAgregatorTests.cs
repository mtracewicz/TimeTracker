using DataBaseLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.Tests
{
    [TestClass()]
    public class RecordsAgregatorTests
    {
        private IEnumerable<RecordModel> SetUp(DateTime dateTime)
        {
            return new List<RecordModel>() 
            {
                new RecordModel("foo",dateTime),
                new RecordModel("bar",dateTime.AddDays(3)),
                new RecordModel("foobar",dateTime),
                new RecordModel("foo",dateTime.AddDays(2))
            }.AsEnumerable();
        }

        [TestMethod()]
        public void GetRecordModelsForNameTest()
        {
            var records = SetUp(DateTime.Now);
            RecordsAgregator recordsAgregator = new RecordsAgregator();
            var actual = recordsAgregator.GetRecordModelsForName("foo", records);
            Assert.AreEqual(2,actual.Count());
            Assert.AreEqual(records.ToArray()[0].AppName, actual.ToArray()[0].AppName);
            Assert.AreEqual(records.ToArray()[3].AppName, actual.ToArray()[1].AppName);
        }

        [TestMethod()]
        public void GetRecordModelsForDateTest()
        {
            DateTime dateTime = DateTime.Now; 
            var records = SetUp(dateTime);
            RecordsAgregator recordsAgregator = new RecordsAgregator();
            var actual = recordsAgregator.GetRecordModelsForDate(dateTime.AddDays(3), records);
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(records.ToArray()[1].StartTime, actual.ToArray()[0].StartTime);
        }

        [TestMethod()]
        public void GetRecordModelsTest()
        {
            DateTime dateTime = DateTime.Now;
            var records = SetUp(dateTime);
            RecordsAgregator recordsAgregator = new RecordsAgregator();
            var actual = recordsAgregator.GetRecordModels("foo",dateTime, records);
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(records.ToArray()[0].AppName, actual.ToArray()[0].AppName);
            Assert.AreEqual(records.ToArray()[0].StartTime, actual.ToArray()[0].StartTime);
        }

        [TestMethod()]
        public void GetRecordModelsZeroRecordsTest()
        {
            DateTime dateTime = DateTime.Now;
            var records = SetUp(dateTime);
            RecordsAgregator recordsAgregator = new RecordsAgregator();
            var actual = recordsAgregator.GetRecordModels("hefalump", dateTime, records);
            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod()]
        public void CreateReportTest()
        {
            //Arrange
            IEnumerable<ReportEntryModel> reportEntrys = new List<ReportEntryModel>()
            {
                new ReportEntryModel(){Name="a",TotalTime=new TimeSpan(0,0,2) },
                new ReportEntryModel(){Name="b",TotalTime=new TimeSpan(0,1,0) },
            }.AsEnumerable();
            DateTime dateTime = DateTime.Now;
            IEnumerable<RecordModel> recordModels = new List<RecordModel>()
            {
                new RecordModel("a",dateTime,dateTime.AddSeconds(1)),
                new RecordModel("a",dateTime.AddSeconds(1),dateTime.AddSeconds(2)),
                new RecordModel("b",dateTime,dateTime.AddSeconds(30)),
                new RecordModel("b",dateTime.AddSeconds(30),dateTime.AddSeconds(61)),
            }.AsEnumerable();
            //Act
            RecordsAgregator recordsAgregator = new RecordsAgregator();
            var acctual = recordsAgregator.CreateReport("a", dateTime, recordModels);
            //Assert
            Assert.AreEqual(1, acctual.Count());
            Assert.AreEqual(reportEntrys.ToArray()[0].Name, acctual.ToArray()[0].Name);
            Assert.AreEqual(reportEntrys.ToArray()[0].TotalTime, acctual.ToArray()[0].TotalTime);
        }

        [TestMethod()]
        public void CreateReportV2Test()
        {
            //Arrange
            IEnumerable<ReportEntryModel> reportEntrys = new List<ReportEntryModel>()
            {
                new ReportEntryModel(){Name="a",TotalTime=new TimeSpan(0,0,2) },
                new ReportEntryModel(){Name="b",TotalTime=new TimeSpan(0,1,0) },
            }.AsEnumerable();
            DateTime dateTime = DateTime.Now;
            IEnumerable<RecordModel> recordModels = new List<RecordModel>()
            {
                new RecordModel("a",dateTime,dateTime.AddSeconds(1)),
                new RecordModel("a",dateTime.AddSeconds(1),dateTime.AddSeconds(2)),
                new RecordModel("b",dateTime,dateTime.AddSeconds(30)),
                new RecordModel("b",dateTime.AddSeconds(30),dateTime.AddSeconds(60)),
            }.AsEnumerable();
            //Act
            RecordsAgregator recordsAgregator = new RecordsAgregator();
            var acctual = recordsAgregator.CreateReport("b", dateTime, recordModels);
            //Assert
            Assert.AreEqual(1, acctual.Count());
            Assert.AreEqual(reportEntrys.ToArray()[1].Name, acctual.ToArray()[0].Name);
            Assert.AreEqual(reportEntrys.ToArray()[1].TotalTime, acctual.ToArray()[0].TotalTime);
        }

        [TestMethod()]
        public void CreateReportNoRecordsTest()
        {
            //Arrange
            DateTime dateTime = DateTime.Now;
            IEnumerable<RecordModel> recordModels = new List<RecordModel>()
            {
                new RecordModel("a",dateTime,dateTime.AddSeconds(1)),
                new RecordModel("a",dateTime.AddSeconds(1),dateTime.AddSeconds(2)),
                new RecordModel("b",dateTime,dateTime.AddSeconds(30)),
                new RecordModel("b",dateTime.AddSeconds(30),dateTime.AddSeconds(60)),
            }.AsEnumerable();
            //Act
            RecordsAgregator recordsAgregator = new RecordsAgregator();
            var acctual = recordsAgregator.CreateReport("c", dateTime, recordModels);
            //Assert
            Assert.AreEqual(0, acctual.Count());
        }
    }
}