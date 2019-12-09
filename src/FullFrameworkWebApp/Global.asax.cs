using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace FullFrameworkWebApp
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            EventLog.WriteEntry("EventLogCanary", "Hello, web!");
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            EventLog.WriteEntry("EventLogCanary", "Begin request");
        }

        void Application_EndRequest(object sender, EventArgs e)
        {
            EventLog.WriteEntry("EventLogCanary", "End request");
        }

        void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            EventLog.WriteEntry("EventLogCanary", ex.ToString(), EventLogEntryType.Error);
        }
    }
}