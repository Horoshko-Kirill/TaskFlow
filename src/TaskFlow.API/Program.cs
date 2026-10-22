using System.Text.Json;
using Prometheus;
using Prometheus.DotNetRuntime;
using TaskFlow.API.Middleware;
using TaskFlow.Application.DI;
using TaskFlow.Infrastructure.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Logging.AddJsonConsole(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss";
    options.UseUtcTimestamp = true;
    options.JsonWriterOptions = new JsonWriterOptions
    {
        Indented = false
    };
});

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(builder.Configuration["Frontend:Url"]!)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

var runtimeCollector = DotNetRuntimeStatsBuilder
    .Default()
    .StartCollecting();

app.Lifetime.ApplicationStopped.Register(
    runtimeCollector.Dispose);

app.Use(async (context, next) =>
{
    var logger = context.RequestServices
        .GetRequiredService<ILogger<Program>>();
    
    using var scope = logger.BeginScope(
        new Dictionary<string, object>
        {
            ["service.name"] = "taskflow-api",
            ["service.environment"] = app.Environment.EnvironmentName
        });

    var stopwatch = System.Diagnostics.Stopwatch.StartNew();

    try
    {
        await next();
    }
    finally
    {
        stopwatch.Stop();

        var statusCode = context.Response.StatusCode;

        var logLevel = statusCode >= 500
            ? LogLevel.Error
            : statusCode >= 400
                ? LogLevel.Warning
                : LogLevel.Information;

        logger.Log(
            logLevel,
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMs} ms",
            context.Request.Method,
            context.Request.Path,
            statusCode,
            stopwatch.ElapsedMilliseconds);
    }
});

app.UseRouting();

app.UseHttpMetrics();

app.UseCors("Frontend");

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapMetrics();

app.Run();