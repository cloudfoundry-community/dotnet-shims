using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudFoundry.Shims.Tests
{
    [TestClass]
    public class TestEventLogShim
    {
        [TestMethod]
        public void TestShim()
        {
            EventLogShim.Initialize();
            System.Diagnostics.EventLog.WriteEntry(nameof(TestEventLogShim), "Hello, world!");
            Assert.AreEqual(1, EventLogShim.Instance.InvocationCheck);
        }

        [TestMethod]
        public void TestLogEntryFormat()
        {
            var source = nameof(TestEventLogShim);
            var msg = "Hello, world!";
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));
            EventLogShim.Initialize();
            System.Diagnostics.EventLog.WriteEntry(source, msg);
            Assert.AreEqual(1, EventLogShim.Instance.InvocationCheck);
            Assert.AreEqual($"{source}: {msg}\r\n", sb.ToString());
        }

        [TestMethod]
        public void TestErrorLogEntryWritesToErr()
        {
            var source = nameof(TestEventLogShim);
            var msg = "Hello, world!";
            var sb = new StringBuilder();
            Console.SetError(new StringWriter(sb));
            EventLogShim.Initialize();
            System.Diagnostics.EventLog.WriteEntry(source, msg, System.Diagnostics.EventLogEntryType.Error);
            Assert.AreEqual(1, EventLogShim.Instance.InvocationCheck);
            Assert.AreEqual($"{source}: {msg}\r\n", sb.ToString());
        }

        [TestMethod]
        public void TestErrorLogExistingSourceExistsReturnsTrue()
        {
            EventLogShim.Initialize();
            bool result = System.Diagnostics.EventLog.SourceExists("MsiInstaller", ".");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestErrorLogNonExistingSourceExistsReturnsTrue()
        {
            EventLogShim.Initialize();
            bool result = System.Diagnostics.EventLog.SourceExists("80EF6F32-0316-484D-B179-BBE0425CB854", ".");
            Assert.IsTrue(result);
        }

        //[TestMethod]
        //public void TestErrorCreateEventSource()
        //{
        //    EventLogShim.Initialize();
        //    System.Diagnostics.EventLog.CreateEventSource(new System.Diagnostics.EventSourceCreationData("",""));
        //    Assert.AreEqual(1, EventLogShim.Instance.InvocationCheck);
        //}

        //[TestMethod]
        //public void TestErrorCreateEventSourceTwoStringParameters()
        //{
        //    EventLogShim.Initialize();
        //    System.Diagnostics.EventLog.CreateEventSource("", "");
        //    Assert.AreEqual(1, EventLogShim.Instance.InvocationCheck);
        //}

        //[TestMethod]
        //public void TestErrorCreateEventSourceThreeStringParameters()
        //{
        //    EventLogShim.Initialize();
        //    System.Diagnostics.EventLog.CreateEventSource("", "", "");
        //    Assert.AreEqual(1, EventLogShim.Instance.InvocationCheck);
        //}


    }
}
