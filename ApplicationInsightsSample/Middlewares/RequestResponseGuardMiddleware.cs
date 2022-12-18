namespace Microshaoft;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using System.Web;

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
                            HttpContext httpContext
                            , ILogger<RequestResponseGuardMiddleware> logger
                            , TelemetryClient telemetryClient
                        )
    {
        var request = httpContext.Request;
        request.EnableBuffering();
        var response = httpContext.Response;

        var controllerName = httpContext.GetRouteData().Values["controller"]!.ToString();
        var actionName = httpContext.GetRouteData().Values["action"]!.ToString();
        var requestPath = request.Path.ToString();
        var requestQueryString = request.QueryString.ToString();
        var requestRelativeUrl = $"{requestPath}";
        if (!string.IsNullOrEmpty(requestQueryString))
        {
            requestQueryString = HttpUtility.UrlDecode(requestQueryString);
            requestRelativeUrl += requestQueryString;
        }

        logger
            .LogOnDemand
                    (
                        LogLevel.Trace
                        , () =>
                        {
                            var log = $"{nameof(RequestResponseGuardMiddleware)}.Request.OnExecuting @ {DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}";
                            telemetryClient.TrackTrace(log, SeverityLevel.Information);
                            return log;
                        }
                    );
        
        httpContext
            .Response
            .OnCompleted 
                (
                    () =>
                    {
                        logger
                            .LogOnDemand
                                    (
                                        LogLevel.Trace
                                        , () =>
                                        {
                                            var responseContentLength = response.ContentLength;
                                            var log = $"{nameof(RequestResponseGuardMiddleware)}.Response.{nameof(httpContext.Response.OnCompleted)}\r\nResponseContentLength:{responseContentLength} @ {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")}";
                                            telemetryClient.TrackTrace(log, SeverityLevel.Information);
                                            return log;
                                        }
                                    );
                        return Task.CompletedTask;
                    }
                );
        httpContext
            .Response
            .OnStarting
                (
                    () =>
                    {
                        logger
                            .LogOnDemand
                                    (
                                        LogLevel.Trace
                                        , () =>
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
                                            var log =
                    $"""
{nameof(RequestResponseGuardMiddleware)}.Response.{nameof(httpContext.Response.OnStarting)}

RequestRelativeUrl: {requestRelativeUrl}
ControllerName: {controllerName}
ActionName: {actionName}
RequestBodyContent:
    {requestBodyContent}
ResponseBodyContent:
    {responseBodyContent}

@ TimeStamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}
""";
                                            telemetryClient.TrackTrace(log, SeverityLevel.Information);
                                            return log;
                                        }
                                    );
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
                _next(httpContext);
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
