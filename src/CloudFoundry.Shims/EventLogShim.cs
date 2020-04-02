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
