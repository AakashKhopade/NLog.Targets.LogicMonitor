# NLog.Targets.LogicMonitor

A NLog Target that send events and logs directly to LogicMonitor platform.
This Package leverages LogicMonitor.DataSDK to send logs.

Export the following environment variable.

 System property   |      Environment variable      |  Description |
|----------|-------------|:------|
| `Configration.company` |  `LM_COMPANY` |  Account name (Company Name) is your organization name |
| `Configration.AccessID` |  `LM_ACCESS_ID` |  Access id while using LMv1 authentication. (Not needed while using Bearer API )  |
| `Configration.AccessKey` |  `LM_ACCESS_KEY` |    Access key while using LMv1 authentication. (Not needed while using Bearer API ) |

```csharp
        var logger = NLog.LogManager.GetCurrentClassLogger();
        var config = new NLog.Config.LoggingConfiguration();

        // Targets where to log to: Console
        var logMonitor = new NLog.Targets.LogicMonitor.LogicMonitorTarget();

        // Rules for mapping loggers to targets
        config.AddRule(LogLevel.Info, LogLevel.Error, logMonitor);

        // Apply config
        NLog.LogManager.Configuration = config;
```

or 

:point_right: Learn more about [NLog Config](https://github.com/nlog/nlog/wiki/Configuration-file) 

```csharp
//Note.: Add a NLog.Config.xml to your project.

var logger = NLog.LogManager.Setup().LoadConfigurationFromXml("Nlog.Config").GetCurrentClassLogger();

```

