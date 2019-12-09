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
