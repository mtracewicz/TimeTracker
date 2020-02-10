using TimerService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataBaseLibrary;

namespace TimerService.Tests
{
    //Checks are done using smss.exe which is windows sesion manager and should be present in all windows copys
    [TestClass()]
    public class ProcessInformationsTests
    {
        [TestMethod()]
        public void CheckIfAppProccessIsRunningTest()
        {
            ProcessInformations info = new ProcessInformations(new List<string>());
            Assert.AreEqual(true, info.CheckIfAppProccessIsRunning("smss.exe"));
        }

        [TestMethod()]
        public void CheckIfAppProccessIsRunningFailedTest()
        {
            ProcessInformations info = new ProcessInformations(new List<string>());
            Assert.AreEqual(false, info.CheckIfAppProccessIsRunning("hefalump45366sadvjhhgckxz.exe"));
        }

        [TestMethod()]
        public void CheckProcessStatusRunningTest()
        {
            ProcessInformations info = new ProcessInformations(new List<string>() { "smss.exe" });
            Assert.AreEqual("Running", info.CheckProcessStatus("smss.exe"));
        }

        [TestMethod()]
        public void CheckProcessStatusNotStartedTest()
        {
            ProcessInformations info = new ProcessInformations(new List<string>() { });
            Assert.AreEqual("NotStarted", info.CheckProcessStatus("hefalump45366sadvjhhgckxz.exe"));
        }

        [TestMethod()]
        public void CheckProcessStatusExitedTest()
        {
            ProcessInformations info = new ProcessInformations(new List<string>() { "hefalump45366sadvjhhgckxz.exe" });
            info.CurrentlyTracked.Add(new RecordModel("hefalump45366sadvjhhgckxz.exe", DateTime.Now));
            Assert.AreEqual("Exited", info.CheckProcessStatus("hefalump45366sadvjhhgckxz.exe"));
        }

        [TestMethod()]
        /*This test will differ base on if it's run as administrator
        If so it tests FindFirstStartedProcess execution
        Else it test if proper exception is thrown (Exception that is should be run as admin)*/
        public void FindFirstStartedProcessTest()
        {
            ProcessInformations info = new ProcessInformations(new List<string>() { });
            var proccess = Process.GetProcessesByName("smss")[0];
            try
            {
                Assert.AreEqual(proccess.StartTime, info.FindFirstStartedProcess("smss"));
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(System.ComponentModel.Win32Exception));
            }

        }

        [TestMethod()]
        /*This test will differ base on if it's run as administrator
        If so it tests FindFirstStartedProcess execution
        Else it test if proper exception is thrown (Exception that is should be run as admin)*/
        public void FindFirstStartedProcessExeTest()
        {
            ProcessInformations info = new ProcessInformations(new List<string>() { });
            var proccess = Process.GetProcessesByName("smss")[0];
            try
            {
                Assert.AreEqual(proccess.StartTime, info.FindFirstStartedProcess("smss.exe"));
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(System.ComponentModel.Win32Exception));
            }

        }

        [TestMethod()]
        /*This test will differ base on if it's run as administrator
        If so it tests FindFirstStartedProcess execution
        Else it test if proper exception is thrown (Exception that is should be run as admin)*/
        public void FindFirstStartedProcessExceptionTest()
        {
            ProcessInformations info = new ProcessInformations(new List<string>() { });
            try
            {
                Assert.ThrowsException<ArgumentException>((Action)(()=>info.FindFirstStartedProcess("hefalump45366sadvjhhgckxz.exe")));
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(System.ComponentModel.Win32Exception));
            }
        }

        [TestMethod()]
        /*This test will differ base on if it's run as administrator
        If so it tests FindFirstStartedProcess execution
        Else it test if proper exception is thrown (Exception that is should be run as admin)*/
        public void FindFirstStartedProcessFalseTest()
        {
            ProcessInformations info = new ProcessInformations(new List<string>() { });
            try
            {
                Assert.AreNotEqual(DateTime.Now, info.FindFirstStartedProcess("smss"));
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(System.ComponentModel.Win32Exception));
            }
        }
    }
}