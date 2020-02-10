using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace TimerService.Tests
{
    [TestClass()]
    public class ProcessInformationsTests
    {
        [TestMethod()]
        public void CheckIfAppProccessIsRunningTest()
        {
            Process proccess = new Process
            {
                StartInfo = new ProcessStartInfo("explorer.exe")
            };
            proccess.Start();
            ProcessInformations info = new ProcessInformations(new List<string>());
            Assert.AreEqual(true, info.CheckIfAppProccessIsRunning("explorer.exe"));
        }

        [TestMethod()]
        public void CheckIfAppProccessIsRunningFailedTest()
        {
            ProcessInformations info = new ProcessInformations(new List<string>());
            Assert.AreEqual(false, info.CheckIfAppProccessIsRunning("hefalump45366sadvjhhgckxz.exe"));
        }
    }
}