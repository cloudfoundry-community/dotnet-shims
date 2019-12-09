### Contributors 
This shim and the explanation was created by Brian Friedman, [@brianuol](https://github.com/brianuol)

## Overview
Sometimes in carpentry, the space where two pieces of wood meet does not result in a perfect angle or a level surface.  Even if our cut was perfect, the surface to which we attach may not conform to our expectations due to changes introduced by its environment.  In these cases, we use shims (thin slices of wood) to make minor adjustments until the desired fit is achieved. This analogy translates well in the world of coding; sometimes despite our best intentions, the contours of our environment in conjunction with the limitations of our underlying runtime prevent us from doing "the same thing" in the Cloud as we do when we run locally.

#### Why would I use Shims?

Shims enable code-free solutions for legacy apps that need to move to the cloud quickly. The shim provided is well tested and minimally invasive. Creating your own shims should be done with caution; with great power comes great responsibility. It is possible to break things, all kinds of things, by using a Transpiler on System libs. If used sparingly and to solve for economies of scale, shims may be a great approach for your legacy app's move to the cloud.

### EventLog Redirection Shim

Many legacy apps make use of the Windows Event Log for persisting errors and warnings raised at runtime.  Until recently, the common wisdom for running code that references the Event Log on Cloud Foundry has been, "don't do it".  In other words, refactor all of your calls to EventLog to something more cloudy, like ILogger.Log, or Console.WriteLine.  Now we have a third option:  just use a shim.

By installing this NuGet package to an ASP Framework 4.x app, all calls to the `System.Diagnostics.EventLog.WriteEvent` and `System.Diagnostics.EventLog.WriteEntry` static methods will write to console.  As with all shims, this behavior will extend only to your app domain.  If you run multiple app domains, or are building a console app, be sure to call `CloudFoundry.Shims.EventLogShim.Initialize()` in each domain at startup.

By using Shims, we make the minor adjustments required to make our approach to logging meet 12-factor at a level surface, so to speak, without changing our code or our environment. This shim leverages the <a target="tab" href="https://github.com/pardeike/Harmony/wiki">Lib.Harmony</a> NuGet package.
