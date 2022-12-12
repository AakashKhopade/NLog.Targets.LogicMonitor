using System;
namespace NLog.Targets.LogicMonitor
{
    public class Constants
    {
        public const string LogLevel = "LogLevel";
        public struct TracesKey
        {
            public const string TraceId = "TraceId";
            public const string SpanId = "SpanId";
            public const string OperationName = "OperationName";
        };
    }
}

