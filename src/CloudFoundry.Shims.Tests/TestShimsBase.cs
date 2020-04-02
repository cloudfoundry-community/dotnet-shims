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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudFoundry.Shims.Tests
{
    [TestClass]
    public class TestShimsBase
    {
        [TestMethod]
        public void TestShimIsSupportedByDefault()
        {
            var shim = CreateSut();
            Assert.IsTrue(shim.IsSupported);
        }

        [TestMethod]
        public void TestShimDefaultStateIsNotInstalled()
        {
            var shim = CreateSut();
            Assert.IsFalse(shim.IsInstalled);
        }

        [TestMethod]
        public void TestShimInstalled()
        {
            var shim = CreateSut();
            shim.Install();
            Assert.IsTrue(shim.IsInstalled);
        }

        [TestMethod]
        public void TestShimUninstalled()
        {
            var shim = CreateSut(flagIsInstalled: true);
            Assert.IsTrue(shim.IsInstalled);

            shim.Uninstall();
            Assert.IsFalse(shim.IsInstalled);
        }

        [TestMethod]
        public void TestNotSupportedShimIsNotInstalled()
        {
            var shim = CreateSut(flagIsSupported: false);
            Assert.IsFalse(shim.IsSupported);

            Assert.IsFalse(shim.IsInstalled);
            shim.Install();
            Assert.IsFalse(shim.IsInstalled);
        }

        [TestMethod]
        public void TestStaticInitializeAndDestroyShim()
        {
            Assert.IsFalse(TestShim.Instance.IsInstalled);
            TestShim.Initialize();
            Assert.IsTrue(TestShim.Instance.IsInstalled);
            TestShim.Destroy();
            Assert.IsFalse(TestShim.Instance.IsInstalled);
        }

        class TestShim : ShimsBase<TestShim>
        {
            private bool _isSupported;

            public TestShim()
                : base()
            {
                _isSupported = base.IsSupported;
            }

            public void SetIsSupportedFlag(bool flagValue)
            {
                _isSupported = flagValue;
            }

            public void SetIsInstalledFlag(bool flagValue)
            {
                base.IsInstalled = flagValue;
            }

            protected override bool IsSupported
            {
                get { return _isSupported; }
            }
        }

        static IShim CreateSut(bool? flagIsSupported = null, bool? flagIsInstalled = null)
        {
            var shim = new TestShim();
            if (flagIsSupported.HasValue)
            {
                shim.SetIsSupportedFlag(flagIsSupported.Value);
            }
            if (flagIsInstalled.HasValue)
            {
                shim.SetIsInstalledFlag(flagIsInstalled.Value);
            }
            return shim;
        }
    }
}
