using DataBaseLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WCFLibrary;

namespace GUI.Tests
{
    [TestClass()]
    public class UtilsTests
    {
        [TestMethod()]
        public void CheckValidFormatHtmlColorToShortTest()
        {
            Assert.AreEqual(false, Utils.CheckValidFormatHtmlColor("#fff"));
        }

        [TestMethod()]
        public void CheckValidFormatHtmlColorToLongTest()
        {
            Assert.AreEqual(false, Utils.CheckValidFormatHtmlColor("#ff003dadffe"));
        }

        [TestMethod()]
        public void CheckValidFormatHtmlColorHashTest()
        {
            Assert.AreEqual(true, Utils.CheckValidFormatHtmlColor("#ffffff"));
        }

        [TestMethod()]
        public void CheckValidFormatHtmlColorNoHashTest()
        {
            Assert.AreEqual(true, Utils.CheckValidFormatHtmlColor("FFFFFF"));
        }

        [TestMethod()]
        public void CheckValidFormatHtmlColorNoHexTest()
        {
            Assert.AreEqual(false, Utils.CheckValidFormatHtmlColor("#fifjfe"));
        }

        [TestMethod()]
        public void CheckValidFormatHtmlColorHexTest()
        {
            Assert.AreEqual(true, Utils.CheckValidFormatHtmlColor("#ff2e2e2e"));
        }

        [TestMethod()]
        public void CheckValidFormatHtmlColorTest()
        {
            Assert.AreEqual(true, Utils.CheckValidFormatHtmlColor("#2e2e2e"));
        }


        [TestMethod()]
        public void GetBrushFromHexTest()
        {
            Assert.ThrowsException<ArgumentException>((Action)(() => { Utils.GetBrushFromHex("advasdfasdf"); }));
        }

        [TestMethod()]
        public void ParseTimeWrongSeparatorTest()
        {
            Assert.AreEqual("60000", Utils.ParseTime("0.5"));
        }

        [TestMethod()]
        public void ParseTimeValidDoubleTest()
        {
            Assert.AreEqual("500", Utils.ParseTime("0,5"));
        }

        [TestMethod()]
        public void ParseTimeValidIntTest()
        {
            Assert.AreEqual("10000", Utils.ParseTime("10"));
        }

        [TestMethod()]
        public void ParseTimeNegativeTimeTest()
        {
            Assert.AreEqual("60000", Utils.ParseTime("-1"));
        }

        [TestMethod()]
        public void ConvertEndTimeNullTest()
        {
            DateTime time = DateTime.Now;
            DbRecord toConvert = new DbRecord("Test", time, null, 0);
            RecordModel expected = new RecordModel("Test", time, null, 0);
            var res = Utils.Convert(new DbRecord[] { toConvert });
            Assert.AreEqual(expected.Id, res[0].Id);
            Assert.AreEqual(expected.AppName, res[0].AppName);
            Assert.AreEqual(expected.StartTime, res[0].StartTime);
            Assert.AreEqual(expected.EndTime, res[0].EndTime);
        }

        [TestMethod()]
        public void ConvertIdNullTest()
        {
            DateTime time = DateTime.Now;
            DbRecord toConvert = new DbRecord("Test", time, time.AddDays(1));
            RecordModel expected = new RecordModel("Test", time, time.AddDays(1));
            var res = Utils.Convert(new DbRecord[] { toConvert });
            Assert.AreEqual(expected.Id, res[0].Id);
            Assert.AreEqual(expected.AppName, res[0].AppName);
            Assert.AreEqual(expected.StartTime, res[0].StartTime);
            Assert.AreEqual(expected.EndTime, res[0].EndTime);
        }

        [TestMethod()]
        public void ConvertBothNullTest()
        {
            DateTime time = DateTime.Now;
            DbRecord toConvert = new DbRecord("Test", time);
            RecordModel expected = new RecordModel("Test", time);
            var res = Utils.Convert(new DbRecord[] { toConvert });
            Assert.AreEqual(expected.Id, res[0].Id);
            Assert.AreEqual(expected.AppName, res[0].AppName);
            Assert.AreEqual(expected.StartTime, res[0].StartTime);
            Assert.AreEqual(expected.EndTime, res[0].EndTime);
        }

        [TestMethod()]
        public void ConvertNonNullTest()
        {
            DateTime time = DateTime.Now;
            DbRecord toConvert = new DbRecord("Test", time, time.AddDays(1), 1);
            RecordModel expected = new RecordModel("Test", time, time.AddDays(1), 1);
            var res = Utils.Convert(new DbRecord[] { toConvert });
            Assert.AreEqual(expected.Id, res[0].Id);
            Assert.AreEqual(expected.AppName, res[0].AppName);
            Assert.AreEqual(expected.StartTime, res[0].StartTime);
            Assert.AreEqual(expected.EndTime, res[0].EndTime);
        }
    }
}