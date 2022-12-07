namespace Microshaoft
{
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.AspNetCore.Mvc.Filters;
    public class SimpleFilter : Attribute, IActionFilter
    {

        private readonly ILogger _logger;
        private readonly TelemetryClient _telemetryClient;

        public SimpleFilter
                        (
                            ILogger<SimpleFilter> logger
                            , TelemetryClient telemetryClient
                        )
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var log = $"""{nameof(SimpleFilter)}.{nameof(OnActionExecuting)} @ TimeStamp: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")}""";
            _logger.LogInformation(log);
            _telemetryClient.TrackTrace(log, SeverityLevel.Information);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var log = $"""{nameof(SimpleFilter)}.{nameof(OnActionExecuted)} @ TimeStamp: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")}""";
            _logger.LogInformation(log);
            _telemetryClient.TrackTrace(log, SeverityLevel.Information);
        }
    }
}

