using Serilog;

namespace SearchService.API.Middleware
{
    public class RequestLoggingMiddleware
    {
        //private readonly RequestDelegate _next;
        //private readonly ILogger<RequestLoggingMiddleware> _logger;

        //public RequestLoggingMiddleware(
        //    RequestDelegate next,
        //    ILogger<RequestLoggingMiddleware> logger)
        //{
        //    _next = next;
        //    _logger = logger;
        //}

        //public async Task InvokeAsync(HttpContext context)
        //{
        //    _logger.LogInformation(
        //        "Request: {Method} {Path}",
        //        context.Request.Method,
        //        context.Request.Path
        //    );

        //    await _next(context); // move to next middleware

        //    _logger.LogInformation(
        //        "Response: {StatusCode}",
        //        context.Response.StatusCode
        //    );
        //}

        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Log.Information(
                "HTTP {Method} {Path} from {IP}",
                context.Request.Method,
                context.Request.Path,
                context.Connection.RemoteIpAddress?.ToString()
            );

            await _next(context);

            Log.Information(
                "Response {StatusCode} for {Path}",
                context.Response.StatusCode,
                context.Request.Path
            );
        }
    }

}
