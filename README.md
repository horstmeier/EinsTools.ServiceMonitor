# ServiceMonitor

## Description

This is a simple service monitor that starts a set of services and monitors them. 
If a service fails, it will restart it.

For Windows it can be used as a Windows Service.

The configuration of the services is done in the `appsettings.json` file.

## Configuration

The configuration is done in the `appsettings.json` file. Changes to the configuration file are applied (almost) 
immediately.

If a service is no longer included in the configuration file or is disabled while running, it will be stopped. If the
definition for a service is changed, the service will be stopped and started again with the new configuration.

You can configure the services to be monitored in the `Services` section, which looks like this:

```json
"Services": {
  "Service1": {
    "FileName": "my-service",
    "Arguments": [
      "-a",
      "-b"
    ],
    "WorkingDirectory": "/tmp",
    "Environment": {
      "LANG": "C"
    },
    "CreateNoWindow": true,
    "Logfile": {
      "FileName": "ls.log",
      "MaxSizeInMB": 10,
      "RetainedFileCountLimit": 5
    },
    "RetryPolicy": {
      "MaxRetryCount": -1,
      "DelayInSeconds": 5
    },
    "Enabled": true
  },
  "Service2": {
    ...
  }
}
```

## Fields

- `FileName`: The name of the executable file to be started.
- `Arguments`: The arguments to be passed to the executable file. This value is optional.
- `WorkingDirectory`: The working directory for the executable file. This value is optional.
- `Environment`: The environment variables to be set for the executable file. This value is optional.
- `CreateNoWindow`: If true, the window of the executable file will not be displayed. This value is optional. The default value is `true`.
- `Logfile`: The configuration for the log file. This value is optional.
  - `FileName`: The name of the log file.
  - `MaxSizeInMB`: The maximum size of the log file in megabytes. This value is optional. The default value is `10`.
  - `RetainedFileCountLimit`: The maximum number of log files to be retained. This value is optional. The default value is `5`.
- `RetryPolicy`: The retry policy for the service. This value is optional.
  - `MaxRetryCount`: The maximum number of retries. If the value is `-1`, the service will be restarted indefinitely. This value is optional. The default value is `-1`.
  - `DelayInSeconds`: The delay in seconds between retries. This value is optional. The default value is `5`.
- `Enabled`: If true, the service will be started. This value is optional. The default value is `true`. 

To disable a service temporarily, you can also add a '-' in front of the service name, like this:

```json
"Services": {
  "-Service1": {
    ...
  }
}
```

You can define a "global" retry policy for all services in the `RetryPolicy` section aside of the `Services` section, 
which looks like this:

```json
"RetryPolicy": {
  "MaxRetryCount": -1,
  "DelayInSeconds": 5
}
```

This one will be used as a default value for all services, that don't have a retry policy defined.

## Installation

To install the ServiceMonitor as a Windows Service, you can use the `sc` command:

```shell
sc create ServiceMonitor binPath= "C:\path\to\ServiceMonitor.exe"
```

