using Microshaoft;
using Microsoft.AspNetCore.HttpLogging;
var builder = WebApplication.CreateBuilder(args);
builder
    .Logging
    .SetMinimumLevel
        (
            LogLevel
                    .Information
        );
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// The following line enables Application Insights telemetry collection.
builder.Services.AddApplicationInsightsTelemetry();

builder
    .Services
    .AddHttpLogging
        (
            logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("sec-ch-ua");
                logging.ResponseHeaders.Add("MyResponseHeader");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            }
        );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRequestResponseGuardMiddleware();

app.UseHttpLogging();

app.Run();
