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
