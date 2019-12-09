using Harmony;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloudFoundry.Shims
{
    public class EventLogShim : ShimsBase<EventLogShim>
    {
        [HarmonyPatch]
        private static class EventLogInternal_WriteEntry
        {
            private static MethodBase TargetMethod()
            {
                return AccessTools
                    .TypeByName("System.Diagnostics.EventLogInternal")
                    .GetMethod("WriteEntry", new[] { typeof(string), typeof(EventLogEntryType), typeof(int), typeof(short), typeof(byte[]) });
            }

            private static bool Prefix(object __instance, string message, EventLogEntryType type)
            {
                Instance.InvocationCheck++;

                var source = Traverse.Create(__instance).Field("sourceName").GetValue<string>();
                var entry = $"{source}: {message}";
                if (type == EventLogEntryType.Error)
                    Console.Error.WriteLine(entry);
                else
                    Console.WriteLine(entry);
                return false;
            }
        }

        [HarmonyPatch]
        private static class EventLogInternal_WriteEvent
        {
            private static MethodBase TargetMethod()
            {
                return AccessTools
                    .TypeByName("System.Diagnostics.EventLogInternal")
                    .GetMethod("WriteEvent", new[] { typeof(EventInstance), typeof(byte[]), typeof(object[]) });
            }

            private static bool Prefix(object __instance, EventInstance instance, object[] values)
            {
                Instance.InvocationCheck++;

                if (values == null)
                    return false;
                var message = values.Select(x => x.ToString() + "\n");
                var source = Traverse.Create(__instance).Field("sourceName").GetValue<string>();
                var entry = $"{source}: {message}";
                if (instance.EntryType == EventLogEntryType.Error)
                    Console.Error.WriteLine(entry);
                else
                    Console.WriteLine(entry);
                return false;
            }
        }

        [HarmonyPatch]
        private static class EventLog_SourceExists
        {
            private static MethodBase TargetMethod()
            {
                return AccessTools
                    .TypeByName("System.Diagnostics.EventLog")
                    .GetMethod("SourceExists",  new[] { typeof(string), typeof(string) });
            }

            private static bool Prefix(object __instance, ref bool __result) //, bool wantToCreate)
            {
                __result = true;
                return false;
            }
        }

        //[HarmonyPatch]
        //private static class EventLog_CreateEventSource
        //{
        //    private static MethodBase TargetMethod()
        //    {
        //        return AccessTools
        //            .TypeByName("System.Diagnostics.EventLog")
        //            .GetMethod("CreateEventSource", new[] { typeof(System.Diagnostics.EventSourceCreationData) });
        //    }

        //    private static bool Prefix() //, bool wantToCreate)
        //    {
        //        Instance.InvocationCheck++;
        //        return false;
        //    }
        //}

    }
}
