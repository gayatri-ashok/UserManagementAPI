namespace UserManagementAPI
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var method = context.Request.Method;
            var path = context.Request.Path;

            await _next(context); // proceed with pipeline

            var statusCode = context.Response?.StatusCode;
            _logger.LogInformation("HTTP {Method} {Path} responded {StatusCode}", method, path, statusCode);
        }
    }
}
