using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudFoundry.Shims
{
    public abstract class ShimsBase<TShim> : IShim
        where TShim : class, IShim
    {
        protected HarmonyInstance _harmony = HarmonyInstance.Create(typeof(TShim).FullName);
        public int InvocationCheck { get; set; } = 0;

        /// <summary>
        /// Defaults to true unless overridden by subclass indicating conditional installation.
        /// Return false if any preconditions for patching are not met.
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsSupported { get; } = true;

        protected bool IsInstalled { get; set; }

        bool IShim.IsInstalled => this.IsInstalled;

        bool IShim.IsSupported => this.IsSupported;

        void IShim.Install()
        {
            if (this.IsSupported)
            {
                InvocationCheck = 0;
                _harmony?.PatchAll();
                this.IsInstalled = true;
            }
        }

        void IShim.Uninstall()
        {
            _harmony?.UnpatchAll();
            this.IsInstalled = false;
        }

        public static void Initialize()
        {
            if (Instance.IsSupported)
            {
                Instance.Install();
            }
        }

        public static void Destroy()
        {
            Instance.Uninstall();
        }

        public static IShim Instance { get; } = Activator.CreateInstance<TShim>();
    }
}
