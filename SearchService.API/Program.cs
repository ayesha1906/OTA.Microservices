using SearchService.API.Middleware;
using SearchService.API.Services;
using Serilog;
using Polly;
using Polly.Extensions.Http;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/searchservice-.log",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddHttpClient("HotelService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7028/");
})
.AddPolicyHandler(GetRetryPolicy())
.AddPolicyHandler(GetCircuitBreakerPolicy())
.AddPolicyHandler(GetTimeoutPolicy());
//builder.Services.AddHttpClient("HotelService", client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7028/");
//})
//.AddTransientHttpErrorPolicy(policy =>
//    policy.WaitAndRetryAsync(3, retry =>
//        TimeSpan.FromSeconds(Math.Pow(2, retry))))
//.AddTransientHttpErrorPolicy(policy =>
//    policy.CircuitBreakerAsync(
//        handledEventsAllowedBeforeBreaking: 2,
//        durationOfBreak: TimeSpan.FromSeconds(30)))
//.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(5));



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
});

builder.Services.AddHttpClient<AmadeusAuthService>(client =>
{
    client.BaseAddress = new Uri("https://test.api.amadeus.com");
});


builder.Services.AddScoped<ICacheService, RedisCacheService>();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
            retryCount: 2,
            sleepDurationProvider: retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
        );
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(10),
            onBreak: (outcome, timespan) =>
            {
                Console.WriteLine($"Circuit opened for {timespan.TotalSeconds}s");
            },
            onReset: () =>
            {
                Console.WriteLine("Circuit closed");
            }
        );
}

static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
{
    return Policy.TimeoutAsync<HttpResponseMessage>(5);
}

