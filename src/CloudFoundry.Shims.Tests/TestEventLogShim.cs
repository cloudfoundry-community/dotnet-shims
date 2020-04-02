/*
Copyright (c) 2019-2020 VMware, Inc. All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
POSSIBILITY OF SUCH DAMAGE.
*/

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
