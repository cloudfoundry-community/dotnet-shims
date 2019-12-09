### Contributors 
This shim and the majority of this explanation was created by Brian Friedman, [@brianuol](https://github.com/brianuol)

## Overview
Sometimes in carpentry, the space where two pieces of wood meet does not result in a perfect angle or a level surface.  Even if our cut was perfect, the surface to which we attach may not conform to our expectations due to changes introduced by its environment.  In these cases, we use shims (thin slices of wood) to make minor adjustments until the desired fit is achieved. This analogy translates well in the world of coding; despite our best intentions, the contours of our environment in conjunction with the limitations of our underlying runtime prevent us from doing "the same thing" in the Cloud as we do when we run locally. Sometimes there are only minor modifications required to get existing applications running on the cloud. 

An important aspect of a <a href="http://12factor.net">Cloud Native</a> application is to treat logs as event streams. By using this shim, we make the minor adjustments required to make our approach to logging meet 12-factor at a level surface, so to speak, without changing our code or our environment.

### EventLog Redirection Shim
 Many .NET legacy apps make use of the Windows Event Log for persisting errors and warnings raised at runtime.  Until recently, the common wisdom for running code that references the Event Log on Cloud Foundry has been, "don't do it".  In other words, refactor all of your calls to EventLog to something more cloudy, like ILogger.Log, or Console.WriteLine.  Now we have a third option:  just use a shim.

By installing this NuGet package to an ASP Framework 4.x app, all calls to the `System.Diagnostics.EventLog.WriteEvent` and `System.Diagnostics.EventLog.WriteEntry` static methods will instead write to console.  As with all shims, this behavior will extend only to your app domain. 

This shim leverages the <a target="tab" href="https://github.com/pardeike/Harmony/wiki">Lib.Harmony</a> NuGet package. 

#### Why would I use Shims?

Shims enable code-free solutions for legacy apps that need to move to the cloud quickly. The shim provided is well-tested and minimally invasive. Creating your own shims should be done with caution; with great power comes great responsibility. If used sparingly and to solve for economies of scale, shims may be a great approach for your legacy app's move to the cloud.
