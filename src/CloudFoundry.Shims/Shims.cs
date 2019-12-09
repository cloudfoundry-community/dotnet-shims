using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudFoundry.Shims
{
    public static class Shims
    {
        public static void Install()
        {
            ToggleInstalled(false);
        }

        public static void Uninstall()
        {
            ToggleInstalled(true);
        }

        static void ToggleInstalled(bool installed = false)
        {
            foreach (var imp in GetImplementations())
            {
                try
                {
                    ToggleInstalled(imp, installed);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error installing shim: {ex}");
                }
            }
        }

        static void ToggleInstalled(Type shimType, bool installed)
        {
            var instance = Activator.CreateInstance(shimType) as IShim;
            if (installed)
            {
                instance.Uninstall();
                Console.WriteLine($"{shimType.FullName} Shim Unloaded");
            }
            else
            {
                instance.Install();
                Console.WriteLine($"{shimType.FullName} Shim Installed");
            }
        }

        static IEnumerable<Type> GetImplementations()
        {
            return typeof(Shims).Assembly.ExportedTypes.Where(t => !t.IsAbstract && t.GetInterfaces().Any(i => i.Name == nameof(IShim)));
        }
    }
}
