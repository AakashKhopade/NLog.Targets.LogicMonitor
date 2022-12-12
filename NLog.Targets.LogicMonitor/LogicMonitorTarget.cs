using LogicMonitor.DataSDK.Api;
using LogicMonitor.DataSDK.Model;
using LogicMonitor.DataSDK;
using System.Diagnostics;
using NLog.Layouts;
namespace NLog.Targets.LogicMonitor
{

    [Target("LogicMonitor")]
    public class LogicMonitorTarget : TargetWithContext
    {
        public Layout Interval { get; set; }
        public Layout Batch { get; set; }
        public Layout Company { get; set; }
        public Layout LMAccessKey { get; set; }
        public Layout LMAccessID { get; set; }


        private static Configuration _config;
        private static ApiClient _apiClient;
        private static Logs _logs;
        private Resource _resource;
        private static int _interval { get; set; }
        private static bool _batch { get; set; }

        public LogicMonitorTarget() :base()
        {
            _interval = 10;
            _batch = true;
        }

        public LogicMonitorTarget(Configuration configuration, Resource resource = null, int interval = 10, bool batch=true) :base()
        {
            _config = configuration ??= new Configuration();
            _interval = interval;
            _batch = batch;
        }
        protected override void InitializeTarget()
        {
            base.InitializeTarget();
            // TODO Custom Init Logic
            if (_config == null)
            {
                string company = Company.ToString();
                string lmAccessID = LMAccessID.ToString();
                string lmAccessKey = LMAccessKey.ToString();
                _batch = Convert.ToBoolean(Batch.ToString());
                _interval = Convert.ToInt32(Interval.ToString());
                _config = new Configuration(company: company, accessID: lmAccessID, accessKey: lmAccessKey);
            }
            _apiClient = new ApiClient(configuration: _config ??= new Configuration());
            _logs = new Logs(batch: _batch, interval: _interval, apiClient: _apiClient);
            _resource = new Resource();
        }

        protected override void CloseTarget()
        {
            // TODO Custom Close Logic
            base.CloseTarget();
        }

        protected override void Write(LogEventInfo logEvent)
        {
            base.Write(logEvent);
            Dictionary<string, string> metaData = new Dictionary<string, string>();
            var currentLogLevel = logEvent.Level.ToString();
            metaData.Add(Constants.LogLevel, currentLogLevel);
            string formattedMessage =logEvent.FormattedMessage;
            if (Activity.Current != null)
                metaData = LogEventMetaData(Activity.Current);
             _logs.SendLogs(message: formattedMessage, resource: _resource,metadata:metaData);
        }

        private Dictionary<string, string> LogEventMetaData(Activity activity)
        {

            var traceID = activity.TraceId.ToString();
            var spanId = activity.SpanId.ToString();
            var operationName = activity.OperationName.ToString();

            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues.Add(Constants.TracesKey.TraceId, traceID);
            keyValues.Add(Constants.TracesKey.SpanId, spanId);
            keyValues.Add(Constants.TracesKey.OperationName, operationName);
            return keyValues;
        }

    }
}

