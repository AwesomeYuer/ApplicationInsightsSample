namespace Microshaoft;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
public class RequestResponseGuardMiddleware
{
    private RequestDelegate _next;
    //private readonly ILogger<WeatherForecastController> _logger;
    //private readonly TelemetryClient _telemetryClient;
    public RequestResponseGuardMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke
                        (
                            HttpContext context
                            , ILogger<RequestResponseGuardMiddleware> logger
                            , TelemetryClient telemetryClient

                        )
    {
        var log = $"{nameof(RequestResponseGuardMiddleware)}.Request.OnExecuting @ {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")}";
        logger.LogInformation(log);
        telemetryClient.TrackTrace(log, SeverityLevel.Information);
        context
            .Response
            .OnCompleted 
                (
                    () =>
                    {
                        var log = $"{nameof(RequestResponseGuardMiddleware)}.Response.{nameof(context.Response.OnCompleted)} @ {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")}";
                        logger.LogInformation(log);
                        telemetryClient.TrackTrace(log, SeverityLevel.Information);
                        return Task.CompletedTask;
                    }
                );
        context
            .Response
            .OnStarting
                (
                    () =>
                    {
                        var log = $"{nameof(RequestResponseGuardMiddleware)}.Response.{nameof(context.Response.OnStarting)} @ {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")}";
                        logger.LogInformation(log);
                        telemetryClient.TrackTrace(log, SeverityLevel.Information);
                        return Task.CompletedTask;
                    }
                );
        await _next.Invoke(context);
    }

}

public static class RequestResponseGuardMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestResponseGuardMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestResponseGuardMiddleware>();
    }
}

