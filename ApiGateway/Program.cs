//using Ocelot.DependencyInjection;
//using Ocelot.Middleware;
using Serilog;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/apigateway-.log",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Host.UseSerilog();

//builder.Configuration
//    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

//builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;          // max requests
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 2;
    });
});

var app = builder.Build();

//await app.UseOcelot();

app.UseRateLimiter();

app.MapReverseProxy();

app.MapGet("/", () => "Hello World!");

app.Run();
