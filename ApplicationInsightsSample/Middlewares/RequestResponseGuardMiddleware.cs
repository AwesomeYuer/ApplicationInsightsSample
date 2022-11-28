namespace Microshaoft;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;

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
        var request = context.Request;
        request.EnableBuffering();
        var response = context.Response;

        var controllerName = context.GetRouteData().Values["controller"]!.ToString();
        var actionName = context.GetRouteData().Values["action"]!.ToString();

        var log = $"{nameof(RequestResponseGuardMiddleware)}.Request.OnExecuting @ {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")}";
        logger.LogInformation(log);
        telemetryClient.TrackTrace(log, SeverityLevel.Information);
        context
            .Response
            .OnCompleted 
                (
                    () =>
                    {
                        var responseContentLength = response.ContentLength;
                        var log = $"{nameof(RequestResponseGuardMiddleware)}.Response.{nameof(context.Response.OnCompleted)}\r\nResponseContentLength:{responseContentLength} @ {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")}";
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
                        var requestBodyContent = string.Empty;
                        //should not use using 
                        using var requestBodyStream = request.Body;
                        if
                            (
                                requestBodyStream
                                                .CanRead
                                &&
                                requestBodyStream
                                                .CanSeek
                            )
                        {
                            requestBodyStream.Position = 0;
                            //should not use using
                            using var streamReader = new StreamReader(requestBodyStream);
                            requestBodyContent = streamReader.ReadToEndAsync().Result;
                            requestBodyStream.Position = 0;
                        }
                        #region Response
                        var responseBodyContent = string.Empty;
                        using var responseBodyStream = response.Body;
                        if
                            (
                                responseBodyStream
                                                .CanRead
                                &&
                                responseBodyStream
                                                .CanSeek
                            )
                        {
                            responseBodyStream.Position = 0;
                            using var streamReader = new StreamReader(responseBodyStream);
                            responseBodyContent = streamReader.ReadToEnd();
                            responseBodyStream.Position = 0;
                        }
                        #endregion
                        var log = $"{nameof(RequestResponseGuardMiddleware)}.Response.{nameof(context.Response.OnStarting)}\r\n"
                                + $"ControllerName:{controllerName}\r\n"
                                + $"ActionName:{actionName}\r\n"
                                + $"RequestBodyContent:\r\n{requestBodyContent}\r\n"
                                + $"ResponseBodyContent:\r\n{responseBodyContent}\r\n"
                                + $"@ TimeStamp:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")}";
                        logger.LogInformation(log);
                        telemetryClient.TrackTrace(log, SeverityLevel.Information);
                        return Task.CompletedTask;
                    }
                );

        var originalResponseBodyStream = response.Body;
        try
        {
            using var workingStream = new MemoryStream();
            response
                    .Body = workingStream;
            await
                _next(context);
            workingStream
                    .Position = 0;
            await
                workingStream
                        .CopyToAsync
                                (
                                    originalResponseBodyStream
                                );
        }
        finally
        {
            response
                    .Body = originalResponseBodyStream;
        }
    }
}

public static class RequestResponseGuardMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestResponseGuardMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestResponseGuardMiddleware>();
    }
}

