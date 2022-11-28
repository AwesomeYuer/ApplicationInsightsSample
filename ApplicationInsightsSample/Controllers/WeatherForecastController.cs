namespace ApplicationInsightsSample.Controllers
{
    using Microshaoft;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("[controller]")]
    [TypeFilter(typeof(SimpleFilter))]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly TelemetryClient _telemetryClient;

        public WeatherForecastController
                            (
                                ILogger<WeatherForecastController> logger
                                , TelemetryClient telemetryClient
                            )
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var log = $"{nameof(WeatherForecastController)}.{HttpContext.GetRouteData().Values["action"]!.ToString()} @ {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")}";
            _logger.LogInformation(log);
            _telemetryClient.TrackTrace(log, SeverityLevel.Information);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost(Name = "PostWeatherForecast")]
        public IEnumerable<WeatherForecast> Post
                                            (
                                                [FromBody]
                                                string q = "*"
                                            )
        {
            var log = $"{nameof(WeatherForecastController)}.{HttpContext.GetRouteData().Values["action"]!.ToString()} @ {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff")}";
            _logger.LogInformation(log);
            _telemetryClient.TrackTrace(log, SeverityLevel.Information);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}