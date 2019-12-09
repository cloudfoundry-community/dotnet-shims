using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudFoundry.Shims
{
    public interface IShim
    {
        bool IsInstalled { get; }
        int InvocationCheck { get; set; }
        bool IsSupported { get; }
        void Install();
        void Uninstall();
    }
}
